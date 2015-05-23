using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Mapping;
using Framework.Common.Web.Metadata;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class PurchaseForm : IMapFrom<Purchase>
    {
        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = @"Monto")]
        [DataType(DataType.Currency)]
        public decimal? Amount { get; set; }

        [UIHint("BenefitId")]
        public int BenefitId { get; set; }

        [Display(Name = @"Nro Tarjeta")]
        [Remote("ValidateCardNumber", "Purchase", "Shop",ErrorMessage = "El numero de tarjeta no es válido")]
        public string CardNumber { get; set; }

        public Purchase ToPurchase()
        {
            var purchase = Mapper.Map<PurchaseForm, Purchase>(this);
            return purchase;
        }

        public static PurchaseForm FromPurchase(Purchase purchase)
        {
            var form = Mapper.Map<Purchase, PurchaseForm>(purchase);
            return form;
        }
    }
}
