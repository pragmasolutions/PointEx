using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Web.Mvc;

namespace PointEx.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public const int DefaultPageSize = 50;
        protected ActionResult RedirectToAction<TController>(Expression<Action<TController>> action)
            where TController : Controller
        {
            return ControllerExtensions.RedirectToAction(this, action);
        }
    }
}
