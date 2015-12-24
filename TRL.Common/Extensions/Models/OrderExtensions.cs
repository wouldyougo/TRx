using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Extensions.Models
{
    public static class OrderExtensions
    {
        public static bool CanBeClearedWith(this Order order, Order right)
        {
            return order.Portfolio == right.Portfolio
                && order.Symbol == right.Symbol
                && order.OrderType == right.OrderType
                && order.Price == right.Price
                && order.Stop == right.Stop
                && !order.IsFilled
                && !right.IsFilled
                && !order.IsExpired
                && !right.IsExpired
                && !order.IsCanceled
                && !right.IsCanceled
                && !order.IsRejected
                && !right.IsRejected;
        }

        public static TradeAction InverseAction(this Order order)
        {
            return (order.TradeAction == TradeAction.Buy) ? TradeAction.Sell : TradeAction.Buy;
        }
    }
}
