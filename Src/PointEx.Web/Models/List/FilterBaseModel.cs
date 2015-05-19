using System;
using System.Web.Routing;

namespace PointEx.Web.Models.List
{
    public abstract class FilterBaseModel : IFilter
    {
        protected FilterBaseModel()
        {
            Page = 1;
        }

        public int Page { get; set; }

        public virtual RouteValueDictionary GetRouteValues(int page = 1)
        {
            return new RouteValueDictionary(new
            {
                Page = page
            });
        }
    }
}