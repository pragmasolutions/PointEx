using System.ComponentModel.DataAnnotations;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Web.Models;

namespace PointEx.Web.Areas.Admin.Models
{
    public class ShopListModel
    {
        public ShopListModel(IPagedList<ShopDto> list, ShopListFiltersModel filters)
        {
            List = list;
            Filters = filters;
        }
        public IPagedList<ShopDto> List { get; set; }

        public ShopListFiltersModel Filters { get; set; }
    }
}