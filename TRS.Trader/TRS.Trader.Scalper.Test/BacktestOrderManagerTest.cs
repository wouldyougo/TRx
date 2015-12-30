using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common;
using System.Collections.Generic;
using System.Linq;
using TRL.Logging;
using TRL.Common.Handlers;

namespace TRx.Trader.Scalper.Test
{
    [TestClass]
    public class BacktestOrderManagerTest
    {
        private StrategyHeader strategyHeader;
        private IDataContext tradingData;
        private IOrderManager orderManager;
        private Bar lastBar;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();

            this.strategyHeader = new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS-3.14_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.lastBar = new Bar("RTS-3.14_FT",
                60,
                new DateTime(2014, 1, 10, 11, 0, 0),
                144000,
                146000,
                143000,
                145000,
                100000);

            this.tradingData.Get<ICollection<Bar>>().Add(this.lastBar);

            this.orderManager = new BacktestOrderManager(this.tradingData, new NullLogger());

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
        }


        [TestMethod]
        public void BacktestOrderManager_executes_market_order_to_buy_test()
        {
            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Order order = new Order(signal);

            this.orderManager.PlaceOrder(order);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());

            Trade trade = this.tradingData.Get<IEnumerable<Trade>>().Last();
            Assert.AreEqual(order.Id, trade.OrderId);
            Assert.AreEqual(this.lastBar.Close, trade.Price);
            Assert.AreEqual(order.Amount, trade.Amount);
            Assert.AreEqual(order.Portfolio, trade.Portfolio);
            Assert.AreEqual(order.Symbol, trade.Symbol);
            Assert.AreEqual(this.lastBar.DateTime, trade.DateTime);

            Assert.AreEqual(this.lastBar.DateTime, signal.DateTime);
            Assert.AreEqual(this.lastBar.DateTime, order.DateTime);
        }

        [TestMethod]
        public void BacktestOrderManager_executes_market_order_to_sell_test()
        {
            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            Order order = new Order(signal);

            this.orderManager.PlaceOrder(order);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());

            Trade trade = this.tradingData.Get<IEnumerable<Trade>>().Last();
            Assert.AreEqual(order.Id, trade.OrderId);
            Assert.AreEqual(this.lastBar.Close, trade.Price);
            Assert.AreEqual(-order.Amount, trade.Amount);
            Assert.AreEqual(order.Portfolio, trade.Portfolio);
            Assert.AreEqual(order.Symbol, trade.Symbol);
            Assert.AreEqual(this.lastBar.DateTime, trade.DateTime);

            Assert.AreEqual(this.lastBar.DateTime, signal.DateTime);
            Assert.AreEqual(this.lastBar.DateTime, order.DateTime);
        }

        [TestMethod]
        public void BacktestOrderManager_ignore_order_if_here_is_no_bar_for_order_symbol_test()
        {
            StrategyHeader anotherStrategy =
                new StrategyHeader(2, "Description", "BP12345-RF-01", "Si-3.14_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(anotherStrategy);

            Signal signal = new Signal(anotherStrategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            Order order = new Order(signal);

            this.orderManager.PlaceOrder(order);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
        }
    }
}
