using System;
using System.Collections.Generic;
using System.Linq;

namespace RosemountDiagnosticsV2.Extension_Methods
{
    public static class StdDeviation
    {
        public static decimal StandardDeviation(this IEnumerable<decimal> values)
        {
            if (values.Count() == 0)
            {
                return -1;
            }
            double avg = Convert.ToDouble(values.Average());
            return Decimal.Round(Convert.ToDecimal(Math.Sqrt(values.Average(v => Math.Pow(Convert.ToDouble(v) - avg, 2)))), 9);
        }
    }
}
