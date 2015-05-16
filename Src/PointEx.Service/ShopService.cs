using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Common.Utility;
using PointEx.Data.Interfaces;
using PointEx.Entities;

namespace PointEx.Service
{
    public class ShopService : ServiceBase, IShopService
    {
        private readonly IClock _clock;

        public ShopService(IPointExUow uow,IClock clock)
        {
            _clock = clock;
            Uow = uow;
        }

        public void Create(Shop shop)
        {
            shop.CreatedDate = _clock.Now;
            Uow.Shops.Add(shop);
            Uow.Commit();
        }

        public void Edit(Shop shop)
        {
            shop.ModifiedDate = _clock.Now;
            Uow.Shops.Edit(shop);
            Uow.Commit();
        }
        
        public void Delete(int shopId)
        {
            Uow.Shops.Delete(shopId);
            Uow.Commit();
        }

        public IQueryable<Shop> GetAll()
        {
            return Uow.Shops.GetAll();
        }

        public Shop GetById(int id)
        {
            return Uow.Shops.Get(id);
        }
    }
}
