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
        public const string NoImagePath = "~/Content/images/noimage.png?width={0}&height={1}";

        public static string GetUrl(this File file, int width = 50, int height = 50)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlhelper.Action("Image", "File", new { area = "", id = file.Id, width = width, height = height });
            return url;
        }

        public static string GetDefaultImageUrl(this BenefitDto benefit, int width = 50, int height = 50)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = benefit.DefaultFileId.HasValue
                ? urlhelper.Action("Image", "File",
                    new { area = "", id = benefit.DefaultFileId, width = width, height = height })
                : urlhelper.GetNoImageUrl(width, height);
            return url;
        }

        public static string GetDefaultImageUrl(this Benefit benefit, int width = 50, int height = 50)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = benefit.DefaultFileId.HasValue
                ? urlhelper.Action("Image", "File",
                    new { area = "", id = benefit.DefaultFileId, width = width, height = height })
                : urlhelper.GetNoImageUrl(width, height);
            return url;
        }

        public static string GetImageUrl(this SectionItem sectionItem, int width = 50, int height = 50)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = sectionItem.DefaultFileId.HasValue
                ? urlhelper.Action("Image", "File",
                    new { area = "", id = sectionItem.DefaultFileId, width = width, height = height })
                : urlhelper.GetNoImageUrl(width, height);
            return url;
        }

        public static string GetNoImageUrl(this UrlHelper urlHelper, int width = 50, int height = 50)
        {
            var url = urlHelper.Content(string.Format(NoImagePath, width, height));
            return url;
        }
    }
}