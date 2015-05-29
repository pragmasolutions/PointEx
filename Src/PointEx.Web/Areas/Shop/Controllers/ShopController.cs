using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Service;
using PointEx.Web.Areas.Admin.Models;
using PointEx.Web.Controllers;
using PointEx.Web.Models;
using PointEx.Web.Infrastructure;

namespace PointEx.Web.Areas.Shop.Controllers
{
    public class ShopController : ShopBaseController
    {
        private readonly IShopService _shopService;
        private readonly ICurrentUser _currentUser;

        public ShopController(IShopService shopService, ICurrentUser currentUser)
        {
            _shopService = shopService;
            _currentUser = currentUser;
        }
               
        public ActionResult Edit()
        {
            var shop = _shopService.GetById(_currentUser.Shop.Id);
            var shopEditForm = ShopEditForm.FromShop(shop);
            return View(shopEditForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ShopEditForm shopEditForm)
        {
            if (!ModelState.IsValid)
            {
                return View(shopEditForm);
            }
                        
            shopEditForm.Id = _currentUser.Shop.Id;
            shopEditForm.Name = _currentUser.Shop.Name;
            _shopService.Edit(shopEditForm.ToShop());

            return RedirectToAction("Index", "Purchase").WithSuccess("Perfíl Actualizado");
        }       
    }
}
