using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Security.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PointEx.Entities.Enums;

namespace PointEx.Service
{
    public interface IBeneficiaryService : IServive
    {
        Task Create(Beneficiary beneficiary, ApplicationUser applicationUser, string theme);
        Task Create(Beneficiary beneficiary, ApplicationUser applicationUser, string theme, ExternalLoginInfo info);
        void Edit(Beneficiary beneficiary);
        void Delete(int beneficiaryId);
        IQueryable<Beneficiary> GetAll();
        List<BeneficiaryDto> GetAll(string sortBy, string sortDirection, string criteria, int? townId, int? educationalInstitutionId, bool? deleted, int pageIndex,
            int pageSize, out int pageTotal);
        Beneficiary GetById(int id);

        Beneficiary GetByUserId(string userId);

        List<PointTransaction> GetTransactions(int beneficiaryId);

        IList<PointsExchange> GetPurchaseByBeneficiaryId(int beneficiaryId);

        void Moderated(int beneficiaryId, int statusId);

        List<BeneficiaryDto> GetBeneficiaryByStatus(string sortBy, string sortDirection, int? townId, StatusEnum status, string criteria, int pageIndex, int pageSize, out int pageTotal);
    }
}