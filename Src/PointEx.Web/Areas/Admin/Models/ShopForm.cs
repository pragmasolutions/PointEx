using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Mapping;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class ShopForm : IMapFrom<Shop>
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Dirección es requerido")]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [UIHint("TownId")]
        [Display(Name = "Localidad")]
        [Required(ErrorMessage = "El campo Localidad es requerido")]
        public int TownId { get; set; }

        [HiddenInput]
        public string UserId { get; set; }

        [Display(Name = "Ubicación")]
        [RegularExpression(@"^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$", 
            ErrorMessage = "La Ubicación no es valida")]
        public System.Data.Entity.Spatial.DbGeography Location { get; set; }

        public Shop ToShop()
        {
            return Mapper.Map<ShopForm, Entities.Shop>(this);
        }

        public static ShopForm FromShop(Shop shop)
        {
            return Mapper.Map<Entities.Shop, ShopForm>(shop);
        }
    }
}
