using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Routing;
using PointEx.Web.Models.List;

namespace PointEx.Web.Models
{
    public class BeneficiaryListFiltersModel : FilterBaseModel
    {
        [UIHint("TownId")]
        public int? TownId { get; set; }

        [Display(Name = "Palabra a Buscar", Prompt = "Palabra a Buscar")]
        public string Criteria { get; set; }

        [UIHint("EducationalInstitutionId")]
        [Display(Name = "Establecimiento Educativo")]
        public int? EducationalInstitutionId { get; set; }

        public override RouteValueDictionary GetRouteValues(int page = 1)
        {
            var routeValues = base.GetRouteValues(page);
            routeValues.Add("Criteria", this.Criteria);
            routeValues.Add("TownId", this.TownId);
            routeValues.Add("EducationalInstitutionId", this.EducationalInstitutionId);
            return routeValues;
        }
    }
}