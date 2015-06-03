using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Web.Infrastructure.Extensions
{
    public static class BenefitExtensions
    {
        public static string GetDetailUrl(this Benefit benefit)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var detailurl = urlhelper.Action("Detail", "Benefit", routeValues: new { area = "", benefit.Id });
            return detailurl;
        }

        public static string GetDetailUrl(this BenefitDto benefit)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var detailurl = urlhelper.Action("Detail", "Benefit", routeValues: new { area = "", benefit.Id });
            return detailurl;
        }
    }
}