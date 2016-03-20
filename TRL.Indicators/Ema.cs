using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Indicators
{
    [System.Obsolete("используйте TRx.Indicators.Indicator.EMA_i")]
    public class Ema
    {
        public static IEnumerable<double> Make(IEnumerable<double> source, int period)
        {
            if (source.Count() >= period)
            {
                List<double> result = new List<double>();

                result.Add(Math.Round(SMA.Make(source.Take(period)), 4));

                double k = 2.0 / (period + 1);

                for (int i = period; i < source.Count(); i++)
                    result.Add(Math.Round((source.ElementAt(i) * k + result.ElementAt(i-period) * (1 - k)), 4));

                return result;
            }

            return new double[0];
        }

    }
}
