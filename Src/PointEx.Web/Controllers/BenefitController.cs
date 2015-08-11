using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Service;
using PointEx.Web.Models;

namespace PointEx.Web.Controllers
{
    public class BenefitController : BaseController
    {
        private readonly IBenefitService _benefitService;
        private readonly IShopService _shopService;
        private readonly IBenefitFileService _benefitFileService;
        private readonly IBranchOfficeService _branchOfficeService;

        public BenefitController(IBenefitService benefitService, IShopService shopService, IBenefitFileService benefitFileService, IBranchOfficeService branchOfficeService)
        {
            _benefitService = benefitService;
            _shopService = shopService;
            _benefitFileService = benefitFileService;
            _branchOfficeService = branchOfficeService;
        }

        public ActionResult Index(BenefitListFiltersModel filters)
        {
            int pageTotal;

            var benefits = _benefitService.GetAll("CreatedDate", "DESC", filters.CategoryId, filters.TownId, filters.ShopId, filters.Criteria, true, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<BenefitDto>(benefits, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new BenefitListModel(pagedList, filters);

            return View(listModel);
        }

        public ActionResult Detail(int id)
        {
            var benefit = _benefitService.GetById(id);

            if (benefit.IsApproved.HasValue && benefit.IsApproved.Value)
            {
                if (benefit == null)
                {
                    return HttpNotFound();
                }

                var shop = _shopService.GetById(benefit.ShopId);
                var images = _benefitFileService.GetByBenefitId(benefit.Id);
                var branchOffices = _branchOfficeService.GetByShopId(benefit.ShopId);

                var benefitDetailModel = new BenefitDetailModel(benefit, shop, images, branchOffices);

                return View(benefitDetailModel);
            }
            else
            {
                throw new HttpException(404, "Not found");
            }


        }
    }
}