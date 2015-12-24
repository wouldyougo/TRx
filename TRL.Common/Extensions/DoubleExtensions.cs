using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Extensions
{
    public static class DoubleExtensions
    {
        public static double RoundUp(this double value, double step)
        {
            double quotient = Math.Ceiling(value / step);

            return quotient * step;
        }

        public static double RoundDown(this double value, double step)
        {
            double quotient = Math.Floor(value / step);

            return quotient * step;
        }

        public static bool HasOppositeSignWith(this double left, double right)
        {
            if (left > 0 && right < 0)
                return true;

            if (left < 0 && right > 0)
                return true;

            return false;
        }
    }
}
