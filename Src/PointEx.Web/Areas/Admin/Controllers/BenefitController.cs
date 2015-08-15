using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Service;
using PointEx.Web.Controllers;
using PointEx.Web.Models;
using PointEx.Web.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using PointEx.Entities;
using PointEx.Entities.Enums;
using PointEx.Web.Areas.Admin.Controllers;
using PointEx.Web.Configuration;

namespace PointEx.Web.Areas.Admin.Controllers
{
    public class BenefitController : AdminBaseController
    {
        private readonly IBenefitService _benefitService;        
        private readonly ICurrentUser _currentUser;
        private readonly IBranchOfficeService _branchOfficeService;
        private readonly INotificationService _notificationService;

        public BenefitController(IBenefitService benefitService, ICurrentUser currentUser, IBranchOfficeService branchOffice,
                                INotificationService notificationService)
        {
            _benefitService = benefitService;
            _notificationService = notificationService;
            _currentUser = currentUser;
            _branchOfficeService = branchOffice;
        }

        public ActionResult Index(BenefitListFiltersModel filters)
        {
            int pageTotal;

            var benefits = _benefitService.GetBenefitByStatus("CreatedDate", "ASC", filters.CategoryId, filters.TownId, filters.ShopId, BenefitStatusEnum.Pending, filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<BenefitDto>(benefits, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new BenefitListModel(pagedList, filters);

            ViewBag.ViewMode = BenefitStatusEnum.Pending;
            ViewBag.TabTitle = "Beneficios Pendientes";
            ViewBag.Title = "Beneficios Pendientes de Aprobación";

            return View(listModel);
        }

        public ActionResult ApprovedBenefit(BenefitListFiltersModel filters)
        {
            int pageTotal;

            var benefits = _benefitService.GetBenefitByStatus("CreatedDate", "ASC", filters.CategoryId, filters.TownId, filters.ShopId, BenefitStatusEnum.Approved, filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<BenefitDto>(benefits, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new BenefitListModel(pagedList, filters);

            ViewBag.ViewMode = BenefitStatusEnum.Approved;
            ViewBag.TabTitle = "Beneficios Aprobados";
            ViewBag.Title = "Beneficios Aprobados";

            return View("Index", listModel);
        }

        public ActionResult RejectedBenefit(BenefitListFiltersModel filters)
        {
            int pageTotal;

            var benefits = _benefitService.GetBenefitByStatus("CreatedDate", "ASC", filters.CategoryId, filters.TownId, filters.ShopId, BenefitStatusEnum.Rejected, filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<BenefitDto>(benefits, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new BenefitListModel(pagedList, filters);

            ViewBag.ViewMode = BenefitStatusEnum.Rejected;
            ViewBag.TabTitle = "Beneficios Rechazados";
            ViewBag.Title = "Beneficios Rechazados";

            return View("Index", listModel);
        }

        public ActionResult Detail(int id)
        {
            var benefit = _benefitService.GetById(id);
            var benefitForm = BenefitForm.FromBenefit(benefit);
            ViewBag.IsApproved = benefit.BenefitStatusId == BenefitStatusEnum.Approved;
            return View(benefitForm);
        }        

        public ActionResult Edit(int id)
        {
            var benefit = _benefitService.GetById(id);
            var benefitForm = BenefitForm.FromBenefit(benefit);
            return View(benefitForm);
        }
               
        [HttpPost]
        public async Task<ActionResult> Approved(int id)
        {
            _benefitService.Moderated(id, (int)BenefitStatusEnum.Approved);

            var benefit = _benefitService.GetById(id);
            await _notificationService.SendBenefitApprovedMail(benefit, AppSettings.SiteBaseUrl);

            return RedirectToAction("RejectedBenefit", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Beneficio Aprobado");
        }

        [HttpPost]
        public ActionResult Rejected(int id)
        {
            _benefitService.Moderated(id, (int)BenefitStatusEnum.Rejected);

            return RedirectToAction("ApprovedBenefit", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Beneficio Rechazado");
        }       
    }
}
