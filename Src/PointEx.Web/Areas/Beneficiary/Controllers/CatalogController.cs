using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Data;
using PointEx.Security;
using PointEx.Service;
using PointEx.Web.Infrastructure;

namespace PointEx.Web.Areas.Beneficiary.Controllers
{
    public class CatalogController : BeneficiaryBaseController
    {
        private readonly ICatalogService _catalogService;
        private readonly ICurrentUser _currentUser;

        public CatalogController(ICatalogService catalogService, ICurrentUser currentUser)
        {
            _catalogService = catalogService;
            _currentUser = currentUser;
        }


        public ActionResult Search()
        {
            var model = _currentUser.Beneficiary;
            return View(model);
        }
    }
}