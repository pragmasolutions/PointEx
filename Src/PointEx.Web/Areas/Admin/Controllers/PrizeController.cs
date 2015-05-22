using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Service;
using PointEx.Web.Areas.Admin.Models;
using PointEx.Web.Controllers;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Admin.Controllers
{
    public class PrizeController : BaseController
    {
        private readonly IPrizeService _prizeService;

        public PrizeController(IPrizeService prizeService)
        {
            _prizeService = prizeService;
        }

        public ActionResult Index(PrizeListFiltersModel filters)
        {
            int pageTotal;

            var prizes = _prizeService.GetAll("CreatedDate", "DESC", filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<PrizeDto>(prizes, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new PrizeListModel(pagedList, filters);

            return View(listModel);
        }

        public ActionResult Detail(int id)
        {
            var prize = _prizeService.GetById(id);
            var prizeForm = PrizeForm.FromPrize(prize);
            return View(prizeForm);
        }

        public ActionResult Create()
        {
            var prizeForm = new PrizeForm();
            return View(prizeForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(PrizeForm prizeForm)
        {
            if (!ModelState.IsValid)
            {
                return View(prizeForm);
            }

            var prize = prizeForm.ToPrize();

            _prizeService.Create(prize);

            return RedirectToAction("Index", new PrizeListFiltersModel().GetRouteValues()).WithSuccess("Premio Creado");
        }

        public ActionResult Edit(int id)
        {
            var prize = _prizeService.GetById(id);
            var prizeForm = PrizeForm.FromPrize(prize);
            return View(prizeForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PrizeForm prizeForm)
        {
            if (!ModelState.IsValid)
            {
                return View(prizeForm);
            }

            _prizeService.Edit(prizeForm.ToPrize());

            return RedirectToAction("Index", new PrizeListFiltersModel().GetRouteValues()).WithSuccess("Premio Editado");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            _prizeService.Delete(id);

            return RedirectToAction("Index", new PrizeListFiltersModel().GetRouteValues()).WithSuccess("Premio Eliminado");
        }

        public ActionResult IsNameAvailable(string name, int id)
        {
            return Json(_prizeService.IsNameAvailable(name, id), JsonRequestBehavior.AllowGet);
        }
    }
}
