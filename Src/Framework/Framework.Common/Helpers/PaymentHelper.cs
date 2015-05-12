using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common.Helpers
{
    public class PaymentHelper
    {
        public static IEnumerable<decimal> Divide(decimal amount, int numBuckets)
        {
            while (numBuckets > 0)
            {
                // determine the next amount to return...
                var partialAmount = Math.Round(amount / numBuckets, 2);
                yield return partialAmount;
                // reduce th remaining amount and #buckets
                // to account for previously yielded values
                amount -= partialAmount;
                numBuckets--;
            }
        }
    }
}
