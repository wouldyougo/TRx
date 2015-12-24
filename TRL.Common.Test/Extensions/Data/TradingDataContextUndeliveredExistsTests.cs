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
    public class TradingDataContextUndeliveredExistsTests : TraderBaseInitializer
    {
        private StrategyHeader strategy1, strategy2;

        [TestInitialize]
        public void Setup()
        {
            this.strategy1 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            this.strategy2 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 2);

            this.signalQueue.Enqueue(new Signal(this.strategy2, DateTime.Now, TradeAction.Buy, OrderType.Market, 32000, 0, 0));
            this.signalQueue.Enqueue(new Signal(this.strategy2, DateTime.Now, TradeAction.Buy, OrderType.Limit, 32000, 0, 32000));
            this.signalQueue.Enqueue(new Signal(this.strategy2, DateTime.Now, TradeAction.Buy, OrderType.Stop, 32000, 32000, 0));
        }

        [TestMethod]
        public void TradingDataContext_UndeliveredExists_returns_false_test()
        {
            Assert.IsFalse(this.tradingData.UndeliveredExists(this.strategy1, OrderType.Market));
            Assert.IsFalse(this.tradingData.UndeliveredExists(this.strategy1, OrderType.Stop));
            Assert.IsFalse(this.tradingData.UndeliveredExists(this.strategy1, OrderType.Limit));
        }

        [TestMethod]
        public void TradingDataContext_UndeliveredExists_returns_true_test()
        {
            Signal signal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.signalQueue.Enqueue(signal);

            Assert.IsTrue(this.tradingData.UndeliveredExists(this.strategy1, signal.OrderType));
        }

        [TestMethod]
        public void TradingDataContext_UndeliveredExists_returns_true_for_stop_order_test()
        {
            Signal signal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Stop, 150000, 150000, 0);
            this.signalQueue.Enqueue(signal);

            Assert.IsTrue(this.tradingData.UndeliveredExists(this.strategy1, signal.OrderType));
        }

        [TestMethod]
        public void TradingDataContext_UndeliveredExists_returns_true_for_limit_order_test()
        {
            Signal signal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.signalQueue.Enqueue(signal);

            Assert.IsTrue(this.tradingData.UndeliveredExists(this.strategy1, signal.OrderType));
        }

        [TestMethod]
        public void TradingDataContext_UndeliveredExists_returns_false_when_order_is_filled_test()
        {
            Signal signal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            EmulateTradeFor(signal);

            Assert.IsFalse(this.tradingData.UndeliveredExists(this.strategy1, signal.OrderType));
        }
    }
}
