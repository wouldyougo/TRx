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
    public class OpenOrderTests
    {
        [TestMethod]
        public void OpenOrder_constructor_test()
        {
            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Market, 10, 0, 0);

            OpenOrder oo = new OpenOrder(order);

            Assert.AreEqual(order, oo.Order);
            Assert.AreEqual(order.Id, oo.OrderId);
            Assert.AreEqual(order.Id, oo.Id);
        }
    }
}
