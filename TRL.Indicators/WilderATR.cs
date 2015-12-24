using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Indicators
{
    public class WilderATR
    {
        public static IEnumerable<double> Values(IEnumerable<Bar> collection, int period)
        {
            int count = collection.Count();

            double[] result = new double[count];

            if (count < period)
                return result;

            IEnumerable<double> trueRanges = TrueRange.Values(collection);

            double[] wilderMA = WilderMA.Values(trueRanges, period).ToArray();

            for (int i = 0; i < count; i++)
                result[i] = wilderMA[i];

            return result;
        }
    }
}
