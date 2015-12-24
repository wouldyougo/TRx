using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Extensions.Data;
using TRL.Emulation;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataContextGetLastOpenOrderTradesTests:TraderBaseInitializer
    {
        private StrategyHeader strategy1, strategy2;

        [TestInitialize]
        public void Setup()
        {
            this.strategy1 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            this.strategy2 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 2);
        }

        [TestMethod]
        public void TradingDataContext_GetLastOpenOrderTrades_returns_null_when_no_any_trades_exists_test()
        {
            Assert.IsNull(this.tradingData.GetLastOpenOrderTrades(this.strategy1));
        }

        [TestMethod]
        public void TradingDataContext_GetLastOpenOrderTrades_returns_collection_test()
        {
            Signal signal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Assert.AreEqual(1, this.tradingData.GetLastOpenOrderTrades(this.strategy1).Count());
        }

        [TestMethod]
        public void TradingDataContext_GetLastOpenOrderTrades_ignore_other_strategies_orders_test()
        {
            Signal signal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Signal signal2 = new Signal(this.strategy2, DateTime.Now, TradeAction.Sell, OrderType.Limit, 32000, 0, 32000);
            EmulateTradeFor(signal2);

            Assert.AreEqual(1, this.tradingData.GetLastOpenOrderTrades(this.strategy1).Count());
            Assert.AreEqual(1, this.tradingData.GetLastOpenOrderTrades(this.strategy2).Count());
        }

        [TestMethod]
        public void TradingDataContext_GetLastOpenOrderTrades_ignore_close_position_trades_test()
        {
            Signal signal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Signal signalToClose = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 151000, 0, 0);
            EmulateTradeFor(signalToClose);

            Assert.AreEqual(1, this.tradingData.GetLastOpenOrderTrades(this.strategy1).Count());

            Trade trade = this.tradingData.GetLastOpenOrderTrades(this.strategy1).Last();
            Assert.AreEqual(signal.Id, trade.Order.SignalId);
        }

        [TestMethod]
        public void TradingDataContext_GetLastOpenOrderTrades_ignore_other_strategies_orders_and_close_orders_test()
        {
            Signal signal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Signal signal2 = new Signal(this.strategy2, DateTime.Now, TradeAction.Sell, OrderType.Limit, 32000, 0, 32000);
            EmulateTradeFor(signal2);

            Assert.AreEqual(1, this.tradingData.GetLastOpenOrderTrades(this.strategy1).Count());
            Assert.AreEqual(1, this.tradingData.GetLastOpenOrderTrades(this.strategy2).Count());

            Signal signalToClose = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 151000, 0, 0);
            EmulateTradeFor(signalToClose);

            Assert.AreEqual(1, this.tradingData.GetLastOpenOrderTrades(this.strategy1).Count());

            Trade trade = this.tradingData.GetLastOpenOrderTrades(this.strategy1).Last();
            Assert.AreEqual(signal.Id, trade.Order.SignalId);
        }

    }
}
