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

        public PurchaseService(IPointExUow uow, IClock clock)
        {
            _clock = clock;
            Uow = uow;
        }

        public void Create(Purchase purchase)
        {
            purchase.PurchaseDate = _clock.Now;
            Uow.Purchases.Add(purchase);
            Uow.Commit();
        }

        public IList<PurchaseDto> GetTodayPurchasesByShopId(int shopId)
        {
            var purchases = Uow.Purchases.GetAll(whereClause: p => p.ShopId == shopId &&
               DbFunctions.TruncateTime(p.PurchaseDate) == DbFunctions.TruncateTime(_clock.Now), 
               includes: p => p.Card.Beneficiary);
            return purchases.Project().To<PurchaseDto>().ToList();
        }
    }
}
