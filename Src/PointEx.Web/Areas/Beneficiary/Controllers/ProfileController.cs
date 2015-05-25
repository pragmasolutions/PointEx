using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Data;
using PointEx.Security;

namespace PointEx.Web.Areas.Beneficiary.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult Index()
        {
            var model = PointExContext.Beneficiary;
            return View(model);
        }
    }
}