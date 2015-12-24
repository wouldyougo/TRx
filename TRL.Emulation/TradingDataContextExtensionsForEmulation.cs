using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Collections;

namespace TRL.Emulation
{
    public static class TradingDataContextExtensionsForEmulation
    {
        public static Order AddSignalAndItsOrder(this IDataContext context, Signal signal)
        {
            if (!context.StrategyExists(signal.Strategy))
                return null;

            if (context.SignalExists(signal))
                return context.GetSignalOrder(signal);

            context.Get<ObservableHashSet<Signal>>().Add(signal);

            Order order = new Order(signal);
            order.DeliveryDate = order.DateTime.AddSeconds(1);

            context.Get<ObservableHashSet<Order>>().Add(order);

            return order;
        }

        public static bool SignalExists(this IDataContext context, Signal signal)
        {
            return context.Get<IEnumerable<Signal>>().Any(s => s.Id == signal.Id);
        }

        public static Order GetSignalOrder(this IDataContext context, Signal signal)
        {
            return context.Get<IEnumerable<Order>>().SingleOrDefault(o => o.SignalId == signal.Id);
        }

        public static bool StrategyExists(this IDataContext context, StrategyHeader strategyHeader)
        {
            return context.Get<IEnumerable<StrategyHeader>>().Any(s => s.Id == strategyHeader.Id);
        }

        public static Trade AddSignalAndItsOrderAndTrade(this IDataContext context, Signal signal)
        {
            return context.AddSignalAndItsOrderAndTrade(signal, signal.Price, signal.Amount);
        }

        public static Trade AddSignalAndItsOrderAndTrade(this IDataContext context, Signal signal, double price)
        {
            return context.AddSignalAndItsOrderAndTrade(signal, price, signal.Amount);
        }

        public static Trade AddSignalAndItsOrderAndTrade(this IDataContext context, Signal signal, double price, double amount)
        {
            Order order = context.AddSignalAndItsOrder(signal);

            if (order == null)
                return null;

            order.FilledAmount += amount;
            order.DeliveryDate = order.DateTime.AddSeconds(1);

            Trade trade = new Trade(order, order.Portfolio, order.Symbol, price, order.TradeAction == TradeAction.Buy ? amount : -amount, order.DateTime);

            context.Get<ObservableHashSet<Trade>>().Add(trade);

            return trade;
        }
    }
}
