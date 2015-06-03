using System.Collections.Generic;
using System.Web.Mvc;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class OrderItemsForm<T>
    {
        public OrderItemsForm()
        {
            Items = new List<T>();
        }

        [HiddenInput]
        public int Id { get; set; }

        public IList<T> Items { get; set; }
    }

    public class OrderImagesForm : OrderItemsForm<ImageToOrderModel>
    {
    }

    public class ImageToOrderModel
    {
        [HiddenInput]
        public int Id { get; set; }

        public string Url { get; set; }
    }
}
