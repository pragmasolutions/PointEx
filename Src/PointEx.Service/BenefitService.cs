using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
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
using PointEx.Entities.Enums;
using PointEx.Security;

namespace PointEx.Service
{
    public class BenefitService : ServiceBase, IBenefitService
    {
        private readonly IClock _clock;
        private readonly IBranchOfficeService _branchOfficeService;
        private readonly INotificationService _notificationService;

        public BenefitService(IPointExUow uow, IClock clock, IBranchOfficeService branchOfficeService, INotificationService notificationService)
        {
            _clock = clock;
            _branchOfficeService = branchOfficeService;
            _notificationService = notificationService;
            Uow = uow;
        }

        public IQueryable<Benefit> GetAll()
        {
            return Uow.Benefits.GetAll(b => !b.IsDeleted);
        }

        public List<BenefitDto> GetAll(string sortBy, string sortDirection, int? categoryId, int? townId, int? shopId, string criteria, StatusEnum? statusId, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<Benefit, bool>> where =
                x =>
                    ((string.IsNullOrEmpty(criteria) || x.Description.Contains(criteria) || x.Name.Contains(criteria) || x.Shop.Name.Contains(criteria)) &&
                     (!shopId.HasValue || x.ShopId == shopId) &&
                     (!statusId.HasValue || x.StatusId == statusId.Value) &&
                     (!categoryId.HasValue || x.Shop.ShopCategories.Any(c => c.CategoryId == categoryId)) &&
                     (!townId.HasValue || x.Shop.TownId == townId ||
                     x.BenefitBranchOffices.Any(bo => bo.BranchOffice.TownId == townId)) &&
                     !x.IsDeleted);

            var results = Uow.Benefits.GetAll(pagingCriteria, where,
                //Includes
                b => b.Shop.ShopCategories,
                b => b.BenefitBranchOffices.Select(bbo => bbo.BranchOffice));

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<BenefitDto>().ToList();
        }

        public Benefit GetById(int id)
        {
            return Uow.Benefits.Get(b => b.Id == id && !b.IsDeleted,
                b => b.BenefitBranchOffices.Select(bbo => bbo.BranchOffice),
                b => b.Shop, b => b.Shop.User, b => b.BenefitType, b => b.Status);
        }

        public IQueryable<Benefit> GetAllByShopId(int shopId)
        {
            return Uow.Benefits.GetAll(b => b.ShopId == shopId && !b.IsDeleted && b.StatusId == StatusEnum.Approved);
        }

        public Benefit GetByName(string name)
        {
            return Uow.Benefits.Get(b => b.Name == name && !b.IsDeleted);
        }

        public void Create(Benefit benefit)
        {
            //if (!IsNameAvailable(benefit.Name, benefit.Id))
            //{
            //    throw new ApplicationException("Un Beneficio con el mismo nombre ya ha sido creado");
            //}

            benefit.CreatedDate = _clock.Now;
            benefit.StatusId = StatusEnum.Pending;
            Uow.Benefits.Add(benefit);
            Uow.Commit();
        }

        public async Task Edit(Benefit benefit, IPrincipal currentUser, string shopEmail, string theme)
        {
            //if (!IsNameAvailable(benefit.Name, benefit.Id))
            //{
            //    throw new ApplicationException("Un Beneficio con el mismo nombre ya ha sido creado");
            //}

            var currentBenefit = this.GetById(benefit.Id);

            var sendPendingConfirmationEmail = currentBenefit.StatusId == StatusEnum.Approved && currentUser.IsInRole(RolesNames.Shop);

            if (sendPendingConfirmationEmail)
            {
                await _notificationService.SendPendingBenefitEmail(benefit.Name, shopEmail, false, theme);
            }

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
            currentBenefit.Name = benefit.Name;
            currentBenefit.DateFrom = benefit.DateFrom;
            currentBenefit.DateTo = benefit.DateTo;
            currentBenefit.BenefitTypeId = benefit.BenefitTypeId;

            if (currentUser.IsInRole(RolesNames.Shop))
            {
                currentBenefit.StatusId = StatusEnum.Pending;
            }

            if ((benefit.BenefitTypeId == BenefitTypesEnum.Discount))
            {
                currentBenefit.DiscountPercentage = benefit.DiscountPercentage;
                currentBenefit.DiscountPercentageCeiling = benefit.DiscountPercentageCeiling;
            }
            else
            {
                currentBenefit.DiscountPercentage = null;
                currentBenefit.DiscountPercentageCeiling = null;
            }

            Uow.Benefits.Edit(currentBenefit);

            Uow.Commit();
        }

