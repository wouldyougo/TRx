using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.Models;
//using TRL.Common.Extensions.Models;
using TRL.Common.Extensions.Collections;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Extensions.Data
{
    public static class TradingDataContextExtensions
    {
        public static IEnumerable<Order> GetOpenOrders(this IDataContext context, StrategyHeader strategyHeader)
        {
            try
            {
                return from s in context.Get<IEnumerable<OpenOrder>>()
                            where s.Order.Signal.StrategyId == strategyHeader.Id 
                            select s.Order;
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<Order> GetFilledOpenOrders(this IDataContext context, StrategyHeader strategyHeader)
        {
            return context.GetOpenOrders(strategyHeader).Where(o => o.IsFilled || o.IsFilledPartially);
        }

        public static IEnumerable<Order> GetCloseOrders(this IDataContext context, StrategyHeader strategyHeader)
        {
            try
            {
                return from s in context.Get<IEnumerable<CloseOrder>>()
                       where s.Order.Signal.StrategyId == strategyHeader.Id
                       select s.Order;
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<Order> GetFilledCloseOrders(this IDataContext context, StrategyHeader strategyHeader)
        {
            return context.GetCloseOrders(strategyHeader).Where(o => o.IsFilled || o.IsFilledPartially);
        }

        public static IEnumerable<Trade> GetTrades(this IDataContext context, StrategyHeader strategyHeader)
        {
            try
            {
                return from s in context.Get<IEnumerable<StrategyHeader>>()
                       join signal in context.Get<IEnumerable<Signal>>()
                       on s equals signal.Strategy
                       join order in context.Get<IEnumerable<Order>>()
                       on signal equals order.Signal
                       join trade in context.Get<IEnumerable<Trade>>()
                       on order equals trade.Order
                       where s.Id == strategyHeader.Id
                       select trade;
            }
            catch
            {
                return null;
            }
        }

        public static double GetAmount(this IDataContext context, StrategyHeader strategyHeader)
        {
           return context.GetTrades(strategyHeader).Sum(t => t.Amount);
        }

        public static IEnumerable<Trade> GetTrades(this IDataContext context, Order order)
        {
            try
            {
                return from t in context.Get<IEnumerable<Trade>>()
                       where t.OrderId == order.Id
                       select t;
            }
            catch
            {
                return null;
            }
        }

        public static bool PositionExists(this IDataContext context, StrategyHeader strategyHeader)
        {
            return context.GetAmount(strategyHeader) != 0;
        }

        public static IEnumerable<Trade> GetBuyTrades(this IDataContext context, StrategyHeader strategyHeader)
        {
            return context.GetTrades(strategyHeader).Where(t => t.Amount > 0);
        }

        public static IEnumerable<Trade> GetSellTrades(this IDataContext context, StrategyHeader strategyHeader)
        {
            return context.GetTrades(strategyHeader).Where(t => t.Amount < 0);
        }

        /// <summary>
        /// Вычислить значение реализованной прибыли или убытка в пунктах для стратегии.
        /// </summary>
        /// <param name="context">Экземпляр контекста торговых данных.</param>
        /// <param name="strategyHeader">Экземпляр описания стратегии, для которой необходимо получить результат.</param>
        /// <returns>Возвращает значение реализованной прибыли или убытка только для закрытых позиций, без учета комиссии.</returns>
        public static double GetProfitAndLossPoints(this IDataContext context, StrategyHeader strategyHeader)
        {
            IEnumerable<Trade> buyTrades = context.GetBuyTrades(strategyHeader);
            IEnumerable<Trade> sellTrades = context.GetSellTrades(strategyHeader);

            double buyAmount = Math.Abs(buyTrades.Sum(t => t.Amount));
            double sellAmount = Math.Abs(sellTrades.Sum(t => t.Amount));

            if (buyAmount > sellAmount)
                return GetPointsSum(sellTrades) - GetPointsSum(buyTrades.TakeForAmount(sellAmount));
            
            if (sellAmount > buyAmount)
                return GetPointsSum(sellTrades.TakeForAmount(buyAmount)) - GetPointsSum(buyTrades);

            return GetPointsSum(sellTrades) - GetPointsSum(buyTrades);
        }

        private static double GetPointsSum(IEnumerable<Trade> trades)
        {
            return trades.Sum(t => (Math.Abs(t.Amount) * t.Price));
        }

        public static double GetProfitAndLoss(this IDataContext context, StrategyHeader strategyHeader)
        {
            Symbol symbol = GetSymbol(context, strategyHeader);

            if (symbol == null)
                return GetProfitAndLossPoints(context, strategyHeader);

            return GetProfitAndLossPoints(context, strategyHeader) / symbol.Step * symbol.StepPrice;
        }

        public static Symbol GetSymbol(this IDataContext context, StrategyHeader strategyHeader)
        {
            try
            {
                return context.Get<IEnumerable<Symbol>>().Single(s => s.Name == strategyHeader.Symbol);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Получить список неисполненных заявок
        /// </summary>
        /// <param name="context"></param>
        /// <param name="strategyHeader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Order> GetUnfilled(this IDataContext context, StrategyHeader strategyHeader, OrderType type)
        {
            try
            {
                return context.Get<IEnumerable<Order>>().Where(o => o.Signal.StrategyId == strategyHeader.Id
                    && o.OrderType == type
                    && !o.IsCanceled
                    && !o.IsExpired
                    && !o.IsFilled
                    && !o.IsRejected);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Получить список неисполненных заявок
        /// </summary>
        /// <param name="context"></param>
        /// <param name="strategyHeader"></param>
        /// <returns></returns>
        public static IEnumerable<Order> GetUnfilled(this IDataContext context, StrategyHeader strategyHeader)
        {
            try
            {
                return context.Get<IEnumerable<Order>>().Where(o => o.Signal.StrategyId == strategyHeader.Id
                    && !o.IsCanceled
                    && !o.IsExpired
                    && !o.IsFilled
                    && !o.IsRejected);
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<Order> GetUndelivered(this IDataContext context, StrategyHeader strategyHeader, OrderType type)
        {
            try
            {
                return context.Get<IEnumerable<Order>>().Where(o => o.Signal.StrategyId == strategyHeader.Id
                    && o.OrderType == type
                    && !o.IsDelivered
                    && !o.IsExpired);
            }
            catch
            {
                return null;
            }
        }

        public static bool UndeliveredExists(this IDataContext context, StrategyHeader strategyHeader, OrderType type)
        {
            return GetUndelivered(context, strategyHeader, type).Count() > 0;
        }

        public static bool UnfilledExists(this IDataContext context, StrategyHeader strategyHeader, OrderType type)
        {
            return GetUnfilled(context, strategyHeader, type).Count() > 0;
        }

        public static bool UnfilledExists(this IDataContext context, StrategyHeader strategyHeader)
        {
            return GetUnfilled(context, strategyHeader).Count() > 0;
        }

        public static StopPointsSettings GetStopPointsSettings(this IDataContext context, StrategyHeader strategyHeader)
        {
            try
            {
                return context.Get<IEnumerable<StopPointsSettings>>().Single(s => s.StrategyId == strategyHeader.Id);
            }
            catch
            {
                return null;
            }
        }

        public static ProfitPointsSettings GetProfitPointsSettings(this IDataContext context, StrategyHeader strategyHeader)
        {
            try
            {
                return context.Get<IEnumerable<ProfitPointsSettings>>().Single(s => s.StrategyId == strategyHeader.Id);
            }
            catch
            {
                return null;
            }
        }

        public static Symbol GetSymbol(this IDataContext context, string symbol)
        {
            try
            {
                return context.Get<IEnumerable<Symbol>>().Single(s => s.Name == symbol);
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<Trade> GetLastOpenOrderTrades(this IDataContext context, StrategyHeader strategyHeader)
        {
            try
            {
                Order lastOpenOrder = context.GetFilledOpenOrders(strategyHeader).Last();

                if (lastOpenOrder == null)
                    return null;

                return context.GetTrades(lastOpenOrder);
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<OrderMoveRequest> GetMoveRequests(this IDataContext context, Order order)
        {
            try
            {
                return context.Get<IEnumerable<OrderMoveRequest>>().Where(r => r.OrderId == order.Id);
            }
            catch
            {
                return null;
            }
        }

        public static bool IsOpening(this IDataContext context, Trade trade)
        {
            try
            {
                return context.Get<IEnumerable<OpenOrder>>().Any(o => o.OrderId == trade.OrderId);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsClosing(this IDataContext context, Trade trade)
        {
            try
            {
                return context.Get<IEnumerable<CloseOrder>>().Any(o => o.OrderId == trade.OrderId);
            }
            catch
            {
                return false;
            }
        }

        public static double GetCurrentProfitAndLoss(this IDataContext context, StrategyHeader strategyHeader)
        {
            double result = 0;
            double amount = context.GetAmount(strategyHeader);

            if (amount == 0)
                return result;

            TradeAction expectedAction = amount > 0 ? TradeAction.Buy : TradeAction.Sell;

            Trade[] trades = context.GetTrades(strategyHeader).ToArray();

            Tick lastTick = context.Get<IEnumerable<Tick>>().LastOrDefault(t => t.Symbol == strategyHeader.Symbol);

            if(lastTick == null)
                return 0;

            double currentPrice = lastTick.Price;

            for (int i = trades.Length - 1; i >= 0; i--)
            {
                if (trades[i].Action != expectedAction)
                    continue;

                amount -= trades[i].Amount;

                if (amount >= 0)
                {
                    result += trades[i].Amount * (currentPrice - trades[i].Price);
                }
                else 
                {
                    result += (trades[i].Amount + amount) * (currentPrice - trades[i].Price);

                    break;
                }
            }

            return result;
        }

        public static bool HasLongPosition(this IDataContext context, StrategyHeader strategyHeader)
        {
            return context.GetAmount(strategyHeader) > 0;
        }

        public static bool HasShortPosition(this IDataContext context, StrategyHeader strategyHeader)
        {
            return context.GetAmount(strategyHeader) < 0;
        }

        public static IEnumerable<Order> GetFilledPartially(this IDataContext context, StrategyHeader strategyHeader, OrderType type)
        {
            try
            {
                return context.Get<IEnumerable<Order>>().Where(o => o.Signal.StrategyId == strategyHeader.Id
                    && o.OrderType == type
                    && o.IsFilledPartially
                    && !o.IsCanceled
                    && !o.IsExpired
                    && !o.IsFilled
                    && !o.IsRejected);
            }
            catch
            {
                return null;
            }
        }

        public static Signal MakeSignalToClosePosition(this IDataContext context, StrategyHeader strategyHeader)
        {
            if (!context.PositionExists(strategyHeader))
                return null;

            if (context.HasLongPosition(strategyHeader))
                return new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 0, 0, 0);

            return new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 0, 0, 0);
        }

        public static Trade GetPositionOpenTrade(this IDataContext context, StrategyHeader strategyHeader)
        {
            if (context.HasLongPosition(strategyHeader))
                return context.Get<IEnumerable<Trade>>().LastOrDefault(s => s.Order.Signal.StrategyId == strategyHeader.Id
                    && s.Amount > 0);

            return context.Get<IEnumerable<Trade>>().LastOrDefault(s => s.Order.Signal.StrategyId == strategyHeader.Id
                && s.Amount < 0);
        }

        public static ICollection<Trade> GetPositionTrades(this IDataContext context, StrategyHeader strategyHeader)
        {
            IGenericFactory<ICollection<Trade>> factory =
                new PositionTradesFactory(context, strategyHeader);

            return factory.Make();
        }

        public static double GetPositionPoints(this IDataContext context, StrategyHeader strategyHeader)
        {
            IGenericFactory<ICollection<Trade>> factory =
                new PositionTradesFactory(context, strategyHeader);

            IEnumerable<Trade> trades = factory.Make();

            return trades.Sum(t => t.Amount * t.Price);
        }
    }
}
