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
    public class EducationalInstitutionController : AdminBaseController
    {
        private readonly IEducationalInstitutionService _educationalInstitutionService;

        public EducationalInstitutionController(IEducationalInstitutionService educationalInstitutionService)
        {
            _educationalInstitutionService = educationalInstitutionService;
        }

        public ActionResult Index(EducationalInstitutionListFiltersModel filters)
        {
            int pageTotal;

            var educationalInstitutions = _educationalInstitutionService.GetAll("CreatedDate", "DESC", filters.Criteria, filters.TownId, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<EducationalInstitutionDto>(educationalInstitutions, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new EducationalInstitutionListModel(pagedList, filters);

            return View(listModel);
        }

        public ActionResult Detail(int id)
        {
            var educationalInstitution = _educationalInstitutionService.GetById(id);
            var educationalInstitutionForm = EducationalInstitutionForm.FromEducationalInstitution(educationalInstitution);
            return View(educationalInstitutionForm);
        }

        public ActionResult Create()
        {
            var educationalInstitutionForm = new EducationalInstitutionForm();
            return View(educationalInstitutionForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(EducationalInstitutionForm educationalInstitutionForm)
        {
            if (!ModelState.IsValid)
            {
                return View(educationalInstitutionForm);
            }

            var educationalInstitution = educationalInstitutionForm.ToEducationalInstitution();

            _educationalInstitutionService.Create(educationalInstitution);

            return RedirectToAction("Index", new EducationalInstitutionListFiltersModel().GetRouteValues()).WithSuccess("Establecimiento Educativo Creado");
        }

        public ActionResult Edit(int id)
        {
            var educationalInstitution = _educationalInstitutionService.GetById(id);
            var educationalInstitutionForm = EducationalInstitutionForm.FromEducationalInstitution(educationalInstitution);
            return View(educationalInstitutionForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EducationalInstitutionForm educationalInstitutionForm)
        {
            if (!ModelState.IsValid)
            {
                return View(educationalInstitutionForm);
            }

            _educationalInstitutionService.Edit(educationalInstitutionForm.ToEducationalInstitution());

            return RedirectToAction("Index", new EducationalInstitutionListFiltersModel().GetRouteValues()).WithSuccess("Establecimiento Educativo Editado");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            _educationalInstitutionService.Delete(id);

            return RedirectToAction("Index", new EducationalInstitutionListFiltersModel().GetRouteValues()).WithSuccess("Establecimiento Educativo Eliminado");
        }

        public ActionResult IsNameAvailable(string name, int id)
        {
            return Json(_educationalInstitutionService.IsNameAvailable(name, id), JsonRequestBehavior.AllowGet);
        }
    }
}
