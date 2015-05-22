using System;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Framework.Common.Web.Helpers
{
    public static partial class Helper
    {
        public static MvcHtmlString BackButton(this HtmlHelper helper, string action, string controller, string area = null, string buttonText = "Volver al Listado")
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlhelper.Action(action, controller, new { area = area });
            var html = String.Format("<a href=\"{0}\" class=\"btn btn-primary btn-back-to-list\"><span class=\"glyphicon glyphicon-arrow-left\"></span> {1}</a>", url, buttonText);
            return MvcHtmlString.Create(html);
        }
    }
}