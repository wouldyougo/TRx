using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class TradeTests
    {
        [TestMethod]
        public void Trade_constructor_test()
        {
            DateTime tradeDate = BrokerDateTime.Make(DateTime.Now);
            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Trade trade = new Trade(order, order.Portfolio, order.Symbol, 150000, order.Amount, tradeDate);

            Assert.IsTrue(trade.Id > 0);
            Assert.AreEqual(order, trade.Order);
            Assert.AreEqual(order.Id, trade.OrderId);
            Assert.AreEqual(150000, trade.Price);
            Assert.AreEqual(order.Amount, trade.Amount);
            Assert.AreEqual(tradeDate, trade.DateTime);
        }

        [TestMethod]
        public void Trade_Parse_test()
        {
            string src = "10, 01/01/2013 10:00:00, BP12345-RF-01, SBRF-9.13_FT, 10000.0000, 8.0000, 35";

            Trade trade = Trade.Parse(src);

            Assert.AreEqual(10, trade.Id);
            Assert.AreEqual(35, trade.OrderId);
            Assert.AreEqual(10000, trade.Price);
            Assert.AreEqual(8, trade.Amount);
            Assert.AreEqual(new DateTime(2013, 1, 1, 10, 0, 0), trade.DateTime);
            Assert.AreEqual("BP12345-RF-01", trade.Portfolio);
            Assert.AreEqual("SBRF-9.13_FT", trade.Symbol);

        }

        [TestMethod]
        public void Trade_Buy_test()
        {
            Trade trade = new Trade { Portfolio = "BP12345-RF-01", Symbol = "RTS-12.13_FT", Amount = 1, Price = 150000 };

            Assert.IsTrue(trade.Buy);
        }

        [TestMethod]
        public void Trade_Sell_test()
        {
            Trade trade = new Trade { Portfolio = "BP12345-RF-01", Symbol = "RTS-12.13_FT", Amount = -1, Price = 150000 };

            Assert.IsTrue(trade.Sell);
        }

        [TestMethod]
        public void Sell_Trade_Sum_test()
        {
            Trade trade = new Trade { Portfolio = "BP12345-RF-01", Symbol = "RTS-12.13_FT", Amount = -3, Price = 150000 };

            Assert.AreEqual(450000, trade.Sum);
        }

        [TestMethod]
        public void Buy_Trade_Sum_test()
        {
            Trade trade = new Trade { Portfolio = "BP12345-RF-01", Symbol = "Si-12.13_FT", Amount = 5, Price = 32000 };

            Assert.AreEqual(160000, trade.Sum);
        }

        [TestMethod]
        public void Trade_copy_constructor()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-3.14_FT", 10);
            Signal signal = new Signal(strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 138000, 0, 0);
            Order order = new Order(signal);
            Trade trade = new Trade(order, order.Portfolio, order.Symbol, order.Price, order.Amount, DateTime.Now);

            Trade clone = new Trade(trade);

            Assert.AreNotSame(trade, clone);
            Assert.AreEqual(trade.Id, clone.Id);
            Assert.AreEqual(trade.Portfolio, clone.Portfolio);
            Assert.AreEqual(trade.Symbol, clone.Symbol);
            Assert.AreEqual(trade.Price, clone.Price);
            Assert.AreEqual(trade.Amount, clone.Amount);
            Assert.AreEqual(trade.DateTime, clone.DateTime);
            Assert.AreEqual(trade.Order, clone.Order);
            Assert.AreNotSame(trade.OrderId, clone.OrderId);
        }

        [TestMethod]
        public void Trade_Sign_returns_negated_one_test()
        {
            Trade trade = new Trade { Portfolio = "BP12345-RF-01", Symbol = "Si-12.13_FT", Amount = -5, Price = 32000 };

            Assert.AreEqual(-1, trade.Sign);
        }

        [TestMethod]
        public void Trade_Sign_returns_one_test()
        {
            Trade trade = new Trade { Portfolio = "BP12345-RF-01", Symbol = "Si-12.13_FT", Amount = 5, Price = 32000 };

            Assert.AreEqual(1, trade.Sign);
        }

        [TestMethod]
        public void Trade_Action_returns_Sell_test()
        {
            Trade trade = new Trade { Portfolio = "BP12345-RF-01", Symbol = "Si-12.13_FT", Amount = -5, Price = 32000 };

            Assert.AreEqual(TradeAction.Sell, trade.Action);
        }

        [TestMethod]
        public void Trade_Action_returns_Buy_test()
        {
            Trade trade = new Trade { Portfolio = "BP12345-RF-01", Symbol = "Si-12.13_FT", Amount = 5, Price = 32000 };

            Assert.AreEqual(TradeAction.Buy, trade.Action);
        }

        [TestMethod]
        public void Trade_AbsoluteAmount_returns_amount_without_sign_test()
        {
            Trade trade = new Trade { Portfolio = "BP12345-RF-01", Symbol = "Si-12.13_FT", Amount = -5, Price = 32000 };

            Assert.AreEqual(5, trade.AbsoluteAmount);
        }
    }
}
