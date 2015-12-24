using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Connect.Smartcom.Events;
using TRL.Common.Collections;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Data
{
    public class MarketDataProvider
    {
        private ILogger logger;
        private SmartComHandlersDatabase handlers;
        private IDataContext tradingData;
        private OrderBookContext orderBook;

        public MarketDataProvider() : this(SmartComHandlers.Instance, TradingData.Instance, OrderBook.Instance, new NullLogger()) { }

        public MarketDataProvider(ILogger logger) : this(SmartComHandlers.Instance, TradingData.Instance, OrderBook.Instance, logger) { }

        public MarketDataProvider(SmartComHandlersDatabase handlers, IDataContext tradingData, OrderBookContext orderBook, ILogger logger)
        {
            this.handlers = handlers;
            this.tradingData = tradingData;
            this.logger = logger;
            this.orderBook = orderBook;
            this.handlers.Add<_IStClient_AddTickEventHandler>(stServer_AddTick);
            this.handlers.Add<_IStClient_UpdateBidAskEventHandler>(stServer_UpdateBidAsk);
            this.handlers.Add<_IStClient_AddBarEventHandler>(stServer_AddBar);
        }

        private void stServer_AddTick(string symbol, DateTime dateTime, double price, double volume, string tradeno, StOrder_Action action)
        {
            Tick Tick = new Tick { Symbol = symbol, DateTime = dateTime, Price = price, Volume = volume, TradeAction = MakeAction(action) };

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, получен Tick {2}, {3:dd/MM/yyyy H:mm:ss.fff}, {4}, {5}, {6}, {7}",
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name, 
                symbol, 
                dateTime, 
                price, 
                volume, 
                tradeno, 
                action));

            this.tradingData.Get<ObservableCollection<Tick>>().Add(Tick);
        }

        private TradeAction MakeAction(StOrder_Action action)
        {
            if (action.Equals(StOrder_Action.StOrder_Action_Buy) || action.Equals(StOrder_Action.StOrder_Action_Cover))
                return TradeAction.Buy;

            return TradeAction.Sell;

        }

        private void stServer_UpdateBidAsk(string symbol, int row, int nrows, double bid, double bidSize, double ask, double askSize)
        {
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, получен UpdateBidAsk, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name,
                symbol, 
                row, 
                nrows, 
                bid, 
                bidSize, 
                ask, 
                askSize));

            this.orderBook.Update(row, symbol, bid, bidSize, ask, askSize);
        }

        private BidAsk GetBidAsk(string symbol)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<BidAsk>>().Single(i => i.Symbol == symbol);
            }
            catch
            {
                return null;
            }
        }

        private void stServer_AddBar(int row, int nrows, string symbol, StBarInterval interval, DateTime datetime, double open, double high, double low, double close, double volume, double open_int)
        {
            Bar item = new Bar(symbol, BarIntervalFactory.Make(interval), datetime, open, high, low, close, volume);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, получен Bar {2}", 
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name, 
                item.ToString()));

            this.tradingData.Get<ObservableCollection<Bar>>().Add(item);
        }

    }
}
