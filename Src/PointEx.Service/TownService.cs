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

        public void Create(Town town)
        {
            Uow.Towns.Add(town);
            Uow.Commit();
        }

        public IQueryable<Town> GetAll()
        {
            return Uow.Towns.GetAll();
        }

        public Town GetById(int id)
        {
            return Uow.Towns.Get(id);
        }

        public Town GetByName(string name)
        {
            var town = Uow.Towns.GetAll().FirstOrDefault(t => t.Name.Contains(name.ToUpper()));
            if (town == null)
            {
                var addTown = new Town() { Name = name };
                this.Create(addTown);
                return addTown;
            }

            return town;
        }
    }
}
