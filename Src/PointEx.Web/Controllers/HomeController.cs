using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Security;

namespace PointEx.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (PointExContext.Role == RolesNames.Beneficiary)
            {
                var model = PointExContext.Beneficiary;
                return View("~/Areas/Beneficiary/Views/Profile/Index.cshtml", model);
            }
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
    }
}