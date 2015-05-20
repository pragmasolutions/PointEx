using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Routing;
using PointEx.Web.Models.List;

namespace PointEx.Web.Models
{
    public class ShopListFiltersModel : FilterBaseModel
    {
        public Guid CurrentRowId { get; set; }

        [Display(Name = "Categoria")]
        public int? CategoryId { get; set; }

        [UIHint("TownId")]
        public int? TownId { get; set; }

        [Display(Name = "Palabra a Buscar", Prompt = "Palabra a Buscar")]
        public string Criteria { get; set; }

        public override RouteValueDictionary GetRouteValues(int page = 1)
        {
            var routeValues = base.GetRouteValues(page);
            routeValues.Add("CategoryId", this.CategoryId);
            routeValues.Add("Criteria", this.Criteria);
            return routeValues;
        }
    }
}