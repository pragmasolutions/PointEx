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
    }
}
