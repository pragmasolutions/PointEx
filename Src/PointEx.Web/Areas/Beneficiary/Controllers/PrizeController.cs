using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Security;
using PointEx.Service;
using PointEx.Web.Configuration;
using PointEx.Web.Models;
using PointEx.Web.Infrastructure;

namespace PointEx.Web.Areas.Beneficiary.Controllers
{
    public class PrizeController : BeneficiaryBaseController
    {
        private readonly IPrizeService _prizeService;
        private readonly IPointsExchangeService _pointsExchangeService;
        private readonly ICurrentUser _currentUser; 

        public PrizeController(IPrizeService prizeService, IPointsExchangeService pointsExchangeService, ICurrentUser currentUser)
        {
            _prizeService = prizeService;
            _pointsExchangeService = pointsExchangeService;
            _currentUser = currentUser;
        }

        public ActionResult Index(PrizeListFiltersModel filters)
        {
            int pageTotal;

            int? maxPointsNeeded = filters.WithinReach ? _currentUser.Beneficiary.Points : (int?)null;

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

        public ActionResult ExchangePoints(int id)
        {
            var prize = _prizeService.GetById(id);
            return View(prize);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ExchangePoints(int id, FormCollection formCollection)
        {
            await _pointsExchangeService.ExchangePoints(id, _currentUser.Beneficiary.Id, AppSettings.Theme);

            return RedirectToAction("ExchangePointsSuccess", new { id });
        }

        public ActionResult ExchangePointsSuccess(int id)
        {
            var prize = _prizeService.GetById(id);

            return View(prize);
        }
    }
}
