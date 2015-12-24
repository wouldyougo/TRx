using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Indicators
{
    public static class WealthLabKAMA
    {
        public static IEnumerable<double> Values(IEnumerable<double> src, int period)
        {
            int count = src.Count();

            double[] result = new double[count];

            if (count == 0 || count <= period)
                return result;

            double[] source = src.ToArray();

            double value = source[period];

            for (int i = period + 1; i < count; ++i)
            {
                double first_diff = Math.Abs(source[i] - source[i - period]);

                double second_diff = 0.0;

                for (int j = 0; j < period; ++j)
                    second_diff += Math.Abs(source[i - j] - source[i - j - 1]);

                double smthn_factor = (second_diff == 0.0 ? 0.0 : first_diff / second_diff) * (56.0 / 93.0) + 2.0 / 31.0;

                value += (smthn_factor * smthn_factor) * (source[i] - value);

                result[i] = value;
            }

            return result;
        }
    }
}
