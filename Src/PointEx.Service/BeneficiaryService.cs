using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper.QueryableExtensions;
using Framework.Common.Utility;
using Framework.Data.Helpers;
using Microsoft.AspNet.Identity;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Security.Managers;
using PointEx.Security.Model;

namespace PointEx.Service
{
    public class BeneficiaryService : ServiceBase, IBeneficiaryService
    {
        private readonly ApplicationUserManager _userManager;
        private readonly IClock _clock;

        public BeneficiaryService(IPointExUow uow, ApplicationUserManager userManager, IClock clock)
        {
            _userManager = userManager;
            _clock = clock;
            Uow = uow;
        }

        public async Task Create(Beneficiary beneficiary, ApplicationUser applicationUser, string password)
        {
            using (var trasactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = await _userManager.CreateAsync(applicationUser, password);

                    if (!result.Succeeded)
                    {
                        throw new ApplicationException(result.Errors.FirstOrDefault());
                    }

                    beneficiary.CreatedDate = _clock.Now;
                    beneficiary.UserId = applicationUser.Id;
                    Uow.Beneficiaries.Add(beneficiary);

                    await Uow.CommitAsync();

                    trasactionScope.Complete();
                }
                catch (Exception)
                {
                    trasactionScope.Dispose();
                    throw;
                }
            }
        }

        public void Edit(Beneficiary beneficiary)
        {
            beneficiary.ModifiedDate = _clock.Now;
            Uow.Beneficiaries.Edit(beneficiary);
            Uow.Commit();
        }

        public void Delete(int shopId)
        {
            var beneficiary = this.GetById(shopId);
            Uow.Beneficiaries.Delete(beneficiary);
            Uow.Commit();
        }

        public IQueryable<Beneficiary> GetAll()
        {
            return Uow.Beneficiaries.GetAll(whereClause: null, includes: s => s.Town);
        }

        public Beneficiary GetById(int id)
        {
            return Uow.Beneficiaries.Get(s => s.Id == id, s => s.Town, s => s.EducationalInstitution);
        }

        public List<BeneficiaryDto> GetAll(string sortBy, string sortDirection, string criteria, int? townId, int? educationalInstitutionId, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<Beneficiary, bool>> where = x => ((string.IsNullOrEmpty(criteria) || x.Name.Contains(criteria)) &&
                                                             (!townId.HasValue || x.TownId == townId) &&
                                                             (!educationalInstitutionId.HasValue || x.EducationalInstitutionId == educationalInstitutionId));

            var results = Uow.Beneficiaries.GetAll(pagingCriteria,
                                                    where,
                                                     s => s.Town, s => s.EducationalInstitution);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<BeneficiaryDto>().ToList();
        }
    }
}
