using System.Linq;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface IShopService
    {
        void Create(Shop shop);
        void Edit(Shop shop);
        void Delete(int shopId);
        IQueryable<Shop> GetAll();
        Shop GetById(int id);
        void Dispose();
    }
}