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

        IList<RptMostUsedBenefits> MostUsedBenefits(DateTime? from, DateTime? to, int? shopId,
            int? educationalInstitutionId);
    }
}
