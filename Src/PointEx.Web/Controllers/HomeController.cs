using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Security;
using PointEx.Service;
using PointEx.Web.Models;

namespace PointEx.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISectionItemService _sectionItemService;
        private readonly IBenefitService _benefitService;

        public HomeController(ISectionItemService sectionItemService,IBenefitService benefitService)
        {
            _sectionItemService = sectionItemService;
            _benefitService = benefitService;
        }

        public ActionResult Index()
        {
            var sliderItems = _sectionItemService.GetBySectionName("SliderHome");
            var outstandingItems = _benefitService.GetOutstandingBenefits();

            var homeModel = new HomeModel(sliderItems, outstandingItems);

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
    }
}