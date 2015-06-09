using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Routing;
using PointEx.Web.Models.List;

namespace PointEx.Web.Models
{
    public class PrizeListFiltersModel : FilterBaseModel
    {
        [Display(Name = "Palabra a Buscar", Prompt = "Palabra a Buscar")]
        public string Criteria { get; set; }

        [Display(Name = "A mi alcance")]
        public bool WithinReach { get; set; }

        [Display(Name = "A mi alcance")]
        public int? MaxPointsNeeded { get; set; }

        public override RouteValueDictionary GetRouteValues(int page = 1)
        {
            var routeValues = base.GetRouteValues(page);
            routeValues.Add("Criteria", this.Criteria);
            return routeValues;
        }
    }
}