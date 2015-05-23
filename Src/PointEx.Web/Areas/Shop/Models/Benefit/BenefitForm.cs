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
    public class BenefitForm : IMapFrom<Benefit>
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [Remote("IsNameAvailable", "Benefit", "Shop", ErrorMessage = "Ya existe un beneficio con este nombre", AdditionalFields = "Id")]
        public string Name { get; set; }

        [Display(Name = @"Descripción")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = @"Porcentaje de Descuento")]
        [UIHint("Percentage")]
        public decimal? DiscountPercentage { get; set; }

        [Display(Name = @"Tope Porcentaje de Descuento")]
        public decimal? DiscountPercentageCeiling { get; set; }

        public Benefit ToBenefit()
        {
            var prize = Mapper.Map<BenefitForm, Benefit>(this);
            return prize;
        }

        public static BenefitForm FromBenefit(Benefit prize)
        {
            var form = Mapper.Map<Benefit, BenefitForm>(prize);
            return form;
        }
    }
}
