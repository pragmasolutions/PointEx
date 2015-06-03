using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Framework.Common.Utility;
using Framework.Data.Helpers;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Service.Exceptions;

namespace PointEx.Service
{
    public class BenefitService : ServiceBase, IBenefitService
    {
        private readonly IClock _clock;
        private readonly IBranchOfficeService _branchOfficeService;

        public BenefitService(IPointExUow uow, IClock clock, IBranchOfficeService branchOfficeService)
        {
            _clock = clock;
            _branchOfficeService = branchOfficeService;
            Uow = uow;
        }

        public IQueryable<Benefit> GetAll()
        {
            return Uow.Benefits.GetAll();
        }

        public List<BenefitDto> GetAll(string sortBy, string sortDirection, string criteria, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<Benefit, bool>> where = x => ((string.IsNullOrEmpty(criteria) || x.Description.Contains(criteria) || x.Name.Contains(criteria)));

            var results = Uow.Benefits.GetAll(pagingCriteria, where, includes: b => b.Shop);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<BenefitDto>().ToList();
        }

        public Benefit GetById(int id)
        {
            return Uow.Benefits.Get(b => b.Id == id, b => b.BenefitBranchOffices.Select(bbo => bbo.BranchOffice));
        }

        public IQueryable<Benefit> GetAllByShopId(int shopId)
        {
            return Uow.Benefits.GetAll(b => b.ShopId == shopId);
        }

        public Benefit GetByName(string name)
        {
            return Uow.Benefits.Get(e => e.Name == name);
        }

        public void Create(Benefit benefit)
        {
            if (!IsNameAvailable(benefit.Description, benefit.Id))
            {
                throw new ApplicationException("Un Beneficio con el mismo nombre ya ha sido creado");
            }

            benefit.CreatedDate = _clock.Now;
            Uow.Benefits.Add(benefit);
            Uow.Commit();
        }

        public void Edit(Benefit benefit)
        {
            if (!IsNameAvailable(benefit.Description, benefit.Id))
            {
                throw new ApplicationException("Un Beneficio con el mismo nombre ya ha sido creado");
            }

            var currentBenefit = this.GetById(benefit.Id);

            foreach (var branchOffice in currentBenefit.BenefitBranchOffices.ToArray())
            {
                Uow.BenefitBranchOffice.Delete(branchOffice);
            }

            foreach (var branchOffice in benefit.BenefitBranchOffices)
            {
                Uow.BenefitBranchOffice.Add(branchOffice);
            }

            currentBenefit.ModifiedDate = _clock.Now;
            currentBenefit.Description = benefit.Description;
            currentBenefit.DiscountPercentage = benefit.DiscountPercentage;
            currentBenefit.DiscountPercentageCeiling = benefit.DiscountPercentageCeiling;
            currentBenefit.Name = benefit.Name;
            currentBenefit.DateFrom = benefit.DateFrom;
            currentBenefit.DateTo = benefit.DateTo;

            Uow.Benefits.Edit(currentBenefit);
            Uow.Commit();
        }

        public void Delete(int benefitId)
        {
            Uow.Benefits.Delete(benefitId);
            Uow.Commit();
        }

        public bool IsNameAvailable(string name, int id)
        {
            var currentBenefit = this.GetByName(name);

            if (currentBenefit == null)
            {
                return true;
            }

            return currentBenefit.Id == id;
        }


        public bool IsBenefitAvailableForBranchOffice(int benefitId, int branchOfficeId)
        {
            var benefit = Uow.Benefits.Get(b => b.Id == benefitId, b => b.BenefitBranchOffices);
            var branchOffice = _branchOfficeService.GetById(branchOfficeId);

            if (benefit == null || branchOffice == null)
            {
                throw new NotFoundException("No se encontro el beneficio");
            }

            if (!_branchOfficeService.GetByShopId(branchOffice.Shop.Id).Any())
            {
                return false;
            }

            return benefit.BenefitBranchOffices.Any(bo => bo.BranchOfficeId == branchOfficeId);
        }

        public IList<Benefit> GetOutstandingBenefits()
        {
            return Uow.Benefits.GetAll(b => b.DateTo >= _clock.Now, 
                b => b.Purchases,
                b => b.BenefitFiles).OrderBy(b => b.Purchases.Count).Take(6).ToList();
        }
    }
}
