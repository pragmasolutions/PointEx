using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PointEx.Service;
using PointEx.Web.Controllers;
using PointEx.Entities;
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

        public ActionResult Index()
        {
            var shops = _shopService.GetAll();
            return View(shops);
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

            return RedirectToAction<ShopController>(c => c.Index()).WithSuccess("Comercio Creado");
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

            var currentShop = _shopService.GetById(shopForm.Id);

            _shopService.Edit(shopForm.PopulateShop(currentShop));

            return RedirectToAction<ShopController>(c => c.Index()).WithSuccess("Comercio Editado");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            _shopService.Delete(id);

            return RedirectToAction<ShopController>(c => c.Index()).WithSuccess("Comercio Eliminado");
        }
    }
}
