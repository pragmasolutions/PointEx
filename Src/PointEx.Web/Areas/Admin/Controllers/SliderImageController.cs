using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Service;
using PointEx.Web.Areas.Admin.Models;
using PointEx.Web.Controllers;
using PointEx.Web.Models;
using System;
using System.Web.Routing;
using Framework.Data.Helpers;

namespace PointEx.Web.Areas.Admin.Controllers
{
    public class SliderImageController : AdminBaseController
    {
        private readonly ISliderImageService _sliderImageService;
        private readonly ISectionItemService _sectionItemService;
        

        public SliderImageController(ISliderImageService sliderImageService, ISectionItemService sectionItemService)
        {
            _sliderImageService = sliderImageService;
            _sectionItemService = sectionItemService;
        }

        public ActionResult Index(SliderImageListFiltersModel filters)
        {
            int pageTotal;

            var sliderImages = _sliderImageService.GetAll("CreatedDate", "DESC", filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<SliderImageDto>(sliderImages, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new SliderImageListModel(pagedList, filters);

            return View(listModel);
        }

        public ActionResult Detail(int id, int sectionId)
        {
            var sliderImage = _sliderImageService.GetById(id);
            var sliderImageForm = SliderImageForm.FromSliderImage(sliderImage);
            sliderImageForm.SectionId = sectionId;
            return View(sliderImageForm);
        }

        public ActionResult Create()
        {
            var sliderImageForm = new SliderImageForm();
            sliderImageForm.SectionId = Convert.ToInt32(Request.QueryString["sectionId"]);
            return View(sliderImageForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(SliderImageForm sliderImageForm)
        {
            if (!ModelState.IsValid)
            {
                return View(sliderImageForm);
            }

            var sliderImage = sliderImageForm.ToSliderImage();

            _sliderImageService.Create(sliderImage);

            return RedirectToAction("AddSliderImage", "SectionItem", new {@sectionId = sliderImageForm.SectionId}).WithSuccess("Imágen Creada");
        }

        public ActionResult Edit(int id, int sectionId)
        {
            var sliderImage = _sliderImageService.GetById(id);
            var sliderImageForm = SliderImageForm.FromSliderImage(sliderImage);
            sliderImageForm.SectionId = sectionId;
            return View(sliderImageForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SliderImageForm sliderImageForm)
        {
            if (!ModelState.IsValid)
            {
                return View(sliderImageForm);
            }

            _sliderImageService.Edit(sliderImageForm.ToSliderImage());

            return Redirect("/Admin/SectionItem/AddSliderImage?sectionId=" + sliderImageForm.SectionId).WithSuccess("Imágen Editada");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int sectionId, FormCollection collection)
        {
            var sectionItems = _sectionItemService.GetBySliderImage(id);
            foreach (var item in sectionItems)
            {
                _sectionItemService.Delete(item.Id);
            }
            _sliderImageService.Delete(id);

            return Redirect("/Admin/SectionItem/AddSliderImage?sectionId=" + sectionId).WithSuccess("Imágen Eliminada");
        }

        public ActionResult IsNameAvailable(string name, int id)
        {
            return Json(_sliderImageService.IsNameAvailable(name, id), JsonRequestBehavior.AllowGet);
        }
    }
}
