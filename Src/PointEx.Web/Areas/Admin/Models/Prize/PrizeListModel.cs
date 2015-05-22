using System.ComponentModel.DataAnnotations;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Web.Models;

namespace PointEx.Web.Models
{
    public class PrizeListModel
    {
        public PrizeListModel(IPagedList<PrizeDto> list, PrizeListFiltersModel filters)
        {
            List = list;
            Filters = filters;
        }
        public IPagedList<PrizeDto> List { get; set; }

        public PrizeListFiltersModel Filters { get; set; }
    }
}