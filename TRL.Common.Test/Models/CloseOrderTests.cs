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
    public class CloseOrderTests
    {
        [TestMethod]
        public void CloseOrderConstructorTest()
        {
            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Sell, OrderType.Market, 10, 0, 0);

            CloseOrder closeOrder = new CloseOrder(order);

            Assert.AreEqual(order, closeOrder.Order);
            Assert.AreEqual(order.Id, closeOrder.OrderId);
            Assert.AreEqual(order.Id, closeOrder.Id);
        }
    }
}
