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
            return BackButton(helper, action, controller, new { area = area }, buttonText);
        }

        public static MvcHtmlString BackButton(this HtmlHelper helper, string action, string controller, object routeValue = null, string buttonText = "Volver al Listado")
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlhelper.Action(action, controller, routeValue);
            var html = String.Format("<a href=\"{0}\" class=\"btn btn-primary btn-back-to-list\"><span class=\"glyphicon glyphicon-arrow-left\"></span> {1}</a>", url, buttonText);
            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString BackButton(this HtmlHelper helper, string url, string buttonText = "Volver al Listado")
        {
            var html = String.Format("<a href=\"{0}\" class=\"btn btn-primary btn-back-to-list\"><span class=\"glyphicon glyphicon-arrow-left\"></span> {1}</a>", url, buttonText);
            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString FilterButton(this HtmlHelper helper, string filterContainer = "filter-container")
        {
            var html =
                String.Format(
                    "<button class=\"btn btn-primary\" type=\"button\" data-toggle=\"collapse\" data-target=\"#{0}\" aria-expanded=\"false\" aria-controls=\"filter-container\">"
                    + "<span class=\"glyphicon glyphicon-filter\"></span>"
                    + "</button>", filterContainer);

            return MvcHtmlString.Create(html);
        }
    }
}