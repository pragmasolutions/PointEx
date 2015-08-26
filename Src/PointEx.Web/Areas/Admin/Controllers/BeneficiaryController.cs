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

            var beneficiarys = _beneficiaryService.GetAll("CreatedDate", "DESC", filters.Criteria, filters.TownId,
                filters.EducationalInstitutionId, false, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<BeneficiaryDto>(beneficiarys, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new BeneficiaryListModel(pagedList, filters);

            return View(listModel);
        }

        public ActionResult Detail(int id)
        {
            var beneficiary = _beneficiaryService.GetById(id);

            var beneficiaryForm = BeneficiaryForm.Create(beneficiary, new ApplicationUser());

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
            
            //we hardcode the institutionId so we don't get validation errors at client side
            if (AppSettings.Theme == ThemeEnum.TekovePoti)
            {
                beneficiaryForm.EducationalInstitutionId = 1;
            }
            return View(beneficiaryForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BeneficiaryForm beneficiaryForm)
        {
            if (!ModelState.IsValid)
            {
                return View(beneficiaryForm);
            }

            if (AppSettings.Theme == ThemeEnum.TekovePoti)
            {
                beneficiaryForm.EducationalInstitutionId = null;
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

            if (AppSettings.Theme == ThemeEnum.TekovePoti)
            {
                beneficiaryForm.EducationalInstitutionId = 1;
            }
            return View(beneficiaryForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BeneficiaryForm beneficiaryForm)
        {
            if (!ModelState.IsValid)
            {
                return View(beneficiaryForm);
            }
            if (AppSettings.Theme == ThemeEnum.TekovePoti)
            {
                beneficiaryForm.EducationalInstitutionId = null;
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

    }
}
