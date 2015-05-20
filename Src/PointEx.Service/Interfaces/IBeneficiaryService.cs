using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public interface IBeneficiaryService
    {
        void Create(Beneficiary student);
        void Edit(Beneficiary student);
        void Delete(int studentId);
        IQueryable<Beneficiary> GetAll();
        List<BeneficiaryDto> GetAll(string sortBy, string sortDirection, string criteria, int? category, int? townId, int pageIndex,
            int pageSize, out int pageTotal);
        Beneficiary GetById(int id);
        void Dispose();
    }
}