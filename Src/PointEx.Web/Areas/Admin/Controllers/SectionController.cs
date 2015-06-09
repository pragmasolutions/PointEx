using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Service;
using PointEx.Web.Controllers;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Admin.Controllers
{
    public class SectionController : AdminBaseController
    {
        private readonly ISectionService _sectionService;

        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        public ActionResult Index()
        {
            var sections = _sectionService.GetAll();
            return View(sections);
        }
    }
}
