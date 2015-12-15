using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Mapping;
using PointEx.Entities;
using PointEx.Security.Model;
using Resources;

namespace PointEx.Web.Models
{
    public class AddBeneficiaryForm : IMapFrom<Beneficiary>
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
        public DateTime? BirthDate { get; set; }

        [UIHint("EducationalInstitutionId")]
        [Display(Name = "Establecimiento Educativo")]
        [Required]
        public int? EducationalInstitutionId { get; set; }

        //[Display(Name = "Informacion de usuario")]
        //public RegisterViewModel RegisterViewModel { get; set; }

        [Required]
        [EmailAddress(ErrorMessageResourceType = typeof(PointExGlobalResources), ErrorMessageResourceName = "EmailAddress", ErrorMessage = null)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public Beneficiary ToBeneficiary()
        {
            var beneficiary = Mapper.Map<AddBeneficiaryForm, Beneficiary>(this);
            return beneficiary;
        }

        public static AddBeneficiaryForm Create(Beneficiary beneficiary, ApplicationUser user)
        {
            var form = Mapper.Map<Beneficiary, AddBeneficiaryForm>(beneficiary);

            form.Email = user.Email;

            return form;
        }
    }
}
