using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Framework.Common.Utility;
using Framework.Data.Helpers;
using Microsoft.AspNet.Identity;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Security;
using PointEx.Security.Managers;
using PointEx.Security.Model;

namespace PointEx.Service
{
    public class BeneficiaryService : ServiceBase, IBeneficiaryService
    {
        private readonly ApplicationUserManager _userManager;
        private readonly INotificationService _notificationService;
        private readonly IClock _clock;

        public BeneficiaryService(IPointExUow uow, ApplicationUserManager userManager,INotificationService notificationService, IClock clock)
        {
            _userManager = userManager;
            _notificationService = notificationService;
            _clock = clock;
            Uow = uow;
        }

        public async Task Create(Beneficiary beneficiary, ApplicationUser applicationUser)
        {
            using (var trasactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (_userManager.FindByEmail(applicationUser.Email) != null)
                    {
                        throw new ApplicationException("Ya existe un usuario con ese email.");
                    }

                    var result = await _userManager.CreateAsync(applicationUser);

                    if (!result.Succeeded)
                    {
                        throw new ApplicationException(result.Errors.FirstOrDefault());
                    }

                    await _userManager.AddToRoleAsync(applicationUser.Id, RolesNames.Beneficiary);

                    await _notificationService.SendAccountConfirmationEmail(applicationUser.Id);

                    beneficiary.CreatedDate = _clock.Now;
                    beneficiary.UserId = applicationUser.Id;
                    Uow.Beneficiaries.Add(beneficiary);

                    await Uow.CommitAsync();

                    trasactionScope.Complete();
                }
                catch (Exception ex)
                {
                    trasactionScope.Dispose();
                    throw ex;
                }
            }
        }

        public void Edit(Beneficiary beneficiary)
        {
            beneficiary.ModifiedDate = _clock.Now;
            Uow.Beneficiaries.Edit(beneficiary);
            Uow.Commit();
        }

        public void Delete(int beneficiaryId)
        {
            var beneficiary = this.GetById(beneficiaryId);
            var user = Uow.Users.Get(u => u.Id == beneficiary.UserId);

            Uow.Beneficiaries.Delete(beneficiary);
            Uow.Users.Delete(user);

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


        public List<PointTransaction> GetTransactions(int beneficiaryId)
        {
            var transactions = Uow.DbContext.BeneficiaryPurchasesAndExchanges(beneficiaryId).OrderBy(t => t.TransactionDate).ToList();
            var count = 0;
            foreach (var transaction in transactions)
            {
                count += transaction.Credit.GetValueOrDefault() - transaction.Debit.GetValueOrDefault();
                transaction.Total = count;
            }
            return transactions.OrderByDescending(t => t.TransactionDate).ToList();
        }
    }
}
