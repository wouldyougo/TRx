using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class OrderBookEntryTests
    {
        [TestMethod]
        public void OrderBookEntry_constructor_test()
        {
            double price = 100;
            double volume = 10;

            OrderBookEntry entry = new OrderBookEntry(price, volume);

            Assert.AreEqual(price, entry.Price);
            Assert.AreEqual(volume, entry.Volume);
        }
    }
}
