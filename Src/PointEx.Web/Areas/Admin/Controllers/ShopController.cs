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

namespace PointEx.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator,SuperAdmin,ShopAdmin")]
    public class ShopController : BaseController
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        public ActionResult Index(ShopListFiltersModel filters)
        {
            int pageTotal;

            var shops = _shopService.GetAll("CreatedDate", "DESC", filters.Criteria, filters.CategoryId, filters.TownId, false, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<ShopDto>(shops, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new ShopListModel(pagedList, filters);

            return View(listModel);
        }

        public ActionResult Detail(int id)
        {
            var shop = _shopService.GetById(id);
            var shopForm = ShopForm.FromShop(shop);
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
                await _shopService.Create(shop, shopForm.Email);
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
    }
}
