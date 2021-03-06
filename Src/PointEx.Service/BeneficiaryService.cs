﻿using System;
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
        private readonly IPurchaseService _purchaseService;
        private readonly INotificationService _notificationService;
        private readonly ICardService _cardService;
        private readonly IClock _clock;

        public BeneficiaryService(IPointExUow uow, ApplicationUserManager userManager, 
            IPurchaseService purchaseService,
            INotificationService notificationService, IClock clock, ICardService cardService)
        {
            _userManager = userManager;
            _purchaseService = purchaseService;
            _notificationService = notificationService;
            _cardService = cardService;
            _clock = clock;
            Uow = uow;
        }

        public async Task Create(Beneficiary beneficiary, ApplicationUser applicationUser, string theme)
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

                    try
                    {
                        await _notificationService.SendAccountConfirmationEmail(applicationUser.Id,theme);
                    }
                    catch (Exception)
                    {

                    }

                    var card = new Card();
                    card.IssueDate = _clock.Now;
                    card.Number = _cardService.GenerateNumber(beneficiary);
                    card.ExpirationDate = _cardService.CalculateExpirationDate(beneficiary.BirthDate.GetValueOrDefault());
                    beneficiary.Cards.Add(card);

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
            var currentBeneficiary = this.GetById(beneficiary.Id);

            currentBeneficiary.Address = beneficiary.Address;
            currentBeneficiary.BirthDate = beneficiary.BirthDate;
            currentBeneficiary.EducationalInstitutionId = beneficiary.EducationalInstitutionId;
            currentBeneficiary.Name = beneficiary.Name;
            currentBeneficiary.IdentificationNumber = beneficiary.IdentificationNumber;
            currentBeneficiary.TownId = beneficiary.TownId;
            currentBeneficiary.ModifiedDate = _clock.Now;
            currentBeneficiary.Sex = beneficiary.Sex;
            currentBeneficiary.TelephoneNumber = beneficiary.TelephoneNumber;

            Uow.Beneficiaries.Edit(currentBeneficiary);
            Uow.Commit();
        }

        public void Delete(int beneficiaryId)
        {
            var beneficiary = this.GetById(beneficiaryId);
            var user = Uow.Users.Get(u => u.Id == beneficiary.UserId);

            if (CanRemoveBeneficiary(beneficiaryId))
            {
                foreach (var card in beneficiary.Cards.ToArray())
                {
                    Uow.Cards.Delete(card);
                    beneficiary.Cards.Remove(card);
                }

                Uow.Beneficiaries.Delete(beneficiary);
                Uow.Users.Delete(user);
            }
            else
            {
                beneficiary.IsDeleted = true;
                user.IsDeleted = true;
            }

            Uow.Commit();
        }

        public IQueryable<Beneficiary> GetAll()
        {
            return Uow.Beneficiaries.GetAll(whereClause: null, includes: s => s.Town);
        }

        public Beneficiary GetById(int id)
        {
            return Uow.Beneficiaries.Get(b => b.Id == id, s => s.Town, b => b.User, b => b.Town,
                                                                                b => b.Cards,
                                                                                b => b.Cards.Select(c => c.Purchases),
                                                                                b => b.EducationalInstitution,
                                                                                b => b.PointsExchanges);
        }

        public Beneficiary GetByUserId(string userId)
        {
            return Uow.Beneficiaries.Get(b => b.UserId == userId, s => s.Town, b => b.User, b => b.Town,
                                                                                b => b.Cards,
                                                                                b => b.Cards.Select(c => c.Purchases),
                                                                                b => b.EducationalInstitution,
                                                                                b => b.PointsExchanges);
        }

        public List<BeneficiaryDto> GetAll(string sortBy, string sortDirection, string criteria, int? townId, int? educationalInstitutionId, bool? deleted, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<Beneficiary, bool>> where =
                x => ((string.IsNullOrEmpty(criteria) || x.Name.Contains(criteria)) &&
                      (!townId.HasValue || x.TownId == townId) &&
                      (!educationalInstitutionId.HasValue || x.EducationalInstitutionId == educationalInstitutionId) &&
                      (!deleted.HasValue || x.IsDeleted == deleted));

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

        public IList<PointsExchange> GetPurchaseByBeneficiaryId(int beneficiaryId)
        {
            return Uow.PointsExchanges.GetAll(pe => pe.BeneficiaryId == beneficiaryId).ToList();
        }

        private bool CanRemoveBeneficiary(int beneficiaryId)
        {
            var prizeExachanges = this.GetPurchaseByBeneficiaryId(beneficiaryId);

            if (prizeExachanges.Any())
            {
                return false;
            }

            var purchases = _purchaseService.GetAllByBeneficiaryId(beneficiaryId);

            if (purchases.Any())
            {
                return false;
            }

            return true;
        }
    }
}
