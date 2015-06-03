using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class HomeModel
    {
        public HomeModel(IList<SectionItem> sliderItems, IList<Benefit> outstandingBenefits)
        {
            SliderItems = sliderItems;
            OutstandingBenefits = outstandingBenefits;
        }

        public IList<SectionItem> SliderItems { get; set; }

        public IList<Benefit> OutstandingBenefits { get; set; }
    }
}