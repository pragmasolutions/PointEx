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
    public class TyCFiltersModel : FilterBaseModel
    {
        private const string DonwloadActionPrefix = "Generate";

        public int BeneficiaryId { get; set; }
        public ReportTypeEnum ReportType { get; set; }
        public string ReportName { get; set; }

        public virtual RouteValueDictionary GetRouteValues(ReportTypeEnum rerportType = ReportTypeEnum.Pdf)
        {
            var routeValues = new RouteValueDictionary();
            routeValues.Add("ReportType", rerportType);
            routeValues.Add("BeneficiaryId", this.BeneficiaryId);
            return routeValues;
        }

        public string GetWordUrl()
        {
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return urlHelper.Action(DonwloadActionPrefix + ReportName, GetRouteValues(ReportTypeEnum.Word));
        }        

        public string GetPdfUrl()
        {
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return urlHelper.Action(DonwloadActionPrefix + ReportName, GetRouteValues(ReportTypeEnum.Pdf));
        }
    }
}