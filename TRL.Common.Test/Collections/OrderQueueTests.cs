using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Collections;
using TRL.Common.Models;

namespace TRL.Common.Test.Collections
{
    [TestClass]
    public class OrderQueueTests
    {
        [TestMethod]
        public void Collections_OrderQueue_Instance_Is_ObservableQueue()
        {
            Assert.IsInstanceOfType(OrderQueue.Instance, typeof(ObservableQueue<Order>));
        }

        [TestMethod]
        public void Collections_OrderQueue_Instance()
        {
            Assert.IsNotNull(OrderQueue.Instance);
        }

        [TestMethod]
        public void Collections_OrderQueue_Is_Singleton()
        {
            OrderQueue q = OrderQueue.Instance;
            OrderQueue q2 = OrderQueue.Instance;

            Assert.AreSame(q, q2);
        }
    }
}
