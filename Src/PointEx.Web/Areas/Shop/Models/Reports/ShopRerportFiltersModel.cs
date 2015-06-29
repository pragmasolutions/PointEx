using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Framework.Report;
using PointEx.Web.Models.List;

namespace PointEx.Web.Areas.Shop.Models.Reports
{
    public class ShopReportFiltersModel : FilterBaseModel
    {
        private const string DonwloadActionPrefix = "GenerateReport";

        public ShopReportFiltersModel()
        {
            From = DateTime.Now.Date.AddMonths(-1);
            To = DateTime.Now.Date;
        }

        [Display(Name = "Desde")]
        [DataType(DataType.Date)]
        public DateTime? From { get; set; }

        [Display(Name = "Hasta")]
        [DataType(DataType.Date)]
        public DateTime? To { get; set; }

        public ReportTypeEnum ReportType { get; set; }

        public int ShopId { get; set; }

        public string ReportName { get; set; }

        public virtual RouteValueDictionary GetRouteValues(ReportTypeEnum rerportType = ReportTypeEnum.Pdf)
        {
            var routeValues = new RouteValueDictionary();
            routeValues.Add("ReportType", rerportType);
            routeValues.Add("From", this.From.HasValue ? this.From.Value.ToShortDateString() : null);
            routeValues.Add("To", this.To.HasValue ? this.To.Value.ToShortDateString() : null);
            routeValues.Add("ShopId", this.ShopId);
            return routeValues;
        }

        public string GetWordUrl()
        {
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return urlHelper.Action(DonwloadActionPrefix + ReportName, GetRouteValues(ReportTypeEnum.Word));
        }

        public string GetExcelUrl()
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