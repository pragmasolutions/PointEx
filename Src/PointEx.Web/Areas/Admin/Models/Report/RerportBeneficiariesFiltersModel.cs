using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Framework.Report;
using PointEx.Web.Models.List;

namespace PointEx.Web.Models
{
    public class ReportBeneficiariesFiltersModel : ReportFiltersModel
    {
        public ReportBeneficiariesFiltersModel()
        {
            From = null;
            To = DateTime.Now.Date;
        }

        [UIHint("Sex")]
        [Display(Name = "Sexo")]
        public int? Sex { get; set; }

        [UIHint("TownId")]
        [Display(Name = "Localidad")]
        public int? TownId { get; set; }

        public override RouteValueDictionary GetRouteValues(ReportTypeEnum reportType = ReportTypeEnum.Pdf)
        {
            var routeValues = base.GetRouteValues(reportType);
            routeValues.Add("Sex", Sex);
            routeValues.Add("TownId", this.TownId);
            return routeValues;
        }
    }
}