﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Mapping;
using Framework.Common.Web.Metadata;
using Newtonsoft.Json;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class SectionItemsModel
    {
        public Section Section { get; set; }
        public IList<SectionItem> SectionItems { get; set; }
    }
}
