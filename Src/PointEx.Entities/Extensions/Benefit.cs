using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointEx.Entities
{
    public partial class Benefit
    {
        public int? DefaultFileId
        {
            get
            {
                var defaultFile = this.BenefitFiles.OrderBy(bf => bf.Order).FirstOrDefault();
                if (defaultFile != null)
                {
                    return defaultFile.FileId;
                }
                return null;
            }
        }
    }
}
