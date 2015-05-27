using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Entities;

namespace PointEx.Web.Infrastructure.Extensions
{
    public static class FileExtensions
    {
        public static string GetUrl(this File file, int width = 50, int height = 50)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlhelper.Action("Image", "File", new { area = "", id = file.Id, width = width, height = height });
            return url;
        }
    }
}