using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointEx.Entities
{
    public partial class EducationalInstitution
    {
        public String FullName
        {
            get { return String.Format("[{0}] {1}", Town.Name, Name); }
        }
    }
}
