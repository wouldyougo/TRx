using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Indicators
{
    public class WilderMA
    {
        public static IEnumerable<double> Values(IEnumerable<double> collection, int period)
        {
            int count = collection.Count();

            double[] result = new double[count];

            if (count < period)
                return result;

            double[] source = collection.ToArray();

            int firstValue = period - 1;

            for (int i = firstValue; i >= 0; --i)
                result[firstValue] += source[i];

            result[firstValue] = result[firstValue] / (double)period;   // First value is just SMA

            for (int index = period; index < count; ++index)
                result[index] = (result[index - 1] * (double)(period - 1) + source[index]) / (double)period;

            return result;
        }
    }
}
