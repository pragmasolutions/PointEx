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
    public class EducationalInstitutionForm : IMapFrom<EducationalInstitution>
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [Remote("IsNameAvailable", "EducationalInstitution", "Admin", ErrorMessage = "Ya existe un establecimiento con este nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [UIHint("TownId")]
        [Display(Name = "Localidad")]
        [Required]
        public int TownId { get; set; }

        [Display(Name = "Ubicación")]
        public DbGeography Location { get; set; }

        public EducationalInstitution ToEducationalInstitution()
        {
            var shop = Mapper.Map<EducationalInstitutionForm, EducationalInstitution>(this);
            return shop;
        }

        public static EducationalInstitutionForm FromEducationalInstitution(EducationalInstitution shop)
        {
            var form = Mapper.Map<EducationalInstitution, EducationalInstitutionForm>(shop);
            return form;
        }
    }
}
