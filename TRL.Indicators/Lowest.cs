using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Indicators
{
    public static class Lowest
    {
        public static IEnumerable<double> LowCollection(IEnumerable<Bar> src, int period)
        {
            return MakeCollection(src.OrderByDescending(o => o.DateTime).Select(i => i.Low).ToList(), period);
        }

        private static IEnumerable<double> MakeCollection(IEnumerable<double> src, int period)
        {
            List<double> result = new List<double>();

            for (int i = 0; i < src.Count(); i += period)
            {
                result.Add(src.Skip(i).Take(period).Min());

                if (result.Count < src.Count())
                {
                    for (int j = 0; j < period - 1; j++)
                    {
                        if (result.Count == src.Count())
                            break;
                        result.Add(result.ElementAt(i));
                    }
                }
            }

            result.Reverse();

            return result;
        }
    }
}
