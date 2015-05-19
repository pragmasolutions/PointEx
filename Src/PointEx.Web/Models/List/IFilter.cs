using System;
using System.Linq.Expressions;
using System.Web.Routing;

namespace PointEx.Web.Models.List
{
    public interface IFilter
    {
        RouteValueDictionary GetRouteValues(int page = 1);
    }
}
