using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Routing;
using PointEx.Web.Models.List;

namespace PointEx.Web.Models
{
    public class PurchaseListFiltersModel : FilterBaseModel
    {
        [UIHint("BranchOfficeId")]
        [Display(Name = @"Sucursal")]
        public int? BranchOfficeId { get; set; }

        public override RouteValueDictionary GetRouteValues(int page = 1)
        {
            var routeValues = base.GetRouteValues(page);
            routeValues.Add("BranchOfficeId", this.BranchOfficeId);
            return routeValues;
        }
    }
}