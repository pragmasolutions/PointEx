using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointEx.Data;
using PointEx.Data.Interfaces;
using PointEx.Entities;

namespace PointEx.Service
{

    public class CategoryService : ServiceBase, ICategoryService
    {
        public CategoryService(IPointExUow uow)
        {
            Uow = uow;
        }

        public IQueryable<Category> GetAll()
        {
            return Uow.Categories.GetAll();
        }

        public IQueryable<Category> GetAllByShopId(int shopId)
        {
            return Uow.Categories.GetAll(c => c.Shops.Any(s => s.Id == shopId));
        }

        public Category GetById(int id)
        {
            return Uow.Categories.Get(id);
        }
    }
}
