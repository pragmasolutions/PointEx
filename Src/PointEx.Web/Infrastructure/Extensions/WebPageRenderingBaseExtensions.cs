using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using PointEx.Entities;

namespace PointEx.Web.Infrastructure.Extensions
{
    public static class WebPageRenderingBaseExtensions
    {
        public static ICurrentUser CurrentUser(this WebPageRenderingBase webPageRendering)
        {
            var currentUser = IocContainer.GetContainer().Get<PointEx.Web.Infrastructure.ICurrentUser>();
            return currentUser;
        }
    }
}