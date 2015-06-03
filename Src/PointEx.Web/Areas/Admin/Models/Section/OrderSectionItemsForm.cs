using System.Collections.Generic;
using System.Web.Mvc;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class OrderSectionItemsForm : OrderItemsForm<SectionItem>
    {
        public Section Section { get; set; }
    }
}
