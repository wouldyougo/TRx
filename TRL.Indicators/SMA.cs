using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Indicators
{
    [System.Obsolete("используйте TRx.Indicators.Indicator.SMA")]
    public class SMA
    {
        public static IEnumerable<double> Close(IEnumerable<Bar> collection, int period)
        {
            return SMA.Make(collection.Select(i => i.Close).ToList(), period);
        }

        public static IEnumerable<double> Make(IEnumerable<double> collection, int period)
        {
            List<double> result = new List<double>();

            int limit = collection.Count() - period;

            for (int i = 0; i <= limit; i++)
                result.Add(collection.Skip(i).Take(period).Sum() / period);

            return result;
        }

        public static double Make(IEnumerable<double> source)
        {
            return source.Average();
        }
    }
}
