using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Web.Models;

namespace PointEx.Web.Models
{
    public class PurchaseListModel
    {
        public PurchaseListModel(List<PurchaseDto> list, PurchaseListFiltersModel filters)
        {
            List = list;
            Filters = filters;
        }

        public List<PurchaseDto> List { get; set; }

        public PurchaseListFiltersModel Filters { get; set; }
    }
}