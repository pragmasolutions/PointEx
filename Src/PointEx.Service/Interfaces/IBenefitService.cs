using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Entities.Enums;

namespace PointEx.Service
{
    public interface IBenefitService
    {
        IQueryable<Benefit> GetAll();

        IList<Benefit> GetOutstandingBenefits();

        Benefit GetById(int id);

        IQueryable<Benefit> GetAllByShopId(int shopId);

        List<BenefitDto> GetAll(string sortBy, string sortDirection, int? categoryId,int? townId, int? shopId, string criteria,
            StatusEnum? benefitStatusId, int pageIndex, int pageSize, out int pageTotal);

        void Create(Benefit benefit);

        Task Edit(Benefit benefit, IPrincipal currentUser, string shopEmail, string theme);

        void Moderated(int benefitId, int statusId);

        void Delete(int enefitId);

        bool IsNameAvailable(string name, int id);

        bool IsBenefitAvailableForBranchOffice(int benefitId, int branchOfficeId);

        List<BenefitDto> GetBenefitByStatus(string sortBy, string sortDirection, int? categoryId, int? townId, int? shopId, StatusEnum status, string criteria,
            int pageIndex, int pageSize, out int pageTotal);

        List<BenefitDto> GetNearestBenefits(double latitude, double longitude, int distance);
    }
}