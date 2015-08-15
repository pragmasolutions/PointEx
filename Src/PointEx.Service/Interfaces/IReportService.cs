using System;
using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface IReportService
    {
        IList<RptPurchases> Purchases(DateTime? from, DateTime? to, int? shopId, int? educationalInstitutionId);

        IList<RptMostExchangedPrizes> MostExchangedPrizes(DateTime? from, DateTime? to, int? educationalInstitutionId);

        IList<RptGeneratedPoints> GeneratedPoints(DateTime? from, DateTime? to, int? shopId, int? beneficiaryId, int? educationalInstitutionId);

        IList<RptMostUsedBenefits> MostUsedBenefits(DateTime? from, DateTime? to, int? shopId,
            int? educationalInstitutionId);

        IList<RptBenefitsUsed> BenefitsUsed(DateTime? from, DateTime? to, int shopId);

        IList<RptBenefitsUsedChart> BenefitsUsedChart(DateTime? from, DateTime? to, int shopId);

        IList<RptBeneficiaries> Beneficiaries(DateTime? from, DateTime? to, int? sex, int? townId,
            int? educationalInstitutionId);
    }
}
