using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Events;
using TRL.Common.Collections;

namespace TRL.Common.Test.Collections
{
    public class OrderHashSetObserver : IGenericObserver<Order>
    {
        public int Count { get; set; }

        public OrderHashSetObserver()
        {
            this.Count = 0;
        }

        public void Update(Order item)
        {
            this.Count++;
        }
    }

    [TestClass]
    public class ObservableHashSetTests
    {
        [TestMethod]
        public void Collections_ObservableHashSet_test()
        {
            OrderHashSetObserver o = new OrderHashSetObserver();
            ObservableHashSet<Order> hs = new ObservableHashSet<Order>();
            hs.RegisterObserver(o);

            Assert.AreEqual(0, o.Count);
            hs.Add(new Order());

            Assert.AreEqual(1, o.Count);

        }
    }
}
