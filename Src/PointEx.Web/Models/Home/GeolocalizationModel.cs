using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Web.Models
{
    public class GeolocalizationModel
    {
        public GeolocalizationModel(IList<BenefitDto> benefits)
        {
            Benefits = benefits;
        }

        public IList<BenefitDto> Benefits { get; set; }
    }
}