using System.Linq;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface ICategoryService : IServive
    {
        IQueryable<Category> GetAll();
        IQueryable<Category> GetAllByShopId(int shopId);
        Category GetById(int id);
    }
}
