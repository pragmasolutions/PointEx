using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Entities.Enums;
using PointEx.Service;
using PointEx.Web.Configuration;
using PointEx.Web.Controllers;
using PointEx.Web.Infrastructure;
using PointEx.Web.Infrastructure.Extensions;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Shop.Controllers
{
    [Authorize(Roles = "Administrator, SuperAdmin, Shop")]
    public class BenefitFileController : BaseController
    {
        private readonly IBenefitFileService _benefitFileService;
        private readonly IBenefitService _benefitService;
        private readonly ICurrentUser _currentUser;

        public BenefitFileController(IBenefitFileService benefitFileService, IBenefitService benefitService, ICurrentUser currentUser)
        {
            _benefitFileService = benefitFileService;
            _benefitService = benefitService;
            _currentUser = currentUser;
        }

        public ActionResult Index(int benefitId)
        {
            var benefitFiles = _benefitFileService.GetByBenefitId(benefitId);
            var benefit = _benefitService.GetById(benefitId);

            var benefitFilesModel = new BenefitFilesModel();
            benefitFilesModel.Benefit = benefit;
            benefitFilesModel.BenefitFiles = benefitFiles;
            ViewBag.ReturnController = _currentUser.Shop != null ? "Shop" : "Admin";
            return View(benefitFilesModel);
        }

        public ActionResult OrderImages(int benefitId)
        {
            var benefitFiles = _benefitFileService.GetByBenefitId(benefitId);
            var benefit = _benefitService.GetById(benefitId);

            var orderImagesForm = new OrderBenefitImagesForm();

            orderImagesForm.Id = benefitId;
            orderImagesForm.Benefit = benefit;
            orderImagesForm.Items = benefitFiles.Select(bf => new ImageToOrderModel() { Id = bf.Id, Url = bf.File.GetUrl() }).ToList();
            ViewBag.ReturnController = _currentUser.Shop != null ? "Shop" : "Admin";
            return View(orderImagesForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult OrderImages(OrderBenefitImagesForm orderImagesForm)
        {
            if (!ModelState.IsValid)
            {
                var benefit = _benefitService.GetById(orderImagesForm.Id);
                orderImagesForm.Benefit = benefit;
                return View(orderImagesForm);
            }

            var orderedImagesIds = orderImagesForm.Items.Select(i => i.Id).ToList();

            _benefitFileService.Order(orderImagesForm.Id, orderedImagesIds);

            return RedirectToAction("Index", new { benefitId = orderImagesForm.Id }).WithSuccess("Imagenes ordenadas");
        }


        public ActionResult UploadImages(int benefitId)
        {
            var benefitForm = new UploadBenefitImageForm();

            var benefit = _benefitService.GetById(benefitId);

            benefitForm.Benefit = benefit;
            benefitForm.Id = benefitId;
            ViewBag.ReturnController = _currentUser.Shop != null ? "Shop" : "Admin";

            return View(benefitForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UploadImages(UploadBenefitImageForm uploadImagesForm)
        {
            if (uploadImagesForm.Files.All(f => f == null))
            {
                return RedirectToAction("Index", new { benefitId = uploadImagesForm.Id });
            }

            if (!ModelState.IsValid)
            {
                var benefit = _benefitService.GetById(uploadImagesForm.Id);
                uploadImagesForm.Benefit = benefit;
                return View(uploadImagesForm);
            }

            List<BenefitFile> benefitFiles = new List<BenefitFile>();

            foreach (var file in uploadImagesForm.Files)
            {
                var benefitFile = new BenefitFile();
                benefitFile.BenefitId = uploadImagesForm.Id;
                benefitFile.File = file.ToFile();
                benefitFiles.Add(benefitFile);
            }

            _benefitFileService.Create(benefitFiles);

            var b = _benefitService.GetById(uploadImagesForm.Id);

            b.StatusId = StatusEnum.Pending;

            _benefitService.Edit(b, User, _currentUser.PointexUser.Email, AppSettings.Theme);
            
            return RedirectToAction("Index", new { benefitId = uploadImagesForm.Id }).WithSuccess("Imagenes subidas");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int benefitFileId, int benefitId, FormCollection collection)
        {
            _benefitFileService.Delete(benefitFileId);

            return RedirectToAction("Index", new { benefitId = benefitId }).WithSuccess("Imagen Eliminada");
        }
    }
}
