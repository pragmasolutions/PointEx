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

        protected ImageResult Image(byte[] fileBytes, string contentType, int width = 0, int height = 0)
        {
            return new ImageResult(fileBytes, contentType, width, height);
        }
    }
}
