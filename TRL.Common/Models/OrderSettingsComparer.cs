using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class OrderSettingsComparer:EqualityComparer<OrderSettings>
    {
        public override bool Equals(OrderSettings x, OrderSettings y)
        {
            if (x.StrategyId == y.StrategyId)
                return true;

            return false;
        }

        public override int GetHashCode(OrderSettings obj)
        {
            return obj.StrategyId.GetHashCode();
        }
    }
}
