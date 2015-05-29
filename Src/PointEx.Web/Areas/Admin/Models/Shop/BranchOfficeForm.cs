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
    public class BranchOfficeForm : IMapFrom<BranchOffice>
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public int ShopId { get; set; }

        [Display(Name = "Nombre Comercio")]
        public string ShopName { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        [UIHint("TownId")]
        [Display(Name = "Localidad")]
        [Required]
        public int TownId { get; set; }

        [Display(Name = "Ubicación")]
        public DbGeography Location { get; set; }

        public BranchOffice ToBranchOffice()
        {
            var shop = Mapper.Map<BranchOfficeForm, BranchOffice>(this);
            return shop;
        }

        public static BranchOfficeForm FromBranchOffice(BranchOffice branchOffice)
        {
            var form = Mapper.Map<BranchOffice, BranchOfficeForm>(branchOffice);
            return form;
        }
    }
}
