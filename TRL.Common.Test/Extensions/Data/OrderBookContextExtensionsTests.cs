using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class OrderBookContextExtensionsTests
    {
        private OrderBookContext orderBook;

        [TestInitialize]
        public void Setup()
        {
            this.orderBook = new OrderBookContext();

            for (int i = 0; i < this.orderBook.Depth; i++)
                this.orderBook.Update(i, "Any", i, 10, i + this.orderBook.Depth, 20);
        }

        [TestMethod]
        public void OrderBookContextExtensions_GetOfferVolumeSum_test()
        {
            Assert.AreEqual(1000, this.orderBook.GetOfferVolumeSum("Any"));
        }

        [TestMethod]
        public void OrderBookContextExtensions_GetBidVolumeSum_test()
        {
            Assert.AreEqual(500, this.orderBook.GetBidVolumeSum("Any"));
        }

        [TestMethod]
        public void OrderBookContextExtensions_GetOfferVolumeSum_for_rows_test()
        {
            Assert.AreEqual(60, this.orderBook.GetOfferVolumeSum("Any", 3));
        }

        [TestMethod]
        public void OrderBookContextExtensions_GetBidVolumeSum_for_rows_test()
        {
            Assert.AreEqual(50, this.orderBook.GetBidVolumeSum("Any", 5));
        }
    }
}
