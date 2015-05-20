using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointEx.Data.Interfaces;
using PointEx.Entities;

namespace PointEx.Service
{
    public class EducationalInstitutionService : ServiceBase, IEducationalInstitutionService
    {
        public EducationalInstitutionService(IPointExUow uow)
        {
            Uow = uow;
        }

        public IQueryable<EducationalInstitution> GetAll()
        {
            return Uow.EducationalInstitutions.GetAll();
        }

        public EducationalInstitution GetById(int id)
        {
            return Uow.EducationalInstitutions.Get(id);
        }
    }
}
