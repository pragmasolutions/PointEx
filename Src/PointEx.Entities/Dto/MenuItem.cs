using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointEx.Entities.Dto
{
    public class MenuItem
    {
        public string Text { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public List<MenuItem> Children { get; set; }
    }
}
