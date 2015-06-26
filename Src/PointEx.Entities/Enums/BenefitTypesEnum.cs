using System.ComponentModel.DataAnnotations;

namespace PointEx.Entities.Enums
{
    public enum BenefitTypesEnum
    {
        [Display(Name = "Descuento")]
        Discount = 1,
        [Display(Name = "2x1")]
        Promo_2x1,
        [Display(Name = "3x2")]
        Promo_3x2
    }
}
