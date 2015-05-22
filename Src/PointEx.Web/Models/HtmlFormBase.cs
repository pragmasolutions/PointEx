using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PointEx.Web.Models
{
    public class HtmlFormBase
    {
        [HiddenInput]
        public bool IsEdit { get; set; }
    }
}