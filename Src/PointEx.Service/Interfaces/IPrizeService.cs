using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public interface IPrizeService
    {
        IQueryable<Prize> GetAll();

        Prize GetById(int id);

        List<PrizeDto> GetAll(string sortBy, string sortDirection, string criteria,
            int pageIndex, int pageSize, out int pageTotal);

        void Create(Prize educationalInstitution);

        void Edit(Prize educationalInstitution);

        void Delete(int educationalInstitutionId);

        bool IsNameAvailable(string name, int id);
    }
}