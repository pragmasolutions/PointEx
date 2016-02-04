using System;
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
using PointEx.Entities.Enums;

namespace PointEx.Web.Models
{
    public class BenefitForm : IMapFrom<Benefit>
    {

        public BenefitForm()
        {
            BranchOfficesSelected = Enumerable.Empty<int>();
        }

        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public int ShopId { get; set; }

        [HiddenInput]
        public PointEx.Entities.Enums.StatusEnum StatusId { get; set; }

        [HiddenInput]
        public bool? IsApproved { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [Remote("IsNameAvailable", "Benefit", "Shop", ErrorMessage = "Ya existe un beneficio con este nombre", AdditionalFields = "Id")]
        public string Name { get; set; }

        [Display(Name = @"Descripción")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [UIHint("BenefitType")]        
        [Display(Name = "Tipo Beneficio")]
        [Required]
        public BenefitTypesEnum? BenefitTypeId { get; set; }

        [Display(Name = @"Porcentaje de Descuento")]
        [UIHint("Percentage")]
        public decimal? DiscountPercentage { get; set; }

        [Display(Name = @"Tope Porcentaje de Descuento")]
        public decimal? DiscountPercentageCeiling { get; set; }

        [Display(Name = "Fecha Desde")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "Fecha Hasta")]
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }

        [UIHint("BranchOffices")]
        [Display(Name = "Sucursales")]
        [NotMapped]
        public IEnumerable<int> BranchOfficesSelected { get; set; }
        
        public Benefit ToBenefit()
        {
            var benefit = Mapper.Map<BenefitForm, Benefit>(this);
            benefit.BenefitBranchOffices =
                this.BranchOfficesSelected.Select(branchOfficeId => new BenefitBranchOffice() { BranchOfficeId = branchOfficeId, BenefitId = this.Id }).ToArray();            
            return benefit;
        }

        public static BenefitForm FromBenefit(Benefit benefit)
        {
            var form = Mapper.Map<Benefit, BenefitForm>(benefit);
            form.BranchOfficesSelected = benefit.BenefitBranchOffices.Select(bbo => bbo.BranchOfficeId);
            return form;
        }       
    }
}
