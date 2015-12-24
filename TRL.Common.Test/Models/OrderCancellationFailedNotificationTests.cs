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
    public class OrderCancellationFailedNotificationTests
    {
        [TestMethod]
        public void OrderCancellationFailedNotification_constructor()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            Signal signal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Stop, 150000, 149000, 0);
            Order order = new Order(signal);
            DateTime notificationDate = BrokerDateTime.Make(DateTime.Now);

            OrderCancellationFailedNotification notification = new OrderCancellationFailedNotification(order, notificationDate, "Order already filled");

            Assert.IsTrue(notification.Id > 0);
            Assert.AreEqual(notificationDate, notification.DateTime);
            Assert.AreEqual("Order already filled", notification.Description);
            Assert.AreSame(order, notification.Order);
            Assert.AreEqual(order.Id, notification.OrderId);
        }
    }
}
