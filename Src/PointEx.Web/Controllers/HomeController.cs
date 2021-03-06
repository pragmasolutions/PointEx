﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PointEx.Data;
using PointEx.Entities.Models;
using Framework.Common.Web.Alerts;
using PointEx.Security;
using PointEx.Service;
using PointEx.Web.Models;
using System.Threading.Tasks;
using PointEx.Entities.Enums;
using PointEx.Web.Configuration;
using PointEx.Web.Infrastructure;

namespace PointEx.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISectionItemService _sectionItemService;
        private readonly IBenefitService _benefitService;
        private readonly ICategoryService _categoryService;
        private readonly INotificationService _notificationService;
        private readonly ITownService _townService;
        private readonly IEducationalInstitutionService _educationalInstitutionService;
        private readonly ICurrentUser _currentUser;
        private readonly IShopService _shopService;

        public HomeController(ISectionItemService sectionItemService, IBenefitService benefitService, 
            ICategoryService categoryService, 
            INotificationService notificationService, 
            ITownService townService,
            IEducationalInstitutionService educationalInstitutionService,
            ICurrentUser currentUser,
            IShopService shopService)
        {
            _sectionItemService = sectionItemService;
            _benefitService = benefitService;
            _categoryService = categoryService;
            _notificationService = notificationService;
            _townService = townService;
            _educationalInstitutionService = educationalInstitutionService;
            _currentUser = currentUser;
            _shopService = shopService;
        }

        public ActionResult Index()
        {
            var sliderItems = _sectionItemService.GetBySectionName("SliderHome");
            var outstandingItems = _benefitService.GetOutstandingBenefits();
            var categories = _categoryService.GetAll().ToList();

            var homeModel = new HomeModel(sliderItems, outstandingItems, categories);            
            return View(homeModel);
        }

        public ActionResult AddMyShop()
        {
            AddMyShopForm form = new AddMyShopForm();
            return View(form);
        }

        public ActionResult AddBeneficiary()
        {
            BeneficiaryForm form = new BeneficiaryForm();
            form.Theme = AppSettings.Theme;
            return View(form);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddMyShop(AddMyShopForm addMyShopForm)
        {
            if (!ModelState.IsValid)
            {
                return View(addMyShopForm);
            }

            var shop = addMyShopForm.ToShop();

            shop.Town = _townService.GetById(addMyShopForm.TownId);
            shop.StatusId = StatusEnum.Pending;

            try
            {
                await _shopService.Create(shop, addMyShopForm.Email, AppSettings.Theme);
                await _notificationService.SendPendingShopEmail(shop.Name, addMyShopForm.Email, true, AppSettings.Theme);
            }
            catch (ApplicationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(addMyShopForm);
            }

            return RedirectToAction("Index").WithSuccess("Su solicitud ha sido enviada correctamente");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddBeneficiary(BeneficiaryForm addBeneficiaryForm)
        {
            if (!ModelState.IsValid)
            {
                return View(addBeneficiaryForm);
            }

            var beneficiary = addBeneficiaryForm.ToBeneficiary();

            beneficiary.Town = _townService.GetById(addBeneficiaryForm.TownId);

            if (addBeneficiaryForm.EducationalInstitutionId.HasValue)
            {
                beneficiary.EducationalInstitution = _educationalInstitutionService.GetById(addBeneficiaryForm.EducationalInstitutionId.Value);
            }

            await _notificationService.SendAddBeneficiaryRequestEmail(beneficiary, addBeneficiaryForm.Email, AppSettings.Theme);

            return RedirectToAction("Index").WithSuccess("Su solicitud ha sido enviada correctamente");
        }

        public ActionResult FrequentlyAskedQuestions()
        {
            if (AppSettings.Theme == ThemeEnum.TarjetaVerde)
            {
                return View();    
            }
            else
            {
                return this.View("FrequentlyAskedQuestionsTekove");
            }
        }

        public ActionResult TermsAndConditions()
        {
            if (AppSettings.Theme == ThemeEnum.TarjetaVerde)
            {
                return View();
            }
            else
            {
                return this.View("TermsAndConditionsTekove");
            }
        }

        public ActionResult About()
        {
            if (AppSettings.Theme == ThemeEnum.TarjetaVerde)
            {
                return View();
            }
            else
            {
                return this.View("AboutTekove");
            }
        }

       public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RedirectToDefault()
        {
            switch (PointExContext.Role)
            {
                case RolesNames.SuperAdmin:
                case RolesNames.Admin:
                    return Redirect("~/Admin");
                case RolesNames.Shop:
                    return Redirect("~/Shop");
                case RolesNames.Beneficiary:
                    return Redirect("~/Beneficiary/Profile/Index");
                case RolesNames.BeneficiaryAdmin:
                    return Redirect("~/Admin/Beneficiary");
                case RolesNames.ShopAdmin:
                    return Redirect("~/Admin/Shop");
                default:
                    return RedirectToAction("Index");
            }
        }

        public ActionResult InformationRequest()
        {
            InformationRequestModel model = new InformationRequestModel();
            if (PointExContext.User != null)
            {
                switch (PointExContext.Role)
                {
                    case RolesNames.Beneficiary:
                        var beneficiary = _currentUser.Beneficiary;
                        model.Name = beneficiary.Name;
                        break;
                    case RolesNames.Shop:
                        var shop = PointExContext.Shop;
                        model.Name = string.Format("[Shop] {0}", shop.Name);
                        model.PhoneNumber = shop.Phone;
                        break;
                }
                model.Email = PointExContext.User.Email;
            }
            
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> InformationRequest(InformationRequestModel model)
        {
            try
            {
                await _notificationService.SendInformationRequestEmail(model, AppSettings.Theme);
                return View("SuccessfullRequest");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Hubo un error en la solicitud. Por favor intente nuevamente más tarde";
                return View(model);
            }
        }
    }
}