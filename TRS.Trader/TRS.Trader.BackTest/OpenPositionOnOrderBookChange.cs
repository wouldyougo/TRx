using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Models;
using TRL.Common.Extensions.Models;
using TRL.Common.TimeHelpers;
//using TRL.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Logging;

namespace TRx.Trader.BackTest
{
    public class OpenPositionOnOrderBookChange
    {
        /// <summary>
        /// Стратегия
        /// </summary>
        private StrategyHeader strategyHeader;
        /// <summary>
        /// Контекст очереди заявок
        /// </summary>
        private OrderBookContext orderBook;
        /// <summary>
        /// Обозреваемая очередь сигналов
        /// </summary>
        private ObservableQueue<Signal> signalQueue;
        /// <summary>
        /// Контекст торговых данных
        /// </summary>
        private TradingDataContext tradingData;
        /// <summary>
        /// Логгер
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strategyHeader">Стратегия</param>
        /// <param name="orderBook"></param>
        /// <param name="signalQueue">Обозреваемая очередь сигналов</param>
        /// <param name="tradingData">Торговых данных контекст</param>
        /// <param name="logger">Логгер</param>
        public OpenPositionOnOrderBookChange(StrategyHeader strategyHeader,
            OrderBookContext orderBook,
            ObservableQueue<Signal> signalQueue,
            TradingDataContext tradingData,
            ILogger logger) 
        {
            this.strategyHeader = strategyHeader;
            this.orderBook = orderBook;
            this.signalQueue = signalQueue;
            this.tradingData = tradingData;
            this.logger = logger;

            this.orderBook.OnQuotesUpdate += new SymbolDataUpdatedNotification(OnChange);
        }

        private void OnChange(string symbol)
        {
            if (this.strategyHeader.Symbol != symbol)
                return;

            if (StrategyPositionExists())
                return;

            if (UnfilledOrdersExists())
                return;

            Order lastOrder = GetLastFilledOrder();

            TradeAction action = GetAction(lastOrder);

            double limitPrice = GetLimitPrice(action);

            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), action, OrderType.Limit, limitPrice, 0, limitPrice);

            this.logger.Log(
                String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сгенерирован сигнал {2}", 
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name, 
                signal.ToString()));

            this.signalQueue.Enqueue(signal);
        }

        private Order GetLastFilledOrder()
        {
            try
            {
                return this.tradingData.GetFilledCloseOrders(this.strategyHeader).Last();
            }
            catch
            {
                return null;
            }
        }

        private TradeAction GetAction(Order order)
        {
            if (order == null)
                return TradeAction.Buy;

            if (order.OrderType == OrderType.Stop)
                return order.TradeAction;

            return order.InverseAction();
        }

        private double GetLimitPrice(TradeAction action)
        {
            if (action == TradeAction.Buy)
                return this.orderBook.GetBidPrice(this.strategyHeader.Symbol, 0);

            return this.orderBook.GetOfferPrice(this.strategyHeader.Symbol, 0);
        }

        private bool StrategyPositionExists()
        {
            if (this.tradingData.GetAmount(this.strategyHeader) != 0)
                return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool UnfilledOrdersExists()
        {
            if (this.tradingData.GetUnfilled(this.strategyHeader, OrderType.Limit).Count() != 0)
                return true;

            return false;
        }

    }
}
