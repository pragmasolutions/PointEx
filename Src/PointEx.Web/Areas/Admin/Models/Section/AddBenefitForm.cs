using System.Collections.Generic;
using System.Drawing.Printing;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class AddBenefitModel
    {
        public BenefitListModel BenefitListModel { get; set; }

        public IList<int> SelectedBenefitIds { get; set; }

        public string SectionName { get; set; }

        public int SectionId { get; set; }
    }

    public class AddBenefitForm
    {
        public int SectionId { get; set; }

        public int BenefitId { get; set; }
    }
}
