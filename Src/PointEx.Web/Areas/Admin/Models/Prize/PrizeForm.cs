using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Mapping;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class PrizeForm : IMapFrom<Prize>
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [Remote("IsNameAvailable", "Prize", "Admin", ErrorMessage = "Ya existe un premio con este nombre", AdditionalFields = "Id")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Puntos necesarios")]
        [Range(1, int.MaxValue)]
        public int? PointsNeeded { get; set; }

        [Display(Name = @"Descripción")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public Prize ToPrize()
        {
            var prize = Mapper.Map<PrizeForm, Prize>(this);
            return prize;
        }

        public static PrizeForm FromPrize(Prize prize)
        {
            var form = Mapper.Map<Prize, PrizeForm>(prize);
            return form;
        }
    }
}
