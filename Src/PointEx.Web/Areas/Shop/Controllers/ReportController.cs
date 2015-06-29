using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Framework.Common.Extentensions;
using Framework.Report;
using PointEx.Entities;
using PointEx.Security;
using PointEx.Service;
using PointEx.Web.App_LocalResources;
using PointEx.Web.Areas.Shop.Models.Reports;
using PointEx.Web.Controllers;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Shop.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ReportController : ShopBaseController
    {
        private readonly IShopService _shopService;
        private readonly IEducationalInstitutionService _educationalInstitutionService;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IReportService _reportService;

        public ReportController(IShopService shopService, IEducationalInstitutionService educationalInstitutionService, 
                                IBeneficiaryService beneficiaryService, IReportService reportService)
        {
            _shopService = shopService;
            _educationalInstitutionService = educationalInstitutionService;
            _beneficiaryService = beneficiaryService;
            _reportService = reportService;
        }
        
        public ActionResult BenefitsUsed(ShopReportFiltersModel filters)
        {
            if (filters != null)
                filters.ReportName = "BenefitsUsed";

            return View(filters);
        }

        public ActionResult GenerateReportBenefitsUsed(ShopReportFiltersModel filters)
        {
            var reporteFactory = new ReportFactory();
            
            reporteFactory
                .SetParameter("From", filters.From.ToShortDateString(null))
                .SetParameter("To", filters.To.ToShortDateString(null));
            var shopId = PointExContext.Shop.Id;

            var points = _reportService.BenefitsUsed(filters.From.AbsoluteStart(), filters.To.AbsoluteEnd(), shopId);

            var chartDataset = _reportService.BenefitsUsedChart(filters.From.AbsoluteStart(), filters.To.AbsoluteEnd(), shopId);


            reporteFactory.SetDataSource("BenefitsUsedDataSet", points)
                          .SetDataSource("ChartDataSet", chartDataset)
                          .SetFullPath(Server.MapPath("~/Reports/BenefitsUsed.rdl"));

            byte[] reportFile = reporteFactory.Render(filters.ReportType);

            return File(reportFile, reporteFactory.MimeType);
        }
    }
}
