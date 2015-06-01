using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Entities;
using PointEx.Entities.Dto;

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

        public static string GetDefaultImageUrl(this BenefitDto benefit, int width = 50, int height = 50)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlhelper.Action("Image", "File", new { area = "", id = benefit.DefaultFileId, width = width, height = height });
            return url;
        }

        public static string GetDefaultImageUrl(this Benefit benefit, int width = 50, int height = 50)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlhelper.Action("Image", "File", new { area = "", id = benefit.DefaultFileId, width = width, height = height });
            return url;
        }

        public static string GetImageUrl(this Prize benefit, int width = 50, int height = 50)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlhelper.Action("Image", "File", new { area = "", id = benefit.ImageFileId, width = width, height = height });
            return url;
        }

        public static string GetImageUrl(this SectionItem sectionItem, int width = 50, int height = 50)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlhelper.Action("Image", "File", new { area = "", id = sectionItem.DefaultFileId, width = width, height = height });
            return url;
        }

    }
}