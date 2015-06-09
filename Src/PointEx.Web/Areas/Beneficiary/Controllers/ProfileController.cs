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
    public class ProfileController : BeneficiaryBaseController
    {
        private readonly IBeneficiaryService _beneficiaryService;

        public ProfileController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }


        public ActionResult Index()
        {
            var model = PointExContext.Beneficiary;
            return View(model);
        }

        public ActionResult History()
        {
            var transactions = _beneficiaryService.GetTransactions(PointExContext.Beneficiary.Id);
            return View(transactions);
        }
    }
}