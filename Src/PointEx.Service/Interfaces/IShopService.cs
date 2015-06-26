using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Security.Model;

namespace PointEx.Service
{
    public interface IShopService
    {
        Task Create(Shop shop, ApplicationUser applicationUser);
        void Edit(Shop shop);
        void Delete(int shopId);
        IQueryable<Shop> GetAll();
        List<ShopDto> GetAll(string sortBy, string sortDirection, string criteria, int? category, int? townId, bool? deleted, int pageIndex,
            int pageSize, out int pageTotal);
        Shop GetById(int id);
        Shop GetByUserId(string userId);
        void Dispose();
    }
}