using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointEx.Data.Interfaces;
using PointEx.Entities;

namespace PointEx.Service
{
    public class TownService : ServiceBase, ITownService
    {
        public TownService(IPointExUow uow)
        {
            Uow = uow;
        }

        public IQueryable<Town> GetAll()
        {
            return Uow.Towns.GetAll();
        }

        public Town GetById(int id)
        {
            return Uow.Towns.Get(id);
        }
    }
}
