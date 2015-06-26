using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Framework.Common.Web.Alerts;
using PointEx.Security;
using PointEx.Service;
using PointEx.Web.Models;

namespace PointEx.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISectionItemService _sectionItemService;
        private readonly IBenefitService _benefitService;
        private readonly ICategoryService _categoryService;
        private readonly INotificationService _notificationService;
        private readonly ITownService _townService;

        public HomeController(ISectionItemService sectionItemService, IBenefitService benefitService, 
            ICategoryService categoryService, 
            INotificationService notificationService, 
            ITownService townService)
        {
            _sectionItemService = sectionItemService;
            _benefitService = benefitService;
            _categoryService = categoryService;
            _notificationService = notificationService;
            _townService = townService;
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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddMyShop(AddMyShopForm addMyShopForm)
        {
            if (!ModelState.IsValid)
            {
                return View(addMyShopForm);
            }

            var shop = addMyShopForm.ToShop();

            shop.Town = _townService.GetById(addMyShopForm.TownId);

            await _notificationService.SendAddShopRequestEmail(shop, addMyShopForm.Email);

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
    }
}