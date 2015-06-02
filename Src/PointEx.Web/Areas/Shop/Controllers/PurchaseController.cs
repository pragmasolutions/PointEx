using System.Linq;
using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Service;
using PointEx.Web.Controllers;
using PointEx.Web.Infrastructure;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Shop.Controllers
{
    public class PurchaseController : ShopBaseController
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IShopService _shopService;
        private readonly ICurrentUser _currentUser;
        private readonly ICardService _cardService;

        public PurchaseController(IPurchaseService purchaseService, IShopService shopService, ICurrentUser currentUser, ICardService cardService)
        {
            _purchaseService = purchaseService;
            _shopService = shopService;
            _currentUser = currentUser;
            _cardService = cardService;
        }

        public ActionResult Index(PurchaseListFiltersModel filters)
        {
            var todayPurchases = _purchaseService.GetTodayPurchasesByShopId(_currentUser.Shop.Id, filters.BranchOfficeId).ToList();
            var purchaseListModel = new PurchaseListModel(todayPurchases, filters);
            return View(purchaseListModel);
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

            var card = _cardService.GetByNumber(purchaseForm.CardNumber);

            purchase.ShopId = _currentUser.Shop.Id;
            purchase.CardId = card.Id;

            _purchaseService.Create(purchase);

            return RedirectToAction("Index").WithSuccess("Compra Creada");
        }

        public ActionResult ValidateCardNumber(string cardNumber)
        {
            return Json(_cardService.ValidateCardNumber(cardNumber), JsonRequestBehavior.AllowGet);
        }
    }
}
