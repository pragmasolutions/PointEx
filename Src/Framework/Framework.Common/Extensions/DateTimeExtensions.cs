using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common.Extentensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets the 12:00:00 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static DateTime? AbsoluteStart(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.AbsoluteStart() : (DateTime?) null;
        }

        /// <summary>
        /// Gets the 11:59:59 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteEnd(this DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }

        public static DateTime? AbsoluteEnd(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.AbsoluteEnd() : (DateTime?)null;
        }

        public static string ToString(this DateTime? dateTime, string format)
        {
            return dateTime.ToString(format, "");
        }

        public static string ToString(this DateTime? dateTime, string format, string returnIfNull)
        {
            if (dateTime.HasValue)
                return dateTime.Value.ToString(format);
            else
                return returnIfNull;
        }

        public static string ToShortDateString(this DateTime? dateTime)
        {
            return dateTime.ToShortDateString("");
        }

        public static string ToShortDateString(this DateTime? dateTime, string returnIfNull)
        {
            if (dateTime.HasValue)
                return dateTime.Value.ToShortDateString();
            else
                return returnIfNull;
        }
    }
}
