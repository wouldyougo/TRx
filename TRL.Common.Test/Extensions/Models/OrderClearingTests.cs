using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
//using TRL.Common.Extensions.Data;
using TRL.Common.Extensions.Models;

namespace TRL.Common.Extensions.Models.Test
{
    [TestClass]
    public class OrderClearingTests
    {
        [TestMethod]
        public void buy_and_sell_market_orders_can_be_cleared_with()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            Order o2 = new Order(s2, 300);

            Assert.IsTrue(o1.CanBeClearedWith(o2));
        }

        [TestMethod]
        public void buy_and_buy_market_orders_can_be_cleared_with()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            Order o2 = new Order(s2, 300);

            Assert.IsTrue(o1.CanBeClearedWith(o2));
        }

        [TestMethod]
        public void sell_and_sell_market_orders_can_be_cleared_with()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            Order o2 = new Order(s2, 300);

            Assert.IsTrue(o1.CanBeClearedWith(o2));
        }

        [TestMethod]
        public void filled_order_cant_be_cleared()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            o1.FilledAmount = 10;
            Assert.IsTrue(o1.IsFilled);

            Order o2 = new Order(s2, 300);

            Assert.IsFalse(o1.CanBeClearedWith(o2));
        }

        [TestMethod]
        public void cant_clear_with_filled_order()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            Order o2 = new Order(s2, 300);
            o2.FilledAmount = 10;
            Assert.IsTrue(o2.IsFilled);

            Assert.IsFalse(o1.CanBeClearedWith(o2));
        }

        [TestMethod]
        public void expired_order_cant_be_cleared()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            o1.ExpirationDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(-61);
            Assert.IsTrue(o1.IsExpired);

            Order o2 = new Order(s2, 300);

            Assert.IsFalse(o1.CanBeClearedWith(o2));
        }

        [TestMethod]
        public void cant_clear_with_expired_order()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            Order o2 = new Order(s2, 300);
            o2.ExpirationDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(-300);
            Assert.IsTrue(o2.IsExpired);

            Assert.IsFalse(o1.CanBeClearedWith(o2));
        }

        [TestMethod]
        public void rejected_order_cant_be_cleared()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            o1.Reject(BrokerDateTime.Make(DateTime.Now), "Reject");
            Assert.IsTrue(o1.IsRejected);

            Order o2 = new Order(s2, 300);

            Assert.IsFalse(o1.CanBeClearedWith(o2));
        }

        [TestMethod]
        public void cant_clear_with_rejected_order()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            Order o2 = new Order(s2, 300);
            o2.Reject(BrokerDateTime.Make(DateTime.Now), "Reject");
            Assert.IsTrue(o2.IsRejected);

            Assert.IsFalse(o1.CanBeClearedWith(o2));
        }

        [TestMethod]
        public void canceled_order_cant_be_cleared()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            o1.Cancel(BrokerDateTime.Make(DateTime.Now), "Cancel");
            Assert.IsTrue(o1.IsCanceled);

            Order o2 = new Order(s2, 300);

            Assert.IsFalse(o1.CanBeClearedWith(o2));
        }

        [TestMethod]
        public void cant_clear_with_canceled_order()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            Order o2 = new Order(s2, 300);
            o2.Cancel(BrokerDateTime.Make(DateTime.Now), "Cancel");
            Assert.IsTrue(o2.IsCanceled);

            Assert.IsFalse(o1.CanBeClearedWith(o2));
        }

        [TestMethod]
        public void partially_filled_order_can_be_cleared()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            o1.FilledAmount = 3;
            Assert.IsFalse(o1.IsFilled);
            Assert.IsTrue(o1.IsFilledPartially);

            Order o2 = new Order(s2, 300);

            Assert.IsTrue(o1.CanBeClearedWith(o2));
        }

        [TestMethod]
        public void can_clear_with_partially_filled_order()
        {
            StrategyHeader st1 = new StrategyHeader(1, "strategyHeader 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader st2 = new StrategyHeader(2, "strategyHeader 2", "BP12345-RF-01", "RTS-9.13_FT", 10);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 149900, 0, 0);

            Order o1 = new Order(s1, 60);
            Order o2 = new Order(s2, 300);
            o2.FilledAmount = 5;
            Assert.IsFalse(o2.IsFilled);
            Assert.IsTrue(o2.IsFilledPartially);

            Assert.IsTrue(o1.CanBeClearedWith(o2));
        }

    }
}
