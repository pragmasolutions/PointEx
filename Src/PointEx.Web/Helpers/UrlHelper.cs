using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PointEx.Web.Configuration;

namespace PointEx.Web.Helpers
{
    public static class UrlHelper
    {
        public static bool IsInPublicSection()
        {
            var baseUrl = AppSettings.SiteBaseUrl;
            var areas = new List<string>
            {
                String.Format("{0}Admin", baseUrl),
                String.Format("{0}Shop", baseUrl),
                String.Format("{0}Beneficiary", baseUrl)
            };
            return !areas.Any(a => HttpContext.Current.Request.Url.ToString().IndexOf(a, StringComparison.CurrentCultureIgnoreCase) != -1);
        }
    }
}