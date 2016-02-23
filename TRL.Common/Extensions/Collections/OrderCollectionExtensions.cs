using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Extensions.Collections
{
    public static class OrderCollectionExtensions
    {
        public static IEnumerable<Order> GetUnfilled(this ICollection<Order> collection, string portfolio, string symbol)
        {
            try
            {
                return collection.Where(o => o.Portfolio == portfolio 
                    && o.Symbol == symbol 
                    && !o.IsCanceled
                    && !o.IsExpired
                    && !o.IsRejected
                    && o.FilledAmount < o.Amount);
            }
            catch
            {
                return null;
            }
        }

        public static double GetUnfilledSignedAmount(this ICollection<Order> collection, string portfolio, string symbol)
        {
            try
            {
                return collection.GetUnfilled(portfolio, symbol).Sum(o => o.UnfilledSignedAmount);
            }
            catch
            {
                return 0;
            }
        }

        public static IEnumerable<Order> GetUnfilled(this ICollection<Order> collection, StrategyHeader strategyHeader)
        {
            try
            {
                return collection.Where(o => o.Signal.StrategyId == strategyHeader.Id
                    && !o.IsCanceled
                    && !o.IsExpired
                    && !o.IsRejected
                    && !o.IsFilled);
            }
            catch
            {
                return null;
            }
        }

        public static double GetUnfilledSignedAmount(this ICollection<Order> collection, StrategyHeader strategyHeader)
        {
            try
            {
                return collection.GetUnfilled(strategyHeader).Sum(o => o.UnfilledSignedAmount);
            }
            catch
            {
                return 0;
            }
        }

        public static IEnumerable<Order> GetOrdersThatCanBeClearedWith(this ICollection<Order> collection, Order order)
        {
            try
            {
                return collection.Where(o => o.Portfolio == order.Portfolio
                    && o.Symbol == order.Symbol
                    && o.OrderType == order.OrderType
                    && o.Price == order.Price
                    && o.Stop == order.Stop
                    && !o.IsFilled
                    && !o.IsCanceled
                    && !o.IsExpired
                    && !o.IsRejected);
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<Order> GetUnfilledOrderJustLikeASignal(this ICollection<Order> collection, Signal signal)
        {
            try
            {
                return collection.Where(o => o.Signal.StrategyId == signal.StrategyId
                    && o.Portfolio == signal.Strategy.Portfolio
                    && o.Symbol == signal.Strategy.Symbol
                    && o.OrderType == signal.OrderType
                    && o.TradeAction == signal.TradeAction
                    && o.Price == signal.Limit
                    && o.Stop == signal.Stop
                    && !o.IsCanceled
                    && !o.IsExpired
                    && !o.IsFilled
                    && !o.IsRejected
                    && o.DateTime.AddSeconds(60) > BrokerDateTime.Make(DateTime.Now));
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<Order> GetUnfilled(this ICollection<Order> collection, string symbol)
        {
            try
            {
                //TODO здесь что-то очень медленно:
                //return collection.Where(o => o.Symbol == symbol
                //    && !o.IsCanceled
                //    && !o.IsExpired
                //    && !o.IsRejected
                //    && o.FilledAmount < o.Amount);
                return collection.Where(o => o.Symbol == symbol)
                                 .Where(o => o.IsCanceled == false)
                                 .Where(o => o.IsExpired  == false)
                                 .Where(o => o.IsRejected == false)
                                 .Where(o => o.FilledAmount < o.Amount);
            }
            catch
            {
                return null;
            }
        }

        public static Order GetOldestOrderWithPrice(this ICollection<Order> collection, double price)
        {
            try
            {
                return collection.First(o => o.Price == price);
            }
            catch
            {
                return null;
            }
        }
    }
}
