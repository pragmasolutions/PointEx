using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Service;
using PointEx.Web.Controllers;
using PointEx.Web.Models;
using PointEx.Web.Infrastructure;
using System.Linq;
using PointEx.Web.Areas.Admin.Controllers;

namespace PointEx.Web.Areas.Admin.Controllers
{
    public class BenefitController : AdminBaseController
    {
        private readonly IBenefitService _benefitService;        
        private readonly ICurrentUser _currentUser;
        private readonly IBranchOfficeService _branchOfficeService;

        public BenefitController(IBenefitService benefitService, ICurrentUser currentUser, IBranchOfficeService branchOffice)
        {
            _benefitService = benefitService;            
            _currentUser = currentUser;
            _branchOfficeService = branchOffice;
        }

        public ActionResult Index(BenefitListFiltersModel filters)
        {
            int pageTotal;

            var benefits = _benefitService.GetPendingBenefit("CreatedDate", "ASC", filters.CategoryId, filters.TownId, filters.ShopId, filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<BenefitDto>(benefits, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new BenefitListModel(pagedList, filters);

            return View(listModel);
        }

        public ActionResult Detail(int id)
        {
            var benefit = _benefitService.GetById(id);
            var benefitForm = BenefitForm.FromBenefit(benefit);
            return View(benefitForm);
        }        

        public ActionResult Edit(int id)
        {
            var benefit = _benefitService.GetById(id);
            var benefitForm = BenefitForm.FromBenefit(benefit);
            return View(benefitForm);
        }
               
        [HttpPost]
        public ActionResult Approved(int id)
        {
            _benefitService.Moderated(id, true);

            return RedirectToAction("Index", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Beneficio Aprobado");
        }

        [HttpPost]
        public void Rejected(int id)
        {
            _benefitService.Moderated(id, false);
        }       
    }
}
