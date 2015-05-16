using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using PointEx.Service;
using PointEx.Web.Controllers;
using PointEx.Entities;

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
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Entities.Shop shop)
        {
            if (!ModelState.IsValid)
            {
                return View(shop);
            }

            _shopService.Create(shop);

            return RedirectToAction<ShopController>(c => c.Index()).WithSuccess("Comercio Creado");
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Entities.Shop shop)
        {
            if (!ModelState.IsValid)
            {
                return View(shop);
            }

            _shopService.Edit(shop);

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
