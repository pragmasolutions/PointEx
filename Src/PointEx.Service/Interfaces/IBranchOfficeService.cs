using System.Collections.Generic;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface IBranchOfficeService : IServive
    {
        void Create(BranchOffice branchOffice);
        void Edit(BranchOffice branchOffice);
        void Delete(int branchOfficeId);
        BranchOffice GetById(int id);
        IList<BranchOffice> GetByShopId(int shopId);
    }
}