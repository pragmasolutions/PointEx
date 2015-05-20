using System.Linq;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface IEducationalInstitutionService
    {
        IQueryable<EducationalInstitution> GetAll();

        EducationalInstitution GetById(int id);
    }
}