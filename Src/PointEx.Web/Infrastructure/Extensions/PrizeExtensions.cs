using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Web.Infrastructure.Extensions
{
    public static class PrizeExtensions
    {
        public static string GetDetailUrl(this Prize prize)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var detailurl = urlhelper.Action("Detail", "Prize", routeValues: new { area = "", prize.Id });
            return detailurl;
        }

        public static string GetDetailUrl(this PrizeDto prize)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var detailurl = urlhelper.Action("Detail", "Prize", routeValues: new { area = "", prize.Id });
            return detailurl;
        }

        public static string GetImageUrl(this Prize benefit, int width = 50, int height = 50)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = benefit.ImageFileId.HasValue
                ? urlhelper.Action("Image", "File",
                    new { area = "", id = benefit.ImageFileId, width = width, height = height })
                : urlhelper.GetNoImageUrl(width, height);
            return url;
        }

        public static string GetImageUrl(this PrizeDto prize, int width = 50, int height = 50)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = prize.ImageFileId.HasValue
                ? urlhelper.Action("Image", "File",
                    new { area = "", id = prize.ImageFileId, width = width, height = height })
                : urlhelper.GetNoImageUrl(width, height);
            return url;
        }
    }
}