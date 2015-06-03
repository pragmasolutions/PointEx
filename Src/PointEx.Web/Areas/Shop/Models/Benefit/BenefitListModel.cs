using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Web.Models;

namespace PointEx.Web.Models
{
    public class BenefitListModel
    {
        public BenefitListModel(IPagedList<BenefitDto> list, BenefitListFiltersModel filters)
        {
            List = list;
            Filters = filters;
        }

        public IPagedList<BenefitDto> List { get; set; }

        public BenefitListFiltersModel Filters { get; set; }
    }
}