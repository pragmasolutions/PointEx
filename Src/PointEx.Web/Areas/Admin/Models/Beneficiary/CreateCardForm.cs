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

namespace PointEx.Web.Models
{
    public class CreateCardForm : IMapFrom<Card>
    {
        [HiddenInput]
        public int BeneficiaryId { get; set; }

        [Display(Name = "Beneficiario")]
        public string BeneficiaryName { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Fecha de Vencimiento")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        public Card ToCard()
        {
            var card = Mapper.Map<CreateCardForm, Card>(this);
            return card;
        }
    }
}