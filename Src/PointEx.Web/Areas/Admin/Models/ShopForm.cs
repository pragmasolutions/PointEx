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
    public class ShopForm : IMapFrom<Shop>
    {
        public ShopForm()
        {
            CategoriesSelected = Enumerable.Empty<int>();
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
        
        [UIHint("Categories")]
        [Display(Name = "Categorias")]
        [NotMapped]
        public IEnumerable<int> CategoriesSelected { get; set; } 

        [Display(Name = "Ubicación")]
        public DbGeography Location { get; set; }

        public Shop ToShop()
        {
            var shop = Mapper.Map<ShopForm, Shop>(this);
            shop.ShopCategories =
                this.CategoriesSelected.Select(categoryId => new ShopCategory() {CategoryId = categoryId, ShopId = this.Id}).ToArray();
            return shop;
        }

        public static ShopForm FromShop(Shop shop)
        {
            var form = Mapper.Map<Shop, ShopForm>(shop);
            form.CategoriesSelected = shop.ShopCategories.Select(sc => sc.CategoryId);
            return form;
        }
    }
}
