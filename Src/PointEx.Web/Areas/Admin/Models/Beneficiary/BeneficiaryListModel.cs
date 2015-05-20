using System.ComponentModel.DataAnnotations;
using PagedList;
using PointEx.Entities.Dto;
using PointEx.Web.Models;

namespace PointEx.Web.Models
{
    public class BeneficiaryListModel
    {
        public BeneficiaryListModel(IPagedList<BeneficiaryDto> list, BeneficiaryListFiltersModel filters)
        {
            List = list;
            Filters = filters;
        }
        public IPagedList<BeneficiaryDto> List { get; set; }

        public BeneficiaryListFiltersModel Filters { get; set; }
    }
}