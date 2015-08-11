using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Service;
using PointEx.Web.Controllers;
using PointEx.Web.Infrastructure.Extensions;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Admin.Controllers
{
    public class SectionItemController : AdminBaseController
    {
        private readonly ISectionItemService _sectionItemService;
        private readonly ISectionService _sectionService;
        private readonly IBenefitService _benefitService;
        private readonly IPrizeService _prizeService;

        public SectionItemController(ISectionItemService sectionItemService, ISectionService sectionService, IBenefitService benefitService, IPrizeService prizeService)
        {
            _sectionItemService = sectionItemService;
            _sectionService = sectionService;
            _benefitService = benefitService;
            _prizeService = prizeService;
        }

        public ActionResult Index(int sectionId)
        {
            var sectionItems = _sectionItemService.GetBySectionId(sectionId);
            var section = _sectionService.GetById(sectionId);

            var sectionItemsModel = new SectionItemsModel();
            sectionItemsModel.Section = section;
            sectionItemsModel.SectionItems = sectionItems;

            return View(sectionItemsModel);
        }

        public ActionResult OrderItems(int sectionId)
        {
            var sectionItems = _sectionItemService.GetBySectionId(sectionId);
            var section = _sectionService.GetById(sectionId);

            var orderSectionItemsForm = new OrderSectionItemsForm();

            orderSectionItemsForm.Id = sectionId;
            orderSectionItemsForm.Section = section;
            orderSectionItemsForm.Items = sectionItems.ToList();

            return View(orderSectionItemsForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult OrderItems(OrderSectionItemsForm orderSectionItemsForm)
        {
            if (!ModelState.IsValid)
            {
                var section = _sectionService.GetById(orderSectionItemsForm.Id);
                orderSectionItemsForm.Section = section;
                return View(orderSectionItemsForm);
            }

            var orderedImagesIds = orderSectionItemsForm.Items.Select(i => i.Id).ToList();

            _sectionItemService.Order(orderSectionItemsForm.Id, orderedImagesIds);

            return RedirectToAction("Index", new { sectionId = orderSectionItemsForm.Id }).WithSuccess("Items ordenados");
        }

        public ActionResult AddBenefit(BenefitListFiltersModel filters, int sectionId)
        {
            var section = _sectionService.GetById(sectionId);

            int pageTotal = 0;
            var benefits = _benefitService.GetAll("CreatedDate", "DESC", filters.CategoryId, filters.TownId,
                filters.ShopId, filters.Criteria,true, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<BenefitDto>(benefits, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new BenefitListModel(pagedList, filters);

            var sectionItems = _sectionItemService.GetBySectionId(sectionId);

            var addBenefitModel = new AddBenefitModel();

            addBenefitModel.SectionName = section.Name;
            addBenefitModel.BenefitListModel = listModel;
            addBenefitModel.SelectedBenefitIds = sectionItems.Where(si => si.BenefitId.HasValue).Select(si => si.BenefitId.GetValueOrDefault()).ToList();
            addBenefitModel.SectionId = sectionId;

            return View(addBenefitModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddBenefit(AddBenefitForm addBenefitForm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var sectionItem = new SectionItem();

            sectionItem.SectionId = addBenefitForm.SectionId;
            sectionItem.BenefitId = addBenefitForm.BenefitId;

            _sectionItemService.Create(sectionItem);

            return RedirectToAction("Index", new { sectionId = addBenefitForm.SectionId }).WithSuccess("Beneficio Agregado");
        }

        public ActionResult AddPrize(PrizeListFiltersModel filters, int sectionId)
        {
            var section = _sectionService.GetById(sectionId);

            int pageTotal = 0;
            var benefits = _prizeService.GetAll("CreatedDate", "DESC", filters.Criteria, filters.MaxPointsNeeded, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<PrizeDto>(benefits, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new PrizeListModel(pagedList, filters);

            var sectionItems = _sectionItemService.GetBySectionId(sectionId);

            var addPrizeModel = new AddPrizeModel();

            addPrizeModel.SectionName = section.Name;
            addPrizeModel.PrizeListModel = listModel;
            addPrizeModel.SelectedPrizeIds = sectionItems.Where(si => si.PrizeId.HasValue).Select(si => si.PrizeId.GetValueOrDefault()).ToList();
            addPrizeModel.SectionId = sectionId;

            return View(addPrizeModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddPrize(AddPrizeForm addPrizeForm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var sectionItem = new SectionItem();

            sectionItem.SectionId = addPrizeForm.SectionId;
            sectionItem.PrizeId = addPrizeForm.PrizeId;

            _sectionItemService.Create(sectionItem);

            return RedirectToAction("Index", new { sectionId = addPrizeForm.SectionId }).WithSuccess("Premio Agregado");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int sectionItemId, int sectionId, FormCollection collection)
        {
            _sectionItemService.Delete(sectionItemId);

            return RedirectToAction("Index", new { sectionId = sectionId }).WithSuccess("Item Eliminado");
        }
    }
}
