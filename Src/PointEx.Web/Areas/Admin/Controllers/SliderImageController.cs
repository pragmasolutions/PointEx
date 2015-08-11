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

namespace PointEx.Web.Areas.Admin.Controllers
{
    public class SliderImageController : AdminBaseController
    {
        private readonly ISliderImageService _sliderImageService;

        public SliderImageController(ISliderImageService sliderImageService)
        {
            _sliderImageService = sliderImageService;
        }

        public ActionResult Index(SliderImageListFiltersModel filters)
        {
            int pageTotal;

            var sliderImages = _sliderImageService.GetAll("CreatedDate", "DESC", filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<SliderImageDto>(sliderImages, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new SliderImageListModel(pagedList, filters);

            return View(listModel);
        }

        public ActionResult Detail(int id)
        {
            var sliderImage = _sliderImageService.GetById(id);
            var sliderImageForm = SliderImageForm.FromSliderImage(sliderImage);
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

        public ActionResult Edit(int id)
        {
            var sliderImage = _sliderImageService.GetById(id);
            var sliderImageForm = SliderImageForm.FromSliderImage(sliderImage);
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

            return RedirectToAction("Index", new SliderImageListFiltersModel().GetRouteValues()).WithSuccess("Imágen Editado");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            _sliderImageService.Delete(id);

            return RedirectToAction("Index", new SliderImageListFiltersModel().GetRouteValues()).WithSuccess("Imágen Eliminado");
        }

        public ActionResult IsNameAvailable(string name, int id)
        {
            return Json(_sliderImageService.IsNameAvailable(name, id), JsonRequestBehavior.AllowGet);
        }
    }
}
