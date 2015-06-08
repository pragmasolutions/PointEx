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

namespace PointEx.Web.Areas.Shop.Controllers
{
    public class BenefitController : ShopBaseController
    {
        private readonly IBenefitService _benefitService;
        private readonly IShopService _shopService;
        private readonly ICurrentUser _currentUser;
        private readonly IBranchOfficeService _branchOfficeService;

        public BenefitController(IBenefitService benefitService, IShopService shopService, ICurrentUser currentUser, IBranchOfficeService branchOffice)
        {
            _benefitService = benefitService;
            _shopService = shopService;
            _currentUser = currentUser;
            _branchOfficeService = branchOffice;
        }

        public ActionResult Index(BenefitListFiltersModel filters)
        {
            int pageTotal;

            var benefits = _benefitService.GetAll("CreatedDate", "DESC", filters.CategoriaId, filters.TownId, _currentUser.Shop.Id, filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

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

        public ActionResult Create()
        {
            var benefitForm = new BenefitForm();
            benefitForm.BranchOfficesSelected = _branchOfficeService.GetByShopId(_currentUser.Shop.Id).Select(bo => bo.Id);
            return View(benefitForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(BenefitForm benefitForm)
        {
            if (!ModelState.IsValid)
            {
                return View(benefitForm);
            }

            var benefit = benefitForm.ToBenefit();

            var currentShop = _shopService.GetByUserId(this.User.Identity.GetUserId());

            benefit.ShopId = currentShop.Id;

            _benefitService.Create(benefit);

            return RedirectToAction("Index", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Premio Creado");
        }

        public ActionResult Edit(int id)
        {
            var benefit = _benefitService.GetById(id);
            var benefitForm = BenefitForm.FromBenefit(benefit);
            return View(benefitForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BenefitForm benefitForm)
        {
            if (!ModelState.IsValid)
            {
                return View(benefitForm);
            }

            _benefitService.Edit(benefitForm.ToBenefit());

            return RedirectToAction("Index", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Premio Editado");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            _benefitService.Delete(id);

            return RedirectToAction("Index", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Premio Eliminado");
        }

        public ActionResult IsNameAvailable(string name, int id)
        {
            return Json(_benefitService.IsNameAvailable(name, id), JsonRequestBehavior.AllowGet);
        }
    }
}
