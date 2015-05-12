using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointEx.Data.Helpers
{
    public class SortHelper
    {
        public static string SortExpression(string sortBy, string sortDirection)
        {
            return string.Format("{0} {1}", sortBy.ToUpper(), sortDirection.ToUpper());
        }
    }
}
