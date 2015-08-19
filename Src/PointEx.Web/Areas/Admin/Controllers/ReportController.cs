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
using PointEx.Web.Configuration;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Admin.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ReportController : AdminBaseController
    {
        private readonly IShopService _shopService;
        private readonly IEducationalInstitutionService _educationalInstitutionService;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IReportService _reportService;
        private readonly ITownService _townService;

        public ReportController(IShopService shopService, IEducationalInstitutionService educationalInstitutionService,
                                IBeneficiaryService beneficiaryService, IReportService reportService,ITownService townService)
        {
            _shopService = shopService;
            _educationalInstitutionService = educationalInstitutionService;
            _beneficiaryService = beneficiaryService;
            _reportService = reportService;
            _townService = townService;
        }

        public ActionResult Purchases(ReportFiltersModel filters)
        {
            if (filters != null)
                filters.ReportName = "Purchases";

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
                .SetParameter("ShopName", shopName)
                .SetParameter("ShowEducationalInstitution", AppSettings.ShowEducationalInstitution.ToString());

            var purchaces = _reportService.Purchases(filters.From.AbsoluteStart(), filters.To.AbsoluteEnd(), filters.ShopId,
                filters.EducationalInstitutionId);

            reporteFactory.SetDataSource("PurchasesDataSet", purchaces)
                          .SetFullPath(Server.MapPath("~/Reports/Purchases.rdl"));

            byte[] reportFile = reporteFactory.Render(filters.ReportType);

            return File(reportFile, reporteFactory.MimeType);
        }

        public ActionResult GeneratedPoints(GeneratedPointsReportFiltersModel filters)
        {
            if (filters != null)
                filters.ReportName = "GeneratedPoints";

            return View(filters);
        }

        public ActionResult GenerateReportGeneratedPoints(GeneratedPointsReportFiltersModel filters)
        {
            var reporteFactory = new ReportFactory();

            var shopName = filters.ShopId.HasValue ? _shopService.GetById(filters.ShopId.Value).Name : PointExResources.LabelAll;
            var educationalInstitutionName = filters.EducationalInstitutionId.HasValue ? _educationalInstitutionService.GetById(filters.EducationalInstitutionId.Value).Name : PointExResources.LabelAll;
            var beneficiaryName = filters.BeneficiaryId.HasValue ? _beneficiaryService.GetById(filters.BeneficiaryId.Value).Name : PointExResources.LabelAll;

            reporteFactory
                .SetParameter("From", filters.From.ToShortDateString(null))
                .SetParameter("To", filters.To.ToShortDateString(null))
                .SetParameter("EducationalInstitutionName", educationalInstitutionName)
                .SetParameter("BeneficiaryName", beneficiaryName)
                .SetParameter("ShopName", shopName)
                .SetParameter("ShowEducationalInstitution", AppSettings.ShowEducationalInstitution.ToString());

            var points = _reportService.GeneratedPoints(filters.From.AbsoluteStart(), filters.To.AbsoluteEnd(),
                filters.ShopId, filters.BeneficiaryId, filters.EducationalInstitutionId);

            reporteFactory.SetDataSource("GeneratedPointsDataSet", points)
                          .SetFullPath(Server.MapPath("~/Reports/GeneratedPoints.rdl"));

            byte[] reportFile = reporteFactory.Render(filters.ReportType);

            return File(reportFile, reporteFactory.MimeType);
        }

        public ActionResult MostExchangedPrizes(ReportFiltersModel filters)
        {
            if (filters != null)
                filters.ReportName = "MostExchangedPrizes";

            return View(filters);
        }

        public ActionResult GenerateReportMostExchangedPrizes(ReportFiltersModel filters)
        {
            var reporteFactory = new ReportFactory();

            var educationalInstitutionName = filters.EducationalInstitutionId.HasValue
                ? _educationalInstitutionService.GetById(filters.EducationalInstitutionId.Value).Name
                : PointExResources.LabelAll;

            reporteFactory
                .SetParameter("From", filters.From.ToShortDateString(null))
                .SetParameter("To", filters.To.ToShortDateString(null))
                .SetParameter("EducationalInstitutionName", educationalInstitutionName)
                .SetParameter("ShowEducationalInstitution", AppSettings.ShowEducationalInstitution.ToString());

            var prizesReport = _reportService.MostExchangedPrizes(filters.From.AbsoluteStart(), filters.To.AbsoluteEnd(),
                filters.EducationalInstitutionId);

            reporteFactory.SetDataSource("MostExchangedPrizesDataSet", prizesReport)
                          .SetFullPath(Server.MapPath("~/Reports/MostExchangedPrizes.rdl"));

            byte[] reportFile = reporteFactory.Render(filters.ReportType);

            return File(reportFile, reporteFactory.MimeType);
        }

        public ActionResult MostUsedBenefits(ReportFiltersModel filters)
        {
            if (filters != null)
                filters.ReportName = "MostUsedBenefits";

            return View(filters);
        }

        public ActionResult GenerateReportMostUsedBenefits(ReportFiltersModel filters)
        {
            var reporteFactory = new ReportFactory();

            var shopName = filters.ShopId.HasValue ? _shopService.GetById(filters.ShopId.Value).Name : PointExResources.LabelAll;

            var educationalInstitutionName = filters.EducationalInstitutionId.HasValue
                ? _educationalInstitutionService.GetById(filters.EducationalInstitutionId.Value).Name
                : PointExResources.LabelAll;

            reporteFactory
                .SetParameter("From", filters.From.ToShortDateString(null))
                .SetParameter("To", filters.To.ToShortDateString(null))
                .SetParameter("EducationalInstitutionName", educationalInstitutionName)
                .SetParameter("ShopName", shopName)
                .SetParameter("ShowEducationalInstitution", AppSettings.ShowEducationalInstitution.ToString());

            var mostUsedBenefits = _reportService.MostUsedBenefits(filters.From.AbsoluteStart(), filters.To.AbsoluteEnd(),
                filters.ShopId,
                filters.EducationalInstitutionId);

            reporteFactory.SetDataSource("MostUsedBenefitsDataSet", mostUsedBenefits)
                          .SetFullPath(Server.MapPath("~/Reports/MostUsedBenefits.rdl"));

            byte[] reportFile = reporteFactory.Render(filters.ReportType);

            return File(reportFile, reporteFactory.MimeType);
        }

        public ActionResult Beneficiaries(ReportBeneficiariesFiltersModel filters)
        {
            if (filters != null)
                filters.ReportName = "Beneficiaries";

            return View(filters);
        }

        public ActionResult GenerateReportBeneficiaries(ReportBeneficiariesFiltersModel filters)
        {
            var reporteFactory = new ReportFactory();

            var educationalInstitutionName = filters.EducationalInstitutionId.HasValue
                ? _educationalInstitutionService.GetById(filters.EducationalInstitutionId.Value).Name
                : PointExResources.LabelAll;

            var townName = filters.TownId.HasValue
                ? _townService.GetById(filters.TownId.Value).Name
                : PointExResources.LabelAll;

            var sexDescription = filters.Sex.HasValue
                ? filters.Sex.Value == 1 ? "MASCULINO" : "FEMENINO" 
                : PointExResources.LabelAll;

            reporteFactory
                .SetParameter("From", filters.From.ToShortDateString(null))
                .SetParameter("To", filters.To.ToShortDateString(null))
                .SetParameter("EducationalInstitutionName", educationalInstitutionName)
                .SetParameter("TownName", townName)
                .SetParameter("SexDescription", sexDescription)
                .SetParameter("ShowEducationalInstitution", AppSettings.ShowEducationalInstitution.ToString());

            var beneficiaries = _reportService.Beneficiaries(filters.From.AbsoluteStart(), filters.To.AbsoluteEnd(),
                filters.Sex,
                filters.TownId,
                filters.EducationalInstitutionId);

            reporteFactory.SetDataSource("BeneficiariesDataSet", beneficiaries)
                          .SetFullPath(Server.MapPath("~/Reports/Beneficiaries.rdl"));

            byte[] reportFile = reporteFactory.Render(filters.ReportType);

            return File(reportFile, reporteFactory.MimeType);
        }
    }
}
