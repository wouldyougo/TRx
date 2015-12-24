using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ru.sazan.trader.Models;
using ru.sazan.trader.Data;
using ru.sazan.trader.Extensions;
using ru.sazan.trader.tests.Extensions;

namespace ru.sazan.trader.tests.Extensions
{
    [TestClass]
    public class TradingDataContextMakeSignalToClosePositionTests
    {
        private DataContext tradingData;
        private Strategy strategy;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();

            this.strategy = new Strategy(1, "Description", "BP12345-RF-01", "RTS-3.14_FT", 10);
            this.tradingData.Get<ICollection<Strategy>>().Add(this.strategy);
        }

        [TestMethod]
        public void MakeSignalToClosePosition_for_long_position_test()
        {
            Signal openSignal = new Signal(this.strategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal = this.tradingData.MakeSignalToClosePosition(this.strategy);
            Assert.AreEqual(this.strategy.Id, closeSignal.StrategyId);
            Assert.AreEqual(TradeAction.Sell, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, closeSignal.OrderType);
            Assert.AreEqual(0, closeSignal.Price);
            Assert.AreEqual(0, closeSignal.Limit);
            Assert.AreEqual(0, closeSignal.Stop);
            Assert.AreEqual(this.strategy.Amount, closeSignal.Amount);
        }

        [TestMethod]
        public void MakeSignalToClosePosition_for_short_position_test()
        {
            Signal openSignal = new Signal(this.strategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal = this.tradingData.MakeSignalToClosePosition(this.strategy);
            Assert.AreEqual(this.strategy.Id, closeSignal.StrategyId);
            Assert.AreEqual(TradeAction.Buy, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, closeSignal.OrderType);
            Assert.AreEqual(0, closeSignal.Price);
            Assert.AreEqual(0, closeSignal.Limit);
            Assert.AreEqual(0, closeSignal.Stop);
            Assert.AreEqual(this.strategy.Amount, closeSignal.Amount);
        }

        [TestMethod]
        public void MakeSignalToClosePosition_returns_null_when_no_position_exists_test()
        {
            Assert.IsNull(this.tradingData.MakeSignalToClosePosition(this.strategy));
        }
    }
}
