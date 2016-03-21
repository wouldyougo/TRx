using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Indicators
{
    [System.Obsolete("используйте TRx.Indicators.Indicator.AverageTrueRange")]
    public class ATR
    {
        public static double Value(IEnumerable<Bar> bars)
        {
            int count = bars.Count();

            if (count == 0)
                return 0;

            double[] trs = new double[count];

            for (int i = 0; i < count; i++)
                trs[i] = TrueRange.Value(bars, i);

            return Math.Round(trs.Average(), 4);
        }

        public static IEnumerable<double> Values(IEnumerable<Bar> bars, int period)
        {
            int count = bars.Count();

            double[] result = new double[count];

            if (count < period)
                return result;

            int firstValue = period - 1;

            for (int i = 0; i < count - firstValue; i++)
            {
                result[firstValue + i] = ATR.Value(bars.Skip(i).Take(period));
            }

            return result;
        }

    }
}
