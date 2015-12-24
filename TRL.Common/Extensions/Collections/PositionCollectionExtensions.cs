using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.Models;

namespace TRL.Common.Extensions.Collections
{
    public static class PositionCollectionExtensions
    {
        public static Position Get(this HashSet<Position> positions, int id)
        {
            try
            {
                return positions.Single(i => i.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public static double GetAmount(this ICollection<Position> positions, string symbol)
        {
            try
            {
                return positions.Where(p => p.Symbol == symbol).Sum(p => p.Amount);
            }
            catch
            {
                return 0;
            }
        }

        public static double GetAmount(this ICollection<Position> positions, string portfolio, string symbol)
        {
            try
            {
                return positions.Where(p => p.Portfolio == portfolio && p.Symbol == symbol).Sum(p => p.Amount);
            }
            catch
            {
                return 0;
            }
        }

        public static bool Exists(this ICollection<Position> collection, string portfolio, string symbol)
        {
            try
            {
                return collection.Any(p => p.Portfolio == portfolio && p.Symbol == symbol);
            }
            catch
            {
                return false;
            }
        }

    }
}
