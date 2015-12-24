using TRL.Csharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Extensions
{
    public static class PositionExtensions
    {
        public static double GetPositionAmount(this IEnumerable<Trade> tradeList)
        {
            return tradeList.Sum(t => t.Amount);
        }
    }
}
