using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
//using TRL.Common.Collections;

namespace TRL.Common.Extensions.Collections
{
    public static class BarCollectionExtensions
    {
        public static IEnumerable<Bar> GetNewestBars(this IEnumerable<Bar> collection, string symbol, int interval)
        {
            try
            {
                //int count = collection.Where(b => b.Symbol.Equals(symbol) && b.Interval == interval).Count();
                //return collection.Where(b => b.Symbol.Equals(symbol) && b.Interval == interval).OrderBy(b => b.DateTime).Take(count);
                return collection.Where(b => b.Symbol.Equals(symbol) && b.Interval == interval).OrderBy(b => b.DateTime);
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<Bar> GetNewestBars(this IEnumerable<Bar> collection, string symbol, int interval, int count)
        {
            try
            {
                int length = collection.Where(b => b.Symbol.Equals(symbol) && b.Interval == interval).Count();

                int skip = length - count;

                if(skip > 0)
                    return collection.Where(b => b.Symbol.Equals(symbol) && b.Interval == interval).OrderBy(b => b.DateTime).Skip(skip).Take(count);
                else
                    return collection.Where(b => b.Symbol.Equals(symbol) && b.Interval == interval).OrderBy(b => b.DateTime).Take(count);
            }
            catch
            {
                return null;
            }
        }

        public static Bar GetNewestBar(this IEnumerable<Bar> collection, string symbol, int interval)
        {
            try
            {
                return collection.Where(b => b.Symbol.Equals(symbol) && b.Interval == interval).OrderBy(b => b.DateTime).Last();
            }
            catch
            {
                return null;
            }
        }

        public static bool LastBarHasHighestHigh(this IEnumerable<Bar> collection)
        {
            try
            {
                int count = collection.Count();

                Bar last = collection.Last();

                return collection.Take(count - 1).Max(b => b.High) < last.High;
            }
            catch
            {
                return false;
            }
        }

        public static bool LastBarHasLowestLow(this IEnumerable<Bar> collection)
        {
            try
            {
                int count = collection.Count();

                Bar last = collection.Last();

                return collection.Take(count - 1).Min(b => b.Low) > last.Low;
            }
            catch
            {
                return false;
            }
        }
    }
}
