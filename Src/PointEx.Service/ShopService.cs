using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Framework.Common.Utility;
using PointEx.Data.Interfaces;
using PointEx.Entities;

namespace PointEx.Service
{
    public class ShopService : ServiceBase, IShopService
    {
        private readonly IClock _clock;
        private readonly ICategoryService _categoryService;

        public ShopService(IPointExUow uow, IClock clock, ICategoryService categoryService)
        {
            _clock = clock;
            _categoryService = categoryService;
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
            var currentShop = GetById(shop.Id);

            Mapper.Map(shop, currentShop);

            shop.ModifiedDate = _clock.Now;
            Uow.Shops.Edit(shop);
            Uow.Commit();
        }

        public void Delete(int shopId)
        {
            var shop = this.GetById(shopId);
            shop.Categories.Clear();
            Uow.Shops.Delete(shop);
            Uow.Commit();
        }

        public IQueryable<Shop> GetAll()
        {
            return Uow.Shops.GetAll(whereClause: null, includes: s => s.Town);
        }

        public Shop GetById(int id)
        {
            return Uow.Shops.Get(s => s.Id == id, s => s.Categories);
        }
    }
}
