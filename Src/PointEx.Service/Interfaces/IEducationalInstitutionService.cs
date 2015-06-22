using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public interface IEducationalInstitutionService
    {
        IQueryable<EducationalInstitution> GetAll();

        EducationalInstitution GetById(int id);

        List<EducationalInstitutionDto> GetAll(string sortBy, string sortDirection, string criteria, int? townId,
            int pageIndex, int pageSize, out int pageTotal);

        void Create(EducationalInstitution educationalInstitution);

        void Edit(EducationalInstitution educationalInstitution);

        void Delete(int educationalInstitutionId);

        bool IsNameAvailable(string name, int id);

        bool CanRemove(int educationInstitutionId);
    }
}