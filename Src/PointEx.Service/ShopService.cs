using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointEx.Data.Interfaces;
using PointEx.Entities;

namespace PointEx.Service
{
    public class ShopService : ServiceBase, IShopService
    {
        public ShopService(IPointExUow uow)
        {
            Uow = uow;
        }

        public void Create(Shop shop)
        {
            Uow.Shops.Add(shop);
            Uow.Commit();
        }

        public void Edit(Shop shop)
        {
            Uow.Shops.Edit(shop);
            Uow.Commit();
        }
        
        public void Delete(int shopId)
        {
            Uow.Shops.Delete(shopId);
            Uow.Commit();
        }
    }
}
