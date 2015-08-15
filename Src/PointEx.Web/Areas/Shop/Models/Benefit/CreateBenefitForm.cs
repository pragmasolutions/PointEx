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
    public class CreateBenefitForm : BenefitForm
    {
        [UIHint("MultipleImageFiles")]
        [Display(Name = "Imagenes")]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}
