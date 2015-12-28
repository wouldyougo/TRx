using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Models;

namespace TRL.Connect.Smartcom.Test.Models
{
    [TestClass]
    public class PendingTradeInfoTests
    {
        [TestMethod]
        public void ExpectedUpdateOrder_constructor_test()
        {
            TradeInfo tradeInfo =
                new TradeInfo("ST12345-RF-01",
                    "RTS-9.14_FT",
                    "23409",
                    130000,
                    -1,
                    DateTime.Now,
                    "82398");

            PendingTradeInfo expectedUpdateOrder = 
                new PendingTradeInfo(tradeInfo);

            Assert.IsInstanceOfType(expectedUpdateOrder, typeof(TradeInfo));
            Assert.AreEqual(tradeInfo.Portfolio, expectedUpdateOrder.Portfolio);
            Assert.AreEqual(tradeInfo.Symbol, expectedUpdateOrder.Symbol);
            Assert.AreEqual(tradeInfo.Price, expectedUpdateOrder.Price);
            Assert.AreEqual(tradeInfo.Amount, expectedUpdateOrder.Amount);
            Assert.AreEqual(tradeInfo.DateTime, expectedUpdateOrder.DateTime);
            Assert.AreEqual(tradeInfo.OrderNo, expectedUpdateOrder.OrderNo);
            Assert.AreEqual(tradeInfo.TradeNo, expectedUpdateOrder.TradeNo);
        }

        [TestMethod]
        public void ExpectedUpdateOrder_ToString_test()
        {
            TradeInfo tradeInfo =
                new TradeInfo("ST12345-RF-01",
                    "RTS-9.14_FT",
                    "23409",
                    130000,
                    -1,
                    DateTime.Now,
                    "82398");

            PendingTradeInfo expectedUpdateOrder =
                new PendingTradeInfo(tradeInfo);

            string expected = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                expectedUpdateOrder.Portfolio,
                expectedUpdateOrder.Symbol,
                expectedUpdateOrder.OrderNo,
                expectedUpdateOrder.Price,
                expectedUpdateOrder.Amount,
                expectedUpdateOrder.DateTime,
                expectedUpdateOrder.TradeNo);
            Assert.AreEqual(expected, expectedUpdateOrder.ToString());
        }
    }
}
