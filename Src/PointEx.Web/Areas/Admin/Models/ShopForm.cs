using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Mapping;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class ShopForm : IMapFrom<Shop>
    {
        public ShopForm()
        {
            Categories = Enumerable.Empty<int>();
        }

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
        public DbGeography Location { get; set; }
        
        [UIHint("Categories")]
        [Display(Name = "Categorias")]
        public IEnumerable<int> Categories { get; set; } 

        public Shop ToShop()
        {
            return Mapper.Map<ShopForm, Shop>(this);
        }

        public Shop PopulateShop(Shop shop)
        {
            return Mapper.Map<ShopForm, Shop>(this, shop);
        }

        public static ShopForm FromShop(Shop shop)
        {
            return Mapper.Map<Shop, ShopForm>(shop);
        }
    }
}