        public void Moderated(int benefitId, int statusId)
        {
            var currentBenefit = this.GetById(benefitId);
            currentBenefit.StatusId = (StatusEnum)statusId;

            Uow.Benefits.Edit(currentBenefit);

            Uow.Commit();
        }

        public void Delete(int benefitId)
        {
            if (CanDeleteBenefit(benefitId))
            {
                var sectionItems = Uow.SectionItems.GetAll(si => si.BenefitId == benefitId).ToList();

                foreach (var sectionItem in sectionItems)
                {
                    Uow.SectionItems.Delete(sectionItem);
                }

                var benefitFiles = Uow.BenefitFiles.GetAll(bf => bf.BenefitId == benefitId, bf => bf.File).ToList();

                foreach (var benefitFile in benefitFiles)
                {
                    Uow.FileContents.Delete(benefitFile.FileId);
                    Uow.Files.Delete(benefitFile.FileId);
                    Uow.BenefitFiles.Delete(benefitFile.Id);
                }

                var benefitsBranchOffices = Uow.BenefitBranchOffice.GetAll(bof => bof.BenefitId == benefitId).ToList();

                foreach (var benefitsBranchOffice in benefitsBranchOffices)
                {
                    Uow.BenefitBranchOffice.Delete(benefitsBranchOffice);
                }

                Uow.Benefits.Delete(benefitId);
            }
            else
            {
                var benefit = this.GetById(benefitId);

                benefit.IsDeleted = true;
            }

            Uow.Commit();
        }

        private bool CanDeleteBenefit(int benefitId)
        {
            return !Uow.Purchases.GetAll(p => p.BenefitId == benefitId).Any();
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
            var today = _clock.Now.AddMonths(-3);
            return Uow.Benefits.GetAll(b => (!b.DateTo.HasValue || b.DateTo >= today) && b.StatusId == StatusEnum.Approved && !b.IsDeleted,
                b => b.Purchases,
                b => b.Shop,
                b => b.BenefitType,
                b => b.BenefitFiles).OrderBy(b => b.Purchases.Count).Take(6).ToList();
        }

        public List<BenefitDto> GetBenefitByStatus(string sortBy, string sortDirection, int? categoryId, int? townId, int? shopId, StatusEnum status, string criteria, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<Benefit, bool>> where =
                x =>
                    ((string.IsNullOrEmpty(criteria) || x.Description.Contains(criteria) || x.Name.Contains(criteria) || x.Shop.Name.Contains(criteria)) &&
                     (!shopId.HasValue || x.ShopId == shopId) &&
                     (!categoryId.HasValue || x.Shop.ShopCategories.Any(c => c.CategoryId == categoryId)) &&
                     (!townId.HasValue || x.Shop.TownId == townId ||
                     x.BenefitBranchOffices.Any(bo => bo.BranchOffice.TownId == townId)) &&
                     !x.IsDeleted && x.StatusId == status);

            var results = Uow.Benefits.GetAll(pagingCriteria, where,
                //Includes
                b => b.Shop.ShopCategories,
                b => b.BenefitBranchOffices.Select(bbo => bbo.BranchOffice),
                b => b.BenefitType);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<BenefitDto>().ToList();
        }

        public Task<List<BenefitDto>> GetNearestBenefits(double latitude, double longitude, int? distance)
        {
            Expression<Func<Benefit, bool>> where =
                x =>!x.IsDeleted && x.StatusId == StatusEnum.Approved;

            var benefits = Uow.Benefits.GetAll(where,
                b => b.Shop, 
                b => b.Shop.ShopCategories,
                b => b.BenefitBranchOffices.Select(bbo => bbo.BranchOffice),
                b => b.BenefitType);

            var lon = longitude.ToString(CultureInfo.InvariantCulture);
            var lat = latitude.ToString(CultureInfo.InvariantCulture);

            distance = distance ?? int.Parse(ConfigurationManager.AppSettings["DefaultNearestLocationDistance"]);

            var point = DbGeography.PointFromText(string.Format("POINT({0} {1})", lon, lat), DbGeography.DefaultCoordinateSystemId);

            var nearest = from b in benefits
                        let region = point.Buffer(distance)
                        where b.Shop.Location != null && SqlSpatialFunctions.Filter(b.Shop.Location, region) == true
                        select b; 

            return nearest.Project().To<BenefitDto>().ToListAsync();
        }
    }
}
