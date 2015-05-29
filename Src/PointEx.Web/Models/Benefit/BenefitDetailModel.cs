using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class BenefitDetailModel
    {
        public BenefitDetailModel(Entities.Benefit benefit, Shop shop,IList<BenefitFile> images)
        {
            Benefit = benefit;
            Images = images;
            Shop = shop;
        }

        public PointEx.Entities.Benefit Benefit { get; set; }

        public IList<BenefitFile> Images { get; set; }

        public Shop Shop { get; set; }
    }
}