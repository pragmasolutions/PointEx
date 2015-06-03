using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public interface IBenefitService
    {
        IQueryable<Benefit> GetAll();

        IList<Benefit> GetOutstandingBenefits();

        Benefit GetById(int id);

        IQueryable<Benefit> GetAllByShopId(int shopId);

        List<BenefitDto> GetAll(string sortBy, string sortDirection, string criteria,
            int pageIndex, int pageSize, out int pageTotal);

        void Create(Benefit benefit);

        void Edit(Benefit benefit);

        void Delete(int enefitId);

        bool IsNameAvailable(string name, int id);

        bool IsBenefitAvailableForBranchOffice(int benefitId, int branchOfficeId);
    }
}