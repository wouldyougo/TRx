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
    public class OrderRejectionTests
    {
        [TestMethod]
        public void OrderRejection_constructor_test()
        {
            Order order = new Order { Portfolio = "BP12345-RF-01", Symbol = "RTS-9.13_FT", TradeAction = TradeAction.Buy, OrderType = OrderType.Limit, Amount = 15 };
            DateTime rejectionDate = BrokerDateTime.Make(DateTime.Now);

            OrderRejection rejection = new OrderRejection(order, rejectionDate, "Отклонен биржей");

            Assert.IsTrue(rejection.Id > 0);
            Assert.AreEqual(order.Id, rejection.OrderId);
            Assert.AreEqual(rejectionDate, rejection.DateTime);
            Assert.AreEqual(order, rejection.Order);
            Assert.AreEqual("Отклонен биржей", rejection.Description);
        }
    }
}
