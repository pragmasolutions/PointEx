using System.Web;
using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Service;
using PointEx.Web.Areas.Shop.Models;
using PointEx.Web.Controllers;
using PointEx.Web.Infrastructure;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Shop.Controllers
{
    public class BranchOfficeController : ShopBaseController
    {
        private readonly IBranchOfficeService _branchOfficeService;
        private readonly IShopService _shopService;
        private readonly ICurrentUser _currentUser;

        public BranchOfficeController(IBranchOfficeService branchOfficeService, IShopService shopService, ICurrentUser currentUser)
        {
            _branchOfficeService = branchOfficeService;
            _shopService = shopService;
            _currentUser = currentUser;
        }

        public ActionResult Index()
        {
            var shopId = _currentUser.Shop.Id;
            var branchOffices = _branchOfficeService.GetByShopId(shopId);
            var shop = _shopService.GetById(shopId);

            var shopBranchOfficesModel = new ShopBranchOfficesModel(shop, branchOffices);

            return View(shopBranchOfficesModel);
        }

        public ActionResult Detail(int id)
        {
            var shop = _branchOfficeService.GetById(id);
            var shopForm = BranchOfficeForm.FromBranchOffice(shop);
            return View(shopForm);
        }

        public ActionResult Create()
        {            
            var shopForm = new BranchOfficeForm();
            shopForm.ShopName = _currentUser.Shop.Name;         
            return View(shopForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(BranchOfficeForm shopForm)
        {
            if (!ModelState.IsValid)
            {
                return View(shopForm);
            }

            var shop = shopForm.ToBranchOffice();
            shop.ShopId = _currentUser.Shop.Id;

            _branchOfficeService.Create(shop);

            return RedirectToAction("Index", new { shopId = shopForm.ShopId }).WithSuccess("Sucursal Creada");
        }

        public ActionResult Edit(int id)
        {
            var shop = _branchOfficeService.GetById(id);
            var shopForm = BranchOfficeForm.FromBranchOffice(shop);
            return View(shopForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BranchOfficeForm shopForm)
        {
            if (!ModelState.IsValid)
            {
                return View(shopForm);
            }

            _branchOfficeService.Edit(shopForm.ToBranchOffice());

            return RedirectToAction("Index", new { shopId = shopForm.ShopId }).WithSuccess("Sucursal Editada");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int shopId, FormCollection collection)
        {
            _branchOfficeService.Delete(id);

            return RedirectToAction("Index", new { shopId = shopId }).WithSuccess("Sucursal Eliminada");
        }
    }
}
