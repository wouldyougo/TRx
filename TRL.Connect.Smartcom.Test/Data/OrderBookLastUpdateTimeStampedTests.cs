using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Events;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Common.TimeHelpers;
using TRL.Connect.Smartcom.Events;
using TRL.Connect.Smartcom.Data;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class OrderBookLastUpdateTimeStampedTests
    {
        private IQuoteProvider orderBook;
        private IDateTime lastOrderBookUpdate;

        [TestInitialize]
        public void Setup()
        {
            this.orderBook = new OrderBookContext();

            this.lastOrderBookUpdate =
                new OrderBookLastUpdateTimeStamped(this.orderBook);

            Assert.IsTrue(this.lastOrderBookUpdate.DateTime == DateTime.MinValue);
        }

        [TestMethod]
        public void notify_DateTime_of_last_OrderBook_update_test()
        {
            this.orderBook.Update(1, "RTS-9.14", 135000, 32, 135010, 11);
            Assert.AreNotEqual(DateTime.MinValue, this.lastOrderBookUpdate.DateTime);
        }
    }
}
