using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Framework.Common.Web.Extensions
{
    public static class UrlHelperExtension
    {
        public static string ContentFullPath(this UrlHelper url, string virtualPath)
        {
            var result = string.Empty;
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;

            result = string.Format("{0}://{1}{2}",
                                   requestUrl.Scheme,
                                   requestUrl.Authority,
                                   VirtualPathUtility.ToAbsolute(virtualPath));
            return result;
        }

        public static string BaseFullUrl(this UrlHelper url)
        {
            var result = string.Empty;
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;

            result = string.Format("{0}://{1}",
                                   requestUrl.Scheme,
                                   requestUrl.Authority);
            return result;
        }
    }
}
