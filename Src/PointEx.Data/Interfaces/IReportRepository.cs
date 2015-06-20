using System;
using System.Linq;
using PointEx.Entities;

namespace PointEx.Data.Interfaces
{
    public interface IReportRepository
    {
        IQueryable<RptPurchases> Purchases(DateTime? from, DateTime? to, int? shopId, int? educationalInstitutionId);

        IQueryable<RptGeneratedPoints> GeneratedPoints(DateTime? from, DateTime? to, int? shopId, int? beneficiaryId, int? educationalInstitutionId);
    }
}
