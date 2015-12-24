using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TRL.Common.Models
{
    public class PositionEqualityComparer:EqualityComparer<Position>
    {
        public override bool Equals(Position x, Position y)
        {
            if (x.Portfolio.Equals(y.Portfolio) && x.Symbol.Equals(y.Symbol))
                return true;

            return false;
        }

        public override int GetHashCode(Position obj)
        {
            return obj.Portfolio.GetHashCode() ^ obj.Symbol.GetHashCode();
        }
    }
}
