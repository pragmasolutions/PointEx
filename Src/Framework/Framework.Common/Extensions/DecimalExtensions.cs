using System;
using System.Collections.Generic;

namespace Framework.Common.Extentensions
{
    public static class DecimalExtensions
    {
        public static string ToPercentageString(this decimal @decimal)
        {
            return (@decimal / 100).ToString("p2");
        }
    }
}
