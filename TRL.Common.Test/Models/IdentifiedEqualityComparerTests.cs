using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class IdentifiedEqualityComparerTests
    {
        [TestMethod]
        public void Orders_With_Same_Id_Are_Equals()
        {
            IdentifiedEqualityComparer<Order> iec = new IdentifiedEqualityComparer<Order>();

            HashSet<Order> orders = new HashSet<Order>(iec);

            Order order = new Order { Id = 1 };

            orders.Add(order);

            Assert.AreEqual(1, orders.Count);

            Order two = new Order { Id = 1 };

            orders.Add(two);

            Assert.AreEqual(1, orders.Count);
        }

        [TestMethod]
        public void IdentifiedEqualityComparer_test()
        {
            EqualityComparer<IIdentified> iec = new IdentifiedComparer();

            HashSet<Order> orders = new HashSet<Order>(iec);

            Order order = new Order { Id = 1 };

            orders.Add(order);

            Assert.AreEqual(1, orders.Count);

            Order two = new Order { Id = 1 };

            orders.Add(two);

            Assert.AreEqual(1, orders.Count);
        }
    }
}
