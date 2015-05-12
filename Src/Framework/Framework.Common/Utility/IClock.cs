using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common.Utility
{
    /// <summary>
    /// Abstracts the system clock and allows it to be used for cache dependencies
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Gets the current time
        /// </summary>
        DateTime Now { get; }
    }
}
