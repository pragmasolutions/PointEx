using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Security.Model;

namespace PointEx.Service
{
    public interface IBeneficiaryService : IServive
    {
        Task Create(Beneficiary beneficiary, ApplicationUser applicationUser);
        void Edit(Beneficiary beneficiary);
        void Delete(int beneficiaryId);
        IQueryable<Beneficiary> GetAll();
        List<BeneficiaryDto> GetAll(string sortBy, string sortDirection, string criteria, int? townId, int? educationalInstitutionId, int pageIndex,
            int pageSize, out int pageTotal);
        Beneficiary GetById(int id);

        Beneficiary GetByUserId(string userId);

        List<PointTransaction> GetTransactions(int beneficiaryId);
    }
}