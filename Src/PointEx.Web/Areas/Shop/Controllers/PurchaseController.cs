using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Service;
using PointEx.Web.Controllers;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Shop.Controllers
{
    public class PurchaseController : BaseController
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IShopService _shopService;
        private readonly ICardService _cardService;

        public PurchaseController(IPurchaseService purchaseService, IShopService shopService, ICardService cardService)
        {
            _purchaseService = purchaseService;
            _shopService = shopService;
            _cardService = cardService;
        }

        public ActionResult Create()
        {
            var purchaseForm = new PurchaseForm();
            return View(purchaseForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseForm purchaseForm)
        {
            if (!ModelState.IsValid)
            {
                return View(purchaseForm);
            }

            var purchase = purchaseForm.ToPurchase();

            var currentShop = _shopService.GetByUserId(this.User.Identity.GetUserId());
            var card = _cardService.GetByNumber(purchaseForm.CardNumber);

            purchase.ShopId = currentShop.Id;
            purchase.CardId = card.Id;

            _purchaseService.Create(purchase);

            return RedirectToAction("Index", "Benefit").WithSuccess("Compra Creada");
        }

        public ActionResult ValidateCardNumber(string cardNumber)
        {
            return Json(_cardService.ValidateCardNumber(cardNumber), JsonRequestBehavior.AllowGet);
        }
    }
}
