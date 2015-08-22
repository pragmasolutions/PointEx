using System;
using System.Collections.Generic;
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
using System.Threading.Tasks;
using PointEx.Entities;
using PointEx.Entities.Enums;
using PointEx.Entities.Models;
using PointEx.Security;
using PointEx.Web.Configuration;
using PointEx.Web.Infrastructure.Extensions;

namespace PointEx.Web.Areas.Shop.Controllers
{
    [Authorize(Roles = "Administrator, SuperAdmin, Shop")]
    public class BenefitController : BaseController
    {
        private readonly IBenefitService _benefitService;
        private readonly IShopService _shopService;
        private readonly ICurrentUser _currentUser;
        private readonly IBranchOfficeService _branchOfficeService;
        private readonly INotificationService _notificationService;
        private readonly IBenefitFileService _benefitFileService;

        public BenefitController(IBenefitService benefitService, IShopService shopService, ICurrentUser currentUser,
            IBranchOfficeService branchOffice, INotificationService notificationService, IBenefitFileService benefitFileService)
        {
            _benefitService = benefitService;
            _shopService = shopService;
            _currentUser = currentUser;
            _branchOfficeService = branchOffice;
            _notificationService = notificationService;
            _benefitFileService = benefitFileService;
        }

        public ActionResult Index(BenefitListFiltersModel filters)
        {
            int pageTotal;

            var benefits = _benefitService.GetAll("CreatedDate", "DESC", filters.CategoryId, filters.TownId, _currentUser.Shop.Id, filters.Criteria, null, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<BenefitDto>(benefits, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new BenefitListModel(pagedList, filters);


            return View(listModel);
        }


        public ActionResult Detail(int id)
        {
            var benefit = _benefitService.GetById(id);
            var benefitForm = BenefitForm.FromBenefit(benefit);

            ViewBag.BenefitStatus = benefit.BenefitStatus != null 
                ? benefit.BenefitStatus.Name 
                : string.Empty;

            ViewBag.ReturnController = _currentUser.Shop != null 
                ? "Shop" 
                : "Admin";

            ViewBag.ShowApprovalButtons = User.IsInRole(RolesNames.Admin) && benefit.BenefitStatusId == BenefitStatusEnum.Pending;

            return View(benefitForm);
        }

        public ActionResult Create()
        {
            var benefitForm = new CreateBenefitForm();
            benefitForm.BranchOfficesSelected = _branchOfficeService.GetByShopId(_currentUser.Shop.Id).Select(bo => bo.Id);
            return View(benefitForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateBenefitForm benefitForm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(benefitForm);
                }

                var benefit = benefitForm.ToBenefit();

                var currentShop = _shopService.GetByUserId(this.User.Identity.GetUserId());

                benefit.ShopId = currentShop.Id;

                _benefitService.Create(benefit);

                if (benefitForm.Files.Any(f => f != null))
                {
                    List<BenefitFile> benefitFiles = new List<BenefitFile>();

                    foreach (var file in benefitForm.Files)
                    {
                        var benefitFile = new BenefitFile();
                        benefitFile.BenefitId = benefit.Id;
                        benefitFile.File = file.ToFile();
                        benefitFiles.Add(benefitFile);
                    }

                    _benefitFileService.Create(benefitFiles);
                }

                var email = _currentUser.PointexUser.Email;

                await _notificationService.SendPendingBenefitEmail(benefit.Name, email, true, AppSettings.Theme);

                return RedirectToAction("Index", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Beneficio Creado");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Hubo un error en la solicitud. Por favor intente nuevamente más tarde");
                return View(benefitForm);
            }
        }

        public ActionResult Edit(int id)
        {
            var benefit = _benefitService.GetById(id);
            var benefitForm = BenefitForm.FromBenefit(benefit);
            return View(benefitForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, BenefitForm benefitForm)
        {
            if (!ModelState.IsValid)
            {
                return View(benefitForm);
            }

            var benefit = benefitForm.ToBenefit();

             await _benefitService.Edit(benefit, User, _currentUser.PointexUser.Email, AppSettings.Theme);

            return RedirectToAction("Index", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Beneficio Editado");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            _benefitService.Delete(id);

            return RedirectToAction("Index", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Beneficio Eliminado");
        }

        public ActionResult IsNameAvailable(string name, int id)
        {
            return Json(_benefitService.IsNameAvailable(name, id), JsonRequestBehavior.AllowGet);
        }
    }
}
