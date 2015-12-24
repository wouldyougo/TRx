using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using System.Globalization;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class OrderDeliveryConfirmationTests
    {
        [TestMethod]
        public void OrderDeliveryConfirmation_constructor_test()
        {
            Order order = 
                new Order(1, 
                    BrokerDateTime.Make(DateTime.Now), 
                    "BP12345-RF-01", 
                    "RTS-9.13_FT", 
                    TradeAction.Buy, 
                    OrderType.Limit, 
                    10, 
                    136000, 
                    0);

            DateTime deliveryDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(1);
            OrderDeliveryConfirmation orderDeliveryConfirmation = 
                new OrderDeliveryConfirmation(order, deliveryDate);

            Assert.AreEqual(order, orderDeliveryConfirmation.Order);
            Assert.AreEqual(order.Id, orderDeliveryConfirmation.OrderId);
            Assert.AreEqual(deliveryDate, orderDeliveryConfirmation.DateTime);
        }

        [TestMethod]
        public void OrderDeliveryConfirmation_ToString_test()
        {
            Order order =
                new Order(1,
                    BrokerDateTime.Make(DateTime.Now),
                    "BP12345-RF-01",
                    "RTS-9.13_FT",
                    TradeAction.Buy,
                    OrderType.Limit,
                    10,
                    136000,
                    0);

            DateTime deliveryDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(1);
            OrderDeliveryConfirmation orderDeliveryConfirmation =
                new OrderDeliveryConfirmation(order, deliveryDate);

            string result = String.Format("Order delivery confirmation for: {0}, {1}",
                orderDeliveryConfirmation.OrderId,
                orderDeliveryConfirmation.DateTime.ToString(CultureInfo.InvariantCulture));

            Assert.AreEqual(result, orderDeliveryConfirmation.ToString());
       }
    }
}
