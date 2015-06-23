using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using Framework.Common.Utility;
using Framework.Data.Helpers;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public class PurchaseService : ServiceBase, IPurchaseService
    {
        private readonly IClock _clock;
        private readonly IBenefitService _benefitService;

        public PurchaseService(IPointExUow uow, IClock clock, IBenefitService benefitService)
        {
            _clock = clock;
            _benefitService = benefitService;
            Uow = uow;
        }

        public void Create(Purchase purchase)
        {
            if (purchase.BranchOfficeId.HasValue)
            {
                if (!_benefitService.IsBenefitAvailableForBranchOffice(purchase.BenefitId, purchase.BranchOfficeId.Value))
                {
                    throw new ApplicationException("El Beneficio no esta disponible para la sucursal");
                }
            }

            purchase.PurchaseDate = _clock.Now;
            Uow.Purchases.Add(purchase);
            Uow.Commit();
        }

        public IList<PurchaseDto> GetTodayPurchasesByShopId(int shopId, int? branchOfficeId)
        {
            var purchases = Uow.Purchases.GetAll(p => p.ShopId == shopId &&
               DbFunctions.TruncateTime(p.PurchaseDate) == DbFunctions.TruncateTime(_clock.Now) &&
               (!branchOfficeId.HasValue || p.BranchOfficeId == branchOfficeId),
                p => p.Card.Beneficiary, p => p.BranchOffice);
            return purchases.Project().To<PurchaseDto>().ToList();
        }

        public IList<Purchase> GetAllByBeneficiaryId(int beneficiaryId)
        {
            return Uow.Purchases.GetAll(p => p.Card.BeneficiaryId == beneficiaryId, p => p.Card).ToList();
        }
    }
}
