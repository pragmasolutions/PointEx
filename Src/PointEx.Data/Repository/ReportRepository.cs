using System;
using System.Data.Entity;
using System.Linq;
using Framework.Data.EntityFramework.Repository;
using PointEx.Data.Interfaces;
using PointEx.Entities;

namespace PointEx.Data.Repository
{
    public class ReportRepository : EFBaseRepository, IReportRepository
    {
        public ReportRepository(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<RptPurchases> Purchases(DateTime? from, DateTime? to, int? shopId, int? educationalInstitutionId)
        {
            throw new NotImplementedException();
        }
    }
}