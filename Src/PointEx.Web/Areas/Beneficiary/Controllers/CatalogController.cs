using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Data;
using PointEx.Security;
using PointEx.Service;

namespace PointEx.Web.Areas.Beneficiary.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }


        public ActionResult Search()
        {
            var model = PointExContext.Beneficiary;
            return View(model);
        }
    }
}