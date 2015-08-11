using System.ComponentModel.DataAnnotations;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Web.Models;

namespace PointEx.Web.Models
{
    public class SliderImageListModel
    {
        public SliderImageListModel(IPagedList<SliderImageDto> list, SliderImageListFiltersModel filters)
        {
            List = list;
            Filters = filters;
        }
        public IPagedList<SliderImageDto> List { get; set; }

        public SliderImageListFiltersModel Filters { get; set; }
    }
}