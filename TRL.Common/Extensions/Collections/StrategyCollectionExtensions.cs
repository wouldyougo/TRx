using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Extensions.Collections
{
    public static class StrategyCollectionExtensions
    {
        public static double GetAmount(this ICollection<StrategyHeader> collection, string portfolio, string symbol)
        {
            try
            {
                return collection.Where(s => s.Portfolio == portfolio
                    && s.Symbol == symbol).Sum(ps => ps.Amount);
            }
            catch
            {
                return 0;
            }
        }
    }
}
