using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Mapping;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class PurchaseForm : IMapFrom<Purchase>
    {
        [HiddenInput]
        public int Id { get; set; }

        [UIHint("BranchOfficeId")]
        [Display(Name = @"Sucursal")]
        public int BranchOfficeId { get; set; }

        [Required]
        [Display(Name = @"Monto")]
        [DataType(DataType.Currency)]
        public decimal? Amount { get; set; }

        [UIHint("BenefitId")]
        [Display(Name = @"Beneficio")]
        public int? BenefitId { get; set; }

        [Required]
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
