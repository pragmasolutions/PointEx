using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Service;
using PointEx.Web.Controllers;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Web.Areas.Admin.Models;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Admin.Controllers
{
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

            var shops = _shopService.GetAll("CreatedDate", "DESC", null, null, filters.Page, DefaultPageSize, out pageTotal);

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
        public ActionResult Create(ShopForm shopForm)
        {
            if (!ModelState.IsValid)
            {
                return View(shopForm);
            }

            var shop = shopForm.ToShop();

            shop.UserId = this.User.Identity.GetUserId();

            _shopService.Create(shop);

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

            _shopService.Edit(shopForm.ToShop());

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
