using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Mapping;
using Framework.Common.Validation;
using PointEx.Entities;
using PointEx.Security.Model;
using PointEx.Web.Configuration;
using Resources;

namespace PointEx.Web.Models
{
    public class BeneficiaryForm : IMapFrom<Beneficiary>
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre y Apellido")]
        public string Name { get; set; }
        
        [Display(Name = "DNI")]
        [Required]
        public string IdentificationNumber { get; set; }

        [UIHint("TownId")]
        [Display(Name = "Localidad")]
        [Required]
        public int TownId { get; set; }

        [UIHint("Sex")]
        [Display(Name = "Sexo")]
        [Required]
        public int Sex { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Barrio")]
        public string Neighborhood { get; set; }

        [UIHint("Long")]
        [Display(Name = "Número de Teléfono")]
        public long? TelephoneNumber { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime? BirthDate { get; set; }

        public string Theme { get; set; }

        [UIHint("EducationalInstitutionId")]
        [Display(Name = "Establecimiento Educativo")]
        [RequiredIf("Theme", ThemeEnum.TarjetaVerde, ErrorMessage = "El campo Establecimiento Educativo es requerido")]
        public int? EducationalInstitutionId { get; set; }


        [Required]
        [EmailAddress(ErrorMessageResourceType = typeof(PointExGlobalResources), ErrorMessageResourceName = "EmailAddress", ErrorMessage = null)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public Beneficiary ToBeneficiary()
        {
            var beneficiary = Mapper.Map<BeneficiaryForm, Beneficiary>(this);
            return beneficiary;
        }

        public static BeneficiaryForm Create(Beneficiary beneficiary, ApplicationUser user)
        {
            var form = Mapper.Map<Beneficiary, BeneficiaryForm>(beneficiary);

            form.Email = beneficiary.User.Email;

            return form;
        }
    }
}
