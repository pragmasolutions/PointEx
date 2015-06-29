using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Framework.Common.Utility;
using Framework.Data.Helpers;
using Microsoft.AspNet.Identity;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Security;
using PointEx.Security.Managers;
using PointEx.Security.Model;

namespace PointEx.Service
{
    public class ReportService : ServiceBase, IReportService
    {

        public ReportService(IPointExUow uow)
        {
            Uow = uow;
        }

        public IList<RptPurchases> Purchases(DateTime? from, DateTime? to, int? shopId, int? educationalInstitutionId)
        {
            return Uow.DbContext.RptPurchases(from, to, shopId, educationalInstitutionId).ToList();
        }

        public IList<RptMostExchangedPrizes> MostExchangedPrizes(DateTime? from, DateTime? to, int? educationalInstitutionId)
        {
            return Uow.DbContext.RptMostExchangedPrizes(from, to, educationalInstitutionId).ToList();
        }


        public IList<RptGeneratedPoints> GeneratedPoints(DateTime? from, DateTime? to, int? shopId, int? beneficiaryId, int? educationalInstitutionId)
        {
            return Uow.DbContext.RptGeneratedPoints(from, to, shopId, beneficiaryId, educationalInstitutionId).ToList();
        }

        public IList<RptMostUsedBenefits> MostUsedBenefits(DateTime? from, DateTime? to, int? shopId, int? educationalInstitutionId)
        {
            return Uow.DbContext.RptMostUsedBenefits(from, to,shopId, educationalInstitutionId).ToList();
        }


        public IList<RptBenefitsUsed> BenefitsUsed(DateTime? from, DateTime? to, int shopId)
        {
            return Uow.DbContext.RptBenefitsUsed(from, to, shopId).ToList();
        }

        public IList<RptBenefitsUsedChart> BenefitsUsedChart(DateTime? from, DateTime? to, int shopId)
        {
            return Uow.DbContext.RptBenefitsUsedChart(from, to, shopId).ToList();
        }
    }
}
