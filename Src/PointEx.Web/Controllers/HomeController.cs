using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PointEx.Data;
using PointEx.Entities.Models;
using PointEx.Security;
using PointEx.Service;
using PointEx.Web.Models;
using System.Threading.Tasks;
using PointEx.Web.Configuration;

namespace PointEx.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISectionItemService _sectionItemService;
        private readonly IBenefitService _benefitService;
        private readonly ICategoryService _categoryService;
        private readonly INotificationService _notificationService;

        public HomeController(ISectionItemService sectionItemService,IBenefitService benefitService,
                            ICategoryService categoryService, INotificationService notificationService)
        {
            _sectionItemService = sectionItemService;
            _benefitService = benefitService;
            _categoryService = categoryService;
            _notificationService = notificationService;
        }

        public ActionResult Index()
        {
            var sliderItems = _sectionItemService.GetBySectionName("SliderHome");
            var outstandingItems = _benefitService.GetOutstandingBenefits();
            var categories = _categoryService.GetAll().ToList();

            var homeModel = new HomeModel(sliderItems, outstandingItems, categories);

            return View(homeModel);
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
                        var beneficiary = PointExContext.Beneficiary;
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
