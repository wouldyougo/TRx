using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;

namespace TRL.Common.Test.Data
{
    [TestClass]
    public class OrderBookContextTests
    {
        [TestMethod]
        public void OrderBookContext_constructor_test()
        {
            int depth = 10;
            OrderBookContext obContext = new OrderBookContext(depth);

            Assert.AreEqual(10, obContext.Depth);
            Assert.AreEqual(0, obContext.GetOfferPrice("RTS-12.13_FT", 0));
            Assert.AreEqual(0, obContext.GetBidPrice("RTS-12.13_FT", 0));
        }

        [TestMethod]
        public void OrderBookContext_update_test()
        {
            OrderBookContext obContext = new OrderBookContext(10);

            obContext.Update(0, "RTS-12.13_FT", 140000, 100, 140010, 50);
            Assert.AreEqual(140010, obContext.GetOfferPrice("RTS-12.13_FT", 0));
            Assert.AreEqual(140000, obContext.GetBidPrice("RTS-12.13_FT", 0));
            Assert.AreEqual(50, obContext.GetOfferVolume("RTS-12.13_FT", 0));
            Assert.AreEqual(100, obContext.GetBidVolume("RTS-12.13_FT", 0));
        }

        [TestMethod]
        public void OrderBookContext_update_notification_test()
        {
            OrderBookContext obContext = new OrderBookContext(10);
            obContext.OnQuotesUpdate += new SymbolDataUpdatedNotification(Notify);

            Assert.AreEqual(string.Empty, this.symbol);

            obContext.Update(0, "RTS-12.13_FT", 140000, 100, 140010, 50);
            
            Assert.AreEqual("RTS-12.13_FT", this.symbol);

        }

        private string symbol = string.Empty;

        private void Notify(string symbol)
        {
            this.symbol = symbol;
        }
    }
}
