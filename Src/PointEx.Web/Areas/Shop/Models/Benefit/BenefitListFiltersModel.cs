using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Routing;
using PointEx.Web.Models.List;
using PointEx.Entities.Enums;

namespace PointEx.Web.Models
{
    public class BenefitListFiltersModel : FilterBaseModel
    {
        [Display(Name = "Palabra a Buscar", Prompt = "Palabra a Buscar")]
        public string Criteria { get; set; }

        [Display(Name = "Comercio")]
        [UIHint("ShopId")]
        public int? ShopId { get; set; }

        [Display(Name = "Categoria")]
        [UIHint("CategoryId")]
        public int? CategoryId { get; set; }

        [Display(Name = "Localidad")]
        [UIHint("TownId")]
        public int? TownId { get; set; }

        [Display(Name = "Estado")]
        [UIHint("BenefitStatusId")]
        public BenefitStatusEnum? BenefitStatusId { get; set; }        

        public override RouteValueDictionary GetRouteValues(int page = 1)
        {
            var routeValues = base.GetRouteValues(page);
            routeValues.Add("Criteria", this.Criteria);
            routeValues.Add("ShopId", this.ShopId);
            routeValues.Add("CategoryId", this.CategoryId);
            routeValues.Add("TownId", this.TownId);
            routeValues.Add("BenefitStatusId", this.BenefitStatusId);
            return routeValues;
        }
    }
}