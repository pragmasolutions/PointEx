using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Security.Managers;
using PointEx.Security.Model;
using PointEx.Service;
using PointEx.Web.Configuration;
using PointEx.Web.Controllers;
using PointEx.Web.Models;
using PointEx.Entities.Enums;
using PointEx.Security;
using Framework.Report;

namespace PointEx.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator,SuperAdmin,BeneficiaryAdmin")]
    public class BeneficiaryController : BaseController
    {
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly ApplicationUserManager _userManager;
        private readonly ICardService _cardService;

        public BeneficiaryController(IBeneficiaryService beneficiaryService, ApplicationUserManager userManager, ICardService cardService)
        {
            _beneficiaryService = beneficiaryService;
            _userManager = userManager;
            _cardService = cardService;
        }

        public ActionResult Index(BeneficiaryListFiltersModel filters)
        {

            int pageTotal;

            var beneficiaries = _beneficiaryService.GetBeneficiaryByStatus("CreatedDate", "ASC", filters.TownId, StatusEnum.Pending, filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<BeneficiaryDto>(beneficiaries, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new BeneficiaryListModel(pagedList, filters);            

            ViewBag.ViewMode = StatusEnum.Pending;
            ViewBag.TabTitle = "Beneficiarios Pendientes";
            ViewBag.Title = "Beneficiarios Pendientes de Aprobación";            

            return View(listModel);
        }

        public ActionResult Detail(int id)
        {
            var beneficiary = _beneficiaryService.GetById(id);

            var beneficiaryForm = BeneficiaryForm.Create(beneficiary, new ApplicationUser());
            ViewBag.IsApproved = beneficiary.StatusId == StatusEnum.Approved;

            ViewBag.ShowApprovalButtons = User.IsInRole(RolesNames.Admin) && beneficiary.StatusId == StatusEnum.Pending;

            return View(beneficiaryForm);
        }
               
        public ActionResult Cards(int id)
        {
            var beneficiary = _beneficiaryService.GetById(id);
            var cards = _cardService.GetByBeneficiaryId(id);
            var beneficiaryCards = new BeneficiaryCardsModel(beneficiary, cards);

            return View(beneficiaryCards);
        }

        public ActionResult CreateCard(int beneficiaryId)
        {
            var createCardForm = new CreateCardForm();
            var beneficiary = _beneficiaryService.GetById(beneficiaryId);
            createCardForm.BeneficiaryId = beneficiaryId;
            createCardForm.BeneficiaryName = beneficiary.Name;
            createCardForm.BirthDate = beneficiary.BirthDate.GetValueOrDefault();
            createCardForm.ExpirationDate = _cardService.CalculateExpirationDate(beneficiary.BirthDate.GetValueOrDefault());

            return View(createCardForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CreateCard(CreateCardForm createCardForm)
        {
            if (!ModelState.IsValid)
            {
                return View(createCardForm);
            }

            var card = createCardForm.ToCard();

            _cardService.Create(card);

            return RedirectToAction("Cards", new { id = createCardForm.BeneficiaryId }).WithSuccess("Tarjeta Creada");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GenerateCard(int id)
        {
            _cardService.Generate(id);
            return RedirectToAction("Cards", new { id = id }).WithSuccess("Tarjeta Generada");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CancelCard(int id)
        {
            var card = _cardService.CancelCard(id);

            return RedirectToAction("Cards", new { id = card.BeneficiaryId }).WithSuccess("Tarjeta Cancelada");
        }

        public ActionResult Create()
        {
            var beneficiaryForm = new BeneficiaryForm();
            beneficiaryForm.Theme = AppSettings.Theme;
            
            return View(beneficiaryForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BeneficiaryForm beneficiaryForm)
        {
            if (!ModelState.IsValid)
            {
                return View(beneficiaryForm);
            }

            var beneficiary = beneficiaryForm.ToBeneficiary();

            var user = new ApplicationUser { UserName = beneficiaryForm.Email, Email = beneficiaryForm.Email };

            try
            {
                await _beneficiaryService.Create(beneficiary, user, AppSettings.Theme);
            }
            catch (ApplicationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(beneficiaryForm);
            }

            return RedirectToAction("Index", new BeneficiaryListFiltersModel().GetRouteValues()).WithSuccess("Beneficiario Creado");
        }

        public ActionResult Edit(int id)
        {
            var beneficiary = _beneficiaryService.GetById(id);
            var user = _userManager.FindById(beneficiary.UserId);
            var beneficiaryForm = BeneficiaryForm.Create(beneficiary, user);
            beneficiaryForm.Theme = AppSettings.Theme;
            
            return View(beneficiaryForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BeneficiaryForm beneficiaryForm)
        {
            if (!ModelState.IsValid)
            {
                return View(beneficiaryForm);
            }
           
            _beneficiaryService.Edit(beneficiaryForm.ToBeneficiary());

            return RedirectToAction("Index", new BeneficiaryListFiltersModel().GetRouteValues()).WithSuccess("Beneficiario Editado");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            _beneficiaryService.Delete(id);

            return RedirectToAction("Index", new BeneficiaryListFiltersModel().GetRouteValues()).WithSuccess("Beneficiario Eliminado");
        }

        public ActionResult ApprovedBeneficiary(BeneficiaryListFiltersModel filters)
        {
            int pageTotal;

            var beneficiaries = _beneficiaryService.GetBeneficiaryByStatus("CreatedDate", "ASC", filters.TownId, StatusEnum.Approved, filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<BeneficiaryDto>(beneficiaries, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new BeneficiaryListModel(pagedList, filters);

            ViewBag.ViewMode = StatusEnum.Approved;
            ViewBag.TabTitle = "Beneficiarios Aprobados";
            ViewBag.Title = "Beneficiarios Aprobados";

            return View("Index", listModel);
        }

        public ActionResult RejectedBeneficiary(BeneficiaryListFiltersModel filters)
        {
            int pageTotal;

            var beneficiaries = _beneficiaryService.GetBeneficiaryByStatus("CreatedDate", "ASC", filters.TownId, StatusEnum.Rejected, filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<BeneficiaryDto>(beneficiaries, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new BeneficiaryListModel(pagedList, filters);

            ViewBag.ViewMode = StatusEnum.Rejected;
            ViewBag.TabTitle = "Beneficios Rechazados";
            ViewBag.Title = "Beneficios Rechazados";

            return View("Index", listModel);
        }

        [HttpPost]
        public async Task<ActionResult> Approved(int id)
        {
            _beneficiaryService.Moderated(id, (int)StatusEnum.Approved);

            var benefit = _beneficiaryService.GetById(id);
            //await _notificationService.SendBenefitApprovedMail(benefit, AppSettings.SiteBaseUrl);

            if (Configuration.AppSettings.SiteBaseUrl.Contains("ApprovedBeneficiary"))
            {
                return RedirectToAction("RejectedBeneficiario", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Beneficiario Aprobado");
            }
            else
            {
                return RedirectToAction("Index", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Beneficiario Aprobado");
            }
        }

        [HttpPost]
        public ActionResult Rejected(int id)
        {
            _beneficiaryService.Moderated(id, (int)StatusEnum.Rejected);
            if (Configuration.AppSettings.SiteBaseUrl.Contains("RejectedBeneficiario"))
            {
                return RedirectToAction("ApprovedBeneficiario", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Beneficiario Rechazado");
            }
            else
            {
                return RedirectToAction("Index", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Beneficiario Aprobado");
            }

        }


        public ActionResult TyC(int id)
        {
            var beneficiary = _beneficiaryService.GetById(id);

            var filters = new TyCFiltersModel();
            filters.BeneficiaryId = beneficiary.Id;
            filters.ReportName = "TyC";

            return View(filters);
        }

        public ActionResult GenerateTyC(TyCFiltersModel filters)
        {
            var reporteFactory = new ReportFactory();

            var beneficiary = _beneficiaryService.GetById(filters.BeneficiaryId);

            reporteFactory
                .SetParameter("Name", beneficiary.Name)
                .SetParameter("Address", beneficiary.Address)
                .SetParameter("Town", beneficiary.Town.Name);
                       

            reporteFactory.SetDataSource("TyCDataSet", null)
                          .SetFullPath(Server.MapPath("~/Reports/TyC.rdl"));

            byte[] reportFile = reporteFactory.Render(filters.ReportType);

            return File(reportFile, reporteFactory.MimeType);
        }
    }
}
