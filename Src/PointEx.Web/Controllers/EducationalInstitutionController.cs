using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Service;

namespace PointEx.Web.Controllers
{
    public class EducationalInstitutionController : Controller
    {
        private IEducationalInstitutionService _educationalInstitutionService;

        public EducationalInstitutionController(IEducationalInstitutionService educationalInstitutionService)
        {
            _educationalInstitutionService = educationalInstitutionService;
        }

        public ActionResult GetListByTown(int id)
        {
            var list = _educationalInstitutionService.GetByTown(id);
            return Json(list.Select(x => new
            {
                id = x.Id,
                text = x.Name
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}