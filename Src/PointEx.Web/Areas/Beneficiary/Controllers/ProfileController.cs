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
    public class ProfileController : BeneficiaryBaseController
    {
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly ICurrentUser _currentUser;

        public ProfileController(IBeneficiaryService beneficiaryService, ICurrentUser currentUser)
        {
            _beneficiaryService = beneficiaryService;
            _currentUser = currentUser;
        }


        public ActionResult Index()
        {
            var model = _currentUser.Beneficiary;
            return View(model);
        }

        public ActionResult History()
        {
            var transactions = _beneficiaryService.GetTransactions(_currentUser.Beneficiary.Id);
            return View(transactions);
        }
    }
}