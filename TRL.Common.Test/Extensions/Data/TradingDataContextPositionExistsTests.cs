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
    public class TradingDataContextPositionExistsTests:TraderBaseInitializer
    {
        private StrategyHeader strategy1, strategy2;

        [TestInitialize]
        public void Setup()
        {
            this.strategy1 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            this.strategy2 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 2);
        }

        [TestMethod]
        public void TradingDataContext_PositionExists_returns_false_when_no_signals_and_orders_exists_test()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());

            Assert.IsFalse(this.tradingData.PositionExists(this.strategy1));
        }

        [TestMethod]
        public void TradingDataContext_PositionExists_returns_true_when_long_position_exists_test()
        {
            Signal signal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);

            EmulateTradeFor(signal);

            Assert.IsTrue(this.tradingData.PositionExists(this.strategy1));
        }

        [TestMethod]
        public void TradingDataContext_PositionExists_returns_true_when_short_position_exists_test()
        {
            Signal signal = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);

            EmulateTradeFor(signal);

            Assert.IsTrue(this.tradingData.PositionExists(this.strategy1));
        }

        [TestMethod]
        public void TradingDataContext_PositionExists_returns_false_when_long_position_closed_test()
        {
            Signal signal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Signal signalToClose = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 150100, 0, 0);
            EmulateTradeFor(signalToClose);

            Assert.IsFalse(this.tradingData.PositionExists(this.strategy1));
        }

        [TestMethod]
        public void TradingDataContext_PositionExists_returns_false_when_short_position_closed_test()
        {
            Signal signal = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Signal signalToClose = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 150100, 0, 0);
            EmulateTradeFor(signalToClose);

            Assert.IsFalse(this.tradingData.PositionExists(this.strategy1));
        }
    }
}
