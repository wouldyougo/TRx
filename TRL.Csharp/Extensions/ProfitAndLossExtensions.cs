using TRL.Csharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Extensions
{
    public static class ProfitAndLossExtensions
    {
        public static double GetProfitAndLossPoints(this IEnumerable<Trade> tradeList)
        {
            return tradeList.Sum(t => t.Sum) * -1;
        }
    }
}
