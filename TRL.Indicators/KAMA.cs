using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Indicators
{
    public static class KAMA
    {
        public static IEnumerable<double> Make(IEnumerable<Bar> source, int period, int fastPeriod, int slowPeriod)
        {
            if (source.Count() >= period)
            {
                double[] result = new double[source.Count() - period];

                Bar[] bars = source.ToArray();

                double prevKama = bars[period - 1].Close;

                for (int i = period; i < source.Count(); i++)
                {
                    double signal = Math.Abs(bars[i].Close - bars[i - period].Close);
                    double noise = 0;

                    for (int j = period; j > 0; j--)
                        noise += Math.Abs(bars[j].Close - bars[j - 1].Close);

                    double er = signal / noise;
                    double fast = 2.0 / (fastPeriod + 1);
                    double slow = 2.0 / (slowPeriod + 1);
                    double squaredSmoothFactor = Math.Pow(er * (fast - slow) + slow, 2);

                    //prevKama = result[i - period] = squaredSmoothFactor * bars[i].Close + (1 - squaredSmoothFactor) * prevKama;
                    prevKama = result[i - period] = prevKama + squaredSmoothFactor * (bars[i].Close - prevKama);
                }

                return result;
            }

            return new double[0];
        }
    }
}
