using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Extensions.Data;
using TRL.Common.Data;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class OrderBookContextExtensionsGetBestPriceTests
    {
        private OrderBookContext orderBook;

        [TestInitialize]
        public void Setup()
        {
            this.orderBook = new OrderBookContext();
        }

        [TestMethod]
        public void OrderBookContextExtensions_GetBestOfferPrice_test()
        {
            this.orderBook.Update(0, "RTS-3.14_FT", 130020, 35, 130050, 3);

            Assert.AreEqual(130040, this.orderBook.GetBestOfferPrice("RTS-3.14_FT", 10));
        }

        [TestMethod]
        public void OrderBookContextExtensions_GetBestBidPrice_test()
        {
            this.orderBook.Update(0, "RTS-3.14_FT", 130020, 35, 130050, 3);

            Assert.AreEqual(130030, this.orderBook.GetBestBidPrice("RTS-3.14_FT", 10));
        }

        [TestMethod]
        public void OrderBookContextExtensions_GetBestOfferPrice_when_no_spread_test()
        {
            this.orderBook.Update(0, "RTS-3.14_FT", 130020, 35, 130030, 3);

            Assert.AreEqual(130030, this.orderBook.GetBestOfferPrice("RTS-3.14_FT", 10));
        }

        [TestMethod]
        public void OrderBookContextExtensions_GetBestBidPrice_when_no_spread_test()
        {
            this.orderBook.Update(0, "RTS-3.14_FT", 130020, 35, 130030, 3);

            Assert.AreEqual(130020, this.orderBook.GetBestBidPrice("RTS-3.14_FT", 10));
        }
    }
}
