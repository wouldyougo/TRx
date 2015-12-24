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
    public class OrderMoveRequestTests
    {
        [TestMethod]
        public void OrderMoveRequest_constructor_test()
        {
            DateTime date = BrokerDateTime.Make(DateTime.Now).Date;

            double stopPrice = 149900;

            double limitPrice = 0;

            string description = "Move order";

            Order order = new Order(1, DateTime.Now, "BP12345-RF-01", "RTS-12.13_FT", TradeAction.Sell, OrderType.Stop, 1, 0, 150000);

            OrderMoveRequest request = new OrderMoveRequest(order, limitPrice, stopPrice, description);

            Assert.IsTrue(request.Id > 0);
            Assert.AreEqual(order, request.Order);
            Assert.AreEqual(order.Id, request.OrderId);
            Assert.AreEqual(limitPrice, request.LimitPrice);
            Assert.AreEqual(stopPrice, request.StopPrice);
            Assert.AreEqual(description, request.Description);
            Assert.AreEqual(date, request.DateTime.Date);
            Assert.AreEqual(DateTime.MinValue, request.DeliveryDate);
            Assert.AreEqual(DateTime.MinValue, request.FaultDate);
            Assert.AreEqual(request.DateTime.AddSeconds(60), request.ExpirationDate);
            Assert.AreEqual(string.Empty, request.FaultDescription);
            Assert.IsFalse(request.IsExpired);
            Assert.IsFalse(request.IsDelivered);
            Assert.IsFalse(request.IsFailed);
        }

        [TestMethod]
        public void OrderMoveRequest_Failed_test()
        {
            DateTime date = BrokerDateTime.Make(DateTime.Now).Date;

            double stopPrice = 149900;

            double limitPrice = 0;

            string description = "Move order";

            Order order = new Order(1, DateTime.Now, "BP12345-RF-01", "RTS-12.13_FT", TradeAction.Sell, OrderType.Stop, 1, 0, 150000);

            OrderMoveRequest request = new OrderMoveRequest(order, limitPrice, stopPrice, description);
            Assert.IsFalse(request.IsFailed);

            DateTime faultDate = BrokerDateTime.Make(DateTime.Now);
            string faultReason = "Fault";

            request.Failed(faultDate, faultReason);

            Assert.AreEqual(faultDate, request.FaultDate);
            Assert.AreEqual(faultReason, request.FaultDescription);
            Assert.IsTrue(request.IsFailed);
        }
    }
}
