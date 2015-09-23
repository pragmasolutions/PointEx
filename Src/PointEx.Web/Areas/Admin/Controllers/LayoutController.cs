using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Service;
using PointEx.Web.Configuration;
using PointEx.Web.Infrastructure;

namespace PointEx.Web.Areas.Admin.Controllers
{
    public class LayoutController : Controller
    {
        private ICurrentUser _currentUser;
        private ILayoutService _layoutService;

        public LayoutController(ICurrentUser currentUser, ILayoutService layoutService)
        {
            _currentUser = currentUser;
            _layoutService = layoutService;
        }

        public ActionResult Menu()
        {
            var role = _currentUser.PointexUser.Roles.First().Name;
            var items = _layoutService.GetAdminMenuItems(role);
            if (AppSettings.Theme != ThemeEnum.TarjetaVerde)
            {
                items = items.Where(i => i.Text != "Est. Educativos").ToList();
            }
            return PartialView(items);
        }
    }

    
}