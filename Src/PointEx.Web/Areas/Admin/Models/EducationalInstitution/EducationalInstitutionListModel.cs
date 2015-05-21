using System.ComponentModel.DataAnnotations;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Admin.Models
{
    public class EducationalInstitutionListModel
    {
        public EducationalInstitutionListModel(IPagedList<EducationalInstitutionDto> list, EducationalInstitutionListFiltersModel filters)
        {
            List = list;
            Filters = filters;
        }
        public IPagedList<EducationalInstitutionDto> List { get; set; }

        public EducationalInstitutionListFiltersModel Filters { get; set; }
    }
}