using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointEx.Data
{
    public partial class PointExDbContext
    {
        public PointExDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
    }
}
