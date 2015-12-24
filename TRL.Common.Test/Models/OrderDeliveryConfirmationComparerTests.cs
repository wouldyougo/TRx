using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using TRL.Common.Collections;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class OrderDeliveryConfirmationComparerTests
    {
        [TestMethod]
        public void two_confirmations_for_one_order_are_equals()
        {
            Order order = new Order(1, 
                BrokerDateTime.Make(DateTime.Now), 
                "BP12345-RF-01", 
                "RTS-9.13_FT", 
                TradeAction.Buy, 
                OrderType.Limit, 
                10, 
                136000, 
                0);

            OrderDeliveryConfirmation c1 = 
                new OrderDeliveryConfirmation(order, BrokerDateTime.Make(DateTime.Now));

            OrderDeliveryConfirmation c2 =
                new OrderDeliveryConfirmation(order, BrokerDateTime.Make(DateTime.Now));

            EqualityComparer<OrderDeliveryConfirmation> ec =
                new OrderDeliveryConfirmationComparer();

            Assert.IsTrue(ec.Equals(c1, c2));
        }

        [TestMethod]
        public void two_confirmations_for_different_orders_are_not_equals()
        {
            Order o1 = new Order(1,
                BrokerDateTime.Make(DateTime.Now),
                "BP12345-RF-01",
                "RTS-9.13_FT",
                TradeAction.Buy,
                OrderType.Limit,
                10,
                136000,
                0);

            OrderDeliveryConfirmation c1 =
                new OrderDeliveryConfirmation(o1, BrokerDateTime.Make(DateTime.Now));

            Order o2 = new Order(2,
                BrokerDateTime.Make(DateTime.Now),
                "BP12345-RF-01",
                "RTS-9.13_FT",
                TradeAction.Sell,
                OrderType.Limit,
                10,
                136000,
                0);

            OrderDeliveryConfirmation c2 =
                new OrderDeliveryConfirmation(o2, BrokerDateTime.Make(DateTime.Now));

            EqualityComparer<OrderDeliveryConfirmation> ec =
                new OrderDeliveryConfirmationComparer();

            Assert.IsFalse(ec.Equals(c1, c2));
        }

        [TestMethod]
        public void cant_add_two_confirmations_for_one_order_to_hashset()
        {
            ObservableHashSet<OrderDeliveryConfirmation> confirmations =
                new ObservableHashSet<OrderDeliveryConfirmation>(new OrderDeliveryConfirmationComparer());

            Order o1 = new Order(1,
                BrokerDateTime.Make(DateTime.Now),
                "BP12345-RF-01",
                "RTS-9.13_FT",
                TradeAction.Buy,
                OrderType.Limit,
                10,
                136000,
                0);

            OrderDeliveryConfirmation c1 =
                new OrderDeliveryConfirmation(o1, BrokerDateTime.Make(DateTime.Now));
            confirmations.Add(c1);
            Assert.AreEqual(1, confirmations.Count);

            OrderDeliveryConfirmation c2 =
                new OrderDeliveryConfirmation(o1, BrokerDateTime.Make(DateTime.Now));
            confirmations.Add(c2);
            Assert.AreEqual(1, confirmations.Count);

        }

        [TestMethod]
        public void only_different_order_confirmations_can_be_added_to_hashset()
        {
            ObservableHashSet<OrderDeliveryConfirmation> confirmations =
                new ObservableHashSet<OrderDeliveryConfirmation>(new OrderDeliveryConfirmationComparer());

            Order o1 = new Order(1,
                BrokerDateTime.Make(DateTime.Now),
                "BP12345-RF-01",
                "RTS-9.13_FT",
                TradeAction.Buy,
                OrderType.Limit,
                10,
                136000,
                0);

            OrderDeliveryConfirmation c1 =
                new OrderDeliveryConfirmation(o1, BrokerDateTime.Make(DateTime.Now));
            confirmations.Add(c1);
            Assert.AreEqual(1, confirmations.Count);

            Order o2 = new Order(2,
                BrokerDateTime.Make(DateTime.Now),
                "BP12345-RF-01",
                "RTS-9.13_FT",
                TradeAction.Sell,
                OrderType.Limit,
                10,
                136000,
                0);

            OrderDeliveryConfirmation c2 =
                new OrderDeliveryConfirmation(o2, BrokerDateTime.Make(DateTime.Now));
            confirmations.Add(c2);
            Assert.AreEqual(2, confirmations.Count);

        }
    }
}
