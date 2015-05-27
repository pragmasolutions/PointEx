using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface IBenefitFileService
    {
        IList<BenefitFile> GetByBenefitId(int benefitId);

        void Create(BenefitFile benefitFile);

        void Create(IList<BenefitFile> benefitFiles);

        void Delete(int benefitFileId);

        void Order(int benefitId, IList<int> imageIdsOrdered);
    }
}