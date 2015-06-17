using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Framework.Common.Extentensions;
using Framework.Report;
using PointEx.Entities;
using PointEx.Service;
using PointEx.Web.App_LocalResources;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Admin.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ReportController : AdminBaseController
    {
        private readonly IShopService _shopService;
        private readonly IEducationalInstitutionService _educationalInstitutionService;
        private readonly IReportService _reportService;

        public ReportController(IShopService shopService, IEducationalInstitutionService educationalInstitutionService, IReportService reportService)
        {
            _shopService = shopService;
            _educationalInstitutionService = educationalInstitutionService;
            _reportService = reportService;
        }

        public ActionResult Purchases(ReportFiltersModel filters)
        {
            return View(filters);
        }

        public ActionResult GenerateReportPurchases(ReportFiltersModel filters)
        {
            var reporteFactory = new ReportFactory();

            var shopName = filters.ShopId.HasValue ? _shopService.GetById(filters.ShopId.Value).Name : PointExResources.LabelAll;
            var educationalInstitutionName = filters.EducationalInstitutionId.HasValue ? _educationalInstitutionService.GetById(filters.EducationalInstitutionId.Value).Name : PointExResources.LabelAll;

            reporteFactory
                .SetParameter("From", filters.From.ToShortDateString(null))
                .SetParameter("To", filters.To.ToShortDateString(null))
                .SetParameter("EducationalInstitutionName", educationalInstitutionName)
                .SetParameter("ShopName", shopName);

            var purchaces = _reportService.Purchases(filters.From.AbsoluteStart(), filters.To.AbsoluteEnd(), filters.ShopId,
                filters.EducationalInstitutionId);

            reporteFactory.SetDataSource("PurchasesDataSet", purchaces)
                          .SetFullPath(Server.MapPath("~/Reports/Purchases.rdl"));

            byte[] reportFile = reporteFactory.Render(filters.ReportType);

            return File(reportFile, reporteFactory.MimeType);
        }
    }
}
