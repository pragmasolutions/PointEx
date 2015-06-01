using System.Collections.Generic;
using System.Drawing.Printing;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class AddPrizeModel
    {
        public PrizeListModel PrizeListModel { get; set; }

        public IList<int> SelectedPrizeIds { get; set; }

        public string SectionName { get; set; }

        public int SectionId { get; set; }
    }

    public class AddPrizeForm
    {
        public int SectionId { get; set; }

        public int PrizeId { get; set; }
    }
}
