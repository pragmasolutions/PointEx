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
    public class BeneficiaryCardsModel   
    {
        public BeneficiaryCardsModel(Beneficiary beneficiary,IList<Card> cards)
        {
            Beneficiary = beneficiary;
            Cards = cards;
        }

        public Beneficiary Beneficiary { get; set; }
        public IList<Card> Cards { get; set; }    
    }
}
