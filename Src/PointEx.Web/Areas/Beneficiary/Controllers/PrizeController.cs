using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Security;
using PointEx.Service;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Beneficiary.Controllers
{
    public class PrizeController : BeneficiaryBaseController
    {
        private readonly IPrizeService _prizeService;

        public PrizeController(IPrizeService prizeService)
        {
            _prizeService = prizeService;
        }

        public ActionResult Index(PrizeListFiltersModel filters)
        {
            int pageTotal;

            int? maxPointsNeeded = filters.WithinReach ? PointExContext.Beneficiary.Points : (int?)null;

            var prizes = _prizeService.GetAll("CreatedDate", "DESC", filters.Criteria, maxPointsNeeded, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<PrizeDto>(prizes, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new PrizeListModel(pagedList, filters);

            return View(listModel);
        }

        public ActionResult Detail(int id)
        {
            var prize = _prizeService.GetById(id);
            return View(prize);
        }

        public ActionResult Exchange(int id)
        {
            var prize = _prizeService.GetById(id);
            var prizeForm = PrizeForm.FromPrize(prize);
            return View(prizeForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Exchange(int id, FormCollection formCollection)
        {
            //_prizeService.Edit(prizeForm.ToPrize());
            return RedirectToAction("Index", new PrizeListFiltersModel().GetRouteValues()).WithSuccess("");
        }
    }
}
