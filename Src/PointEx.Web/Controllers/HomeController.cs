using System;
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

        public HomeController(ISectionItemService sectionItemService, IBenefitService benefitService, 
            ICategoryService categoryService, 
            INotificationService notificationService, 
            ITownService townService,
            IEducationalInstitutionService educationalInstitutionService,
            ICurrentUser currentUser)
        {
            _sectionItemService = sectionItemService;
            _benefitService = benefitService;
            _categoryService = categoryService;
            _notificationService = notificationService;
            _townService = townService;
            _educationalInstitutionService = educationalInstitutionService;
            _currentUser = currentUser;
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
            AddBeneficiaryForm form = new AddBeneficiaryForm();
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

            await _notificationService.SendAddShopRequestEmail(shop, addMyShopForm.Email, AppSettings.Theme);

            return RedirectToAction("Index").WithSuccess("Su solicitud ha sido enviada correctamente");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddBeneficiary(AddBeneficiaryForm addBeneficiaryForm)
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
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
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
                    return Redirect("~/Beneficiary");
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