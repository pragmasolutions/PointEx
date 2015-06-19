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
    public class GeneratedPointsReportFiltersModel : ReportFiltersModel
    {
        [UIHint("BeneficiaryId")]
        [Display(Name = @"Beneficiario")]
        public int? BeneficiaryId { get; set; }

        public override RouteValueDictionary GetRouteValues(ReportTypeEnum rerportType = ReportTypeEnum.Pdf)
        {
            var routeValues = new RouteValueDictionary();
            routeValues.Add("ReportType", rerportType);
            routeValues.Add("From", this.From.HasValue ? this.From.Value.ToShortDateString() : null);
            routeValues.Add("To", this.To.HasValue ? this.To.Value.ToShortDateString() : null);
            routeValues.Add("EducationalInstitutionId", this.EducationalInstitutionId);
            routeValues.Add("ShopId", this.ShopId);
            routeValues.Add("BeneficiaryId", this.BeneficiaryId);
            return routeValues;
        }
    }
}