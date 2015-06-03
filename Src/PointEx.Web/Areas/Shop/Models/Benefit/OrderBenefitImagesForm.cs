using System.Collections.Generic;
using System.Web.Mvc;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class OrderBenefitImagesForm : OrderImagesForm
    {
        public Benefit Benefit { get; set; }
    }
}
