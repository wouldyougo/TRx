using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Extensions.Collections
{
    public static class TickCollectionExtension
    {
        public static IEnumerable<Tick> Last(this IEnumerable<Tick> collection, string symbol, TimeSpan ts)
        {
            try
            {
                DateTime firstTickDate = collection.Last(t => t.Symbol == symbol).DateTime.Add(-ts);

                return collection.Where(t => t.Symbol == symbol
                    && t.DateTime > firstTickDate);
            }
            catch
            {
                return null;
            }
        }

        public static double PriceSpan(this IEnumerable<Tick> collection)
        {
            if (collection == null)
                return 0;

            if (collection.Count() == 1)
                return 0;

            return collection.Max(t => t.Price) - collection.Min(t => t.Price);
        }

        public static IEnumerable<Tick> Last(this IEnumerable<Tick> collection, string symbol, int count)
        {
            int length = collection.Where(b => b.Symbol.Equals(symbol)).Count();

            int skip = length - count;

            if (skip > 0)
                return collection.Where(b => b.Symbol.Equals(symbol)).OrderBy(b => b.DateTime).Skip(skip).Take(count);
            else
                return collection.Where(b => b.Symbol.Equals(symbol)).OrderBy(b => b.DateTime).Take(count);

        }
    }
}
