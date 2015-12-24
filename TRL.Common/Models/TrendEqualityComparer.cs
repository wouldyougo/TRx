using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class TrendEqualityComparer:EqualityComparer<Trend>
    {
        public override bool Equals(Trend x, Trend y)
        {
            if (x.Interval.Equals(y.Interval) && x.Symbol.Equals(y.Symbol))
                return true;

            return false;
        }

        public override int GetHashCode(Trend obj)
        {
            return obj.Interval.GetHashCode() ^ obj.Symbol.GetHashCode();
        }
    }
}
