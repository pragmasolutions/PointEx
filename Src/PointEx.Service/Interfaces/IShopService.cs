using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public interface IShopService
    {
        void Create(Shop shop);
        void Edit(Shop shop);
        void Delete(int shopId);
        IQueryable<Shop> GetAll();
        List<ShopDto> GetAll(string sortBy, string sortDirection, string criteria, int? category, int? townId, int pageIndex,
            int pageSize, out int pageTotal);
        Shop GetById(int id);
        Shop GetByUserId(string userId);
        void Dispose();
    }
}