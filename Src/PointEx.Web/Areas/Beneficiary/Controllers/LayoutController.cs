using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Web.Infrastructure;

namespace PointEx.Web.Areas.Beneficiary.Controllers
{
    public class LayoutController : Controller
    {
        private readonly ICurrentUser _currentUser;

        public LayoutController(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public ActionResult MenuLateral()
        {
            var picture = _currentUser.Beneficiary.Sex == 1 ? "man" : "woman";
            ViewBag.PictureUrl = String.Format("/Content/images/profile-{0}.jpg", picture);
            return PartialView("_MenuLateral");
        }
    }
}