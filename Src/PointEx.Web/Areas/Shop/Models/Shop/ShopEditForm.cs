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
    public class ShopEditForm : IMapFrom<Shop>
    {
        public ShopEditForm()
        {
            CategoriesSelected = Enumerable.Empty<int>();
        }

        [HiddenInput]
        public int Id { get; set; }
                
        [Display(Name = "Nombre")]       
        public string Name { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        [UIHint("TownId")]
        [Display(Name = "Localidad")]
        [Required]
        public int TownId { get; set; }
        
        [UIHint("Categories")]
        [Display(Name = "Categorias")]
        [NotMapped]
        public IEnumerable<int> CategoriesSelected { get; set; } 

        [Display(Name = "Ubicación")]
        public DbGeography Location { get; set; }

        public Shop ToShop()
        {
            var shop = Mapper.Map<ShopEditForm, Shop>(this);
            shop.ShopCategories =
                this.CategoriesSelected.Select(categoryId => new ShopCategory() {CategoryId = categoryId, ShopId = this.Id}).ToArray();            
            return shop;
        }

        public static ShopEditForm FromShop(Shop shop)
        {
            var form = Mapper.Map<Shop, ShopEditForm>(shop);
            form.CategoriesSelected = shop.ShopCategories.Select(sc => sc.CategoryId);
            return form;
        }
    }
}
