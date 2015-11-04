using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Security.Model;
using PointEx.Service;
using PointEx.Web.Areas.Admin.Models;
using PointEx.Web.Controllers;
using PointEx.Web.Models;
using PointEx.Web.Configuration;
using PointEx.Entities.Enums;
using PointEx.Security;

namespace PointEx.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator,SuperAdmin,ShopAdmin")]
    public class ShopController : BaseController
    {
        private readonly IShopService _shopService;
        private readonly INotificationService _notificationService;

        public ShopController(IShopService shopService, INotificationService notificationService)
        {
            _shopService = shopService;
            _notificationService = notificationService;
        }

        public ActionResult Index(ShopListFiltersModel filters)
        {
            int pageTotal;

            var shops = _shopService.GetShopByStatus("CreatedDate", "DESC", filters.CategoryId, filters.TownId, StatusEnum.Pending, filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<ShopDto>(shops, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new ShopListModel(pagedList, filters);

            ViewBag.ViewMode = StatusEnum.Pending;
            ViewBag.TabTitle = "Comercios Pendientes";
            ViewBag.Title = "Comercios Pendientes de Aprobación";

            return View(listModel);
        }

        public ActionResult Detail(int id)
        {
            var shop = _shopService.GetById(id);
            var shopForm = ShopForm.FromShop(shop);

            ViewBag.ShowApprovalButtons = User.IsInRole(RolesNames.Admin) && shop.StatusId == StatusEnum.Pending;
            return View(shopForm);
        }

        public ActionResult Create()
        {
            var shopForm = new ShopForm();
            return View(shopForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ShopForm shopForm)
        {
            if (!ModelState.IsValid)
            {
                return View(shopForm);
            }

            var shop = shopForm.ToShop();

            try
            {
                shop.StatusId = StatusEnum.Approved;
                await _shopService.Create(shop, shopForm.Email, AppSettings.Theme);
                await _notificationService.SendAccountConfirmationEmail(shop.UserId, AppSettings.Theme);
            }
            catch (ApplicationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(shopForm);
            }

            return RedirectToAction("Index", new ShopListFiltersModel().GetRouteValues()).WithSuccess("Comercio Creado");
        }

        public ActionResult Edit(int id)
        {
            var shop = _shopService.GetById(id);
            var shopForm = ShopForm.FromShop(shop);
            return View(shopForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ShopForm shopForm)
        {
            if (!ModelState.IsValid)
            {
                return View(shopForm);
            }

            _shopService.Edit(shopForm.ToShop(), shopForm.Email);

            return RedirectToAction("Index", new ShopListFiltersModel().GetRouteValues()).WithSuccess("Comercio Editado");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            _shopService.Delete(id);

            return RedirectToAction("Index", new ShopListFiltersModel().GetRouteValues()).WithSuccess("Comercio Eliminado");
        }

        public ActionResult ApprovedShop(ShopListFiltersModel filters)
        {
            int pageTotal;

            var shops = _shopService.GetShopByStatus("CreatedDate", "ASC", filters.CategoryId, filters.TownId, StatusEnum.Approved, filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<ShopDto>(shops, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new ShopListModel(pagedList, filters);

            ViewBag.ViewMode = StatusEnum.Approved;
            ViewBag.TabTitle = "Comercios Aprobados";
            ViewBag.Title = "Comercios Aprobados";

            return View("Index", listModel);
        }

        public ActionResult RejectedShop(ShopListFiltersModel filters)
        {
            int pageTotal;

            var shops = _shopService.GetShopByStatus("CreatedDate", "ASC", filters.CategoryId, filters.TownId, StatusEnum.Rejected, filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<ShopDto>(shops, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new ShopListModel(pagedList, filters);

            ViewBag.ViewMode = StatusEnum.Rejected;
            ViewBag.TabTitle = "Comercios Rechazados";
            ViewBag.Title = "Comercios Rechazados";

            return View("Index", listModel);
        }

        [HttpPost]
        public async Task<ActionResult> Approved(int id)
        {
            _shopService.Moderated(id, (int)StatusEnum.Approved);

            var shop = _shopService.GetById(id);
            await _notificationService.SendShopApprovedMail(shop, AppSettings.SiteBaseUrl);
            await _notificationService.SendAccountConfirmationEmail(shop.UserId, AppSettings.Theme);

            if (Configuration.AppSettings.SiteBaseUrl.Contains("ApprovedShop"))
            {
                return RedirectToAction("RejectedShop", new BenefitListFiltersModel().GetRouteValues()).WithSuccess("Comercio Aprobado");
            }
            else
            {
                return RedirectToAction("Index", new ShopListFiltersModel().GetRouteValues()).WithSuccess("Comercio Aprobado");
            }
        }

        [HttpPost]
        public ActionResult Rejected(int id)
        {
            _shopService.Moderated(id, (int)StatusEnum.Rejected);
            if (Configuration.AppSettings.SiteBaseUrl.Contains("RejectedShop"))
            {
                return RedirectToAction("ApprovedShop", new ShopListFiltersModel().GetRouteValues()).WithSuccess("Comercio Rechazado");
            }
            else
            {
                return RedirectToAction("Index", new ShopListFiltersModel().GetRouteValues()).WithSuccess("Comercio Aprobado");
            }

        }       
    }
}
