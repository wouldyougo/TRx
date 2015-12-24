using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;

namespace TRL.Common.Test.Data
{
    [TestClass]
    public class OrderBookTests
    {
        [TestMethod]
        public void OrderBook_is_singleton()
        {
            OrderBookContext storage = OrderBook.Instance;
            OrderBookContext anotherStorage = OrderBook.Instance;

            Assert.AreSame(storage, anotherStorage);
        }
    }
}
