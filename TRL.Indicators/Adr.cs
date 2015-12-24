using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Indicators
{
    public static class Adr
    {
        public static double Make(IEnumerable<Bar> bars, int period)
        {
            int skip = 0;

            if (bars.Count() > period)
                skip = bars.Count() - period;

            int white = bars.OrderBy(b => b.DateTime).Skip(skip).Where(b => b.IsWhite).Count();
            int black = bars.OrderBy(b => b.DateTime).Skip(skip).Where(b => b.IsBlack).Count();

            if (black == 0)
                return (double)white;

            return (double)white / (double)black;
        }
    }
}
