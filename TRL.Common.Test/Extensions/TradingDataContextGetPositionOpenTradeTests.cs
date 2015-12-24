using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ru.sazan.trader.Extensions;
using ru.sazan.trader.tests.Extensions;
using ru.sazan.trader.Data;
using ru.sazan.trader.Models;

namespace ru.sazan.trader.tests.Extensions
{
    [TestClass]
    public class TradingDataContextGetPositionOpenTradeTests
    {
        private DataContext tradingData;
        private Strategy strategy;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();

            this.strategy = new Strategy(1, "Description", "BP12345-RF-01", "RTS-3.14_FT", 10);
            this.tradingData.Get<ICollection<Strategy>>().Add(this.strategy);

            AddAnotherStrategyTrades();
        }

        private void AddAnotherStrategyTrades()
        {
            Strategy strategy = new Strategy(2, "Description", "BP12345-RF-01", "RTS-3.14_FT", 10);
            this.tradingData.Get<ICollection<Strategy>>().Add(strategy);

            Signal openSignal = new Signal(strategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 140000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 140010, 7);
        }

        [TestMethod]
        public void GetPositionOpenTrade_returns_trade_for_long_position_test()
        {
            Signal openSignal = new Signal(this.strategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Trade trade = this.tradingData.GetPositionOpenTrade(this.strategy);

            Assert.AreEqual(trade.Order.SignalId, openSignal.Id);
            Assert.IsTrue(trade.Amount == openSignal.Amount);
        }

        [TestMethod]
        public void GetPositionOpenTrade_returns_last_trade_for_long_position_with_multiple_trades_test()
        {
            Signal openSignal = new Signal(this.strategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 140000, 5);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 140010, 5);

            Trade trade = this.tradingData.GetPositionOpenTrade(this.strategy);

            Assert.AreEqual(trade.Order.SignalId, openSignal.Id);
            Assert.IsTrue(trade.Amount == 5);
        }

        [TestMethod]
        public void GetPositionOpenTrade_returns_trade_when_long_position_was_partially_closed_test()
        {
            Signal openSignal = new Signal(this.strategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal = new Signal(this.strategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 139900, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal, 139900, 5);

            Trade trade = this.tradingData.GetPositionOpenTrade(this.strategy);

            Assert.AreEqual(trade.Order.SignalId, openSignal.Id);
            Assert.IsTrue(trade.Amount == openSignal.Amount);
        }

        [TestMethod]
        public void GetPositionOpenTrade_returns_trade_for_short_position_test()
        {
            Signal openSignal = new Signal(this.strategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Trade trade = this.tradingData.GetPositionOpenTrade(this.strategy);

            Assert.AreEqual(trade.Order.SignalId, openSignal.Id);
            Assert.IsTrue(trade.Amount == -openSignal.Amount);
        }

        [TestMethod]
        public void GetPositionOpenTrade_returns_last_trade_for_short_with_position_with_multiple_trades_test()
        {
            Signal openSignal = new Signal(this.strategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 140000, 8);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 140000, 2);

            Trade trade = this.tradingData.GetPositionOpenTrade(this.strategy);

            Assert.AreEqual(trade.Order.SignalId, openSignal.Id);
            Assert.IsTrue(trade.Amount == -2);
        }

        [TestMethod]
        public void GetPositionOpenTrade_returns_trade_when_short_position_was_partially_closed_test()
        {
            Signal openSignal = new Signal(this.strategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal = new Signal(this.strategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 141000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal, 141000, 7);

            Trade trade = this.tradingData.GetPositionOpenTrade(this.strategy);

            Assert.AreEqual(-3, this.tradingData.GetAmount(this.strategy));

            Assert.AreEqual(trade.Order.SignalId, openSignal.Id);
            Assert.IsTrue(trade.Amount == -openSignal.Amount);
        }

        [TestMethod]
        public void GetPositionOpenTrade_returns_null_when_no_position_exists_test()
        {
            Assert.IsNull(this.tradingData.GetPositionOpenTrade(this.strategy));
        }
    }
}
