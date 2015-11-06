using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Mapping;
using PointEx.Entities;
using Resources;

namespace PointEx.Web.Models
{
    public class AddMyShopForm : IMapFrom<Shop>
    {
        public AddMyShopForm()
        {
            CategoriesSelected = Enumerable.Empty<int>();
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Teléfono de Contacto")]
        [Required]
        public string Phone { get; set; }

        [UIHint("TownId")]
        [Display(Name = "Localidad")]
        [Required]
        public int TownId { get; set; }

        [UIHint("Categories")]
        [Display(Name = "Categorias")]
        [NotMapped]
        public IEnumerable<int> CategoriesSelected { get; set; } 

        [Required]
        [EmailAddress(ErrorMessageResourceType = typeof(PointExGlobalResources), ErrorMessageResourceName = "EmailAddress", ErrorMessage = null)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public Shop ToShop()
        {
            var shop = Mapper.Map<AddMyShopForm, Shop>(this);
            shop.ShopCategories =
                this.CategoriesSelected.Select(categoryId => new ShopCategory() { CategoryId = categoryId, ShopId = this.Id }).ToArray();            
            return shop;
        }
    }
}
