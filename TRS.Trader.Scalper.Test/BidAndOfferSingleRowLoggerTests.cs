using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common;
using TRL.Logging;

namespace TRx.Trader.Scalper.Test
{
    [TestClass]
    public class BidAndOfferSingleRowLoggerTests
    {
        private IQuoteProvider orderBook;
        private string symbol;
        private int rowIndex;
        private BidAndOfferSingleRowLogger handler;

        [TestInitialize]
        public void Setup()
        {
            this.orderBook = new OrderBookContext(10);
            this.symbol = "RTS-3.14_FT";
            this.rowIndex = 0;

            this.handler =
                new BidAndOfferSingleRowLogger(this.symbol, this.rowIndex, (OrderBookContext)this.orderBook, new NullLogger());

            Assert.AreEqual(0, this.orderBook.GetBidPrice(this.symbol, this.rowIndex));
            Assert.AreEqual(0, this.orderBook.GetOfferPrice(this.symbol, this.rowIndex));
            Assert.AreEqual(0, this.handler.PreviousBidPrice);
            Assert.AreEqual(0, this.handler.PreviousOfferPrice);
            Assert.AreEqual(0, this.handler.LoggedRowsCounter);
        }

        [TestMethod]
        public void BidAndOfferSingleRowLogger_log_first_orderBook_change_test()
        {
            this.orderBook.Update(this.rowIndex, this.symbol, 135, 10, 136, 15);

            Assert.AreEqual(135, this.handler.PreviousBidPrice);
            Assert.AreEqual(136, this.handler.PreviousOfferPrice);
            Assert.AreEqual(1, this.handler.LoggedRowsCounter);
        }

        [TestMethod]
        public void BidAndOfferSingleRowLogger_do_not_log_if_update_contains_same_prices_test()
        {
            this.orderBook.Update(this.rowIndex, this.symbol, 135, 10, 136, 15);

            Assert.AreEqual(135, this.handler.PreviousBidPrice);
            Assert.AreEqual(136, this.handler.PreviousOfferPrice);
            Assert.AreEqual(1, this.handler.LoggedRowsCounter);

            this.orderBook.Update(this.rowIndex, this.symbol, 135, 10, 136, 15);
            Assert.AreEqual(1, this.handler.LoggedRowsCounter);
        }

        [TestMethod]
        public void BidAndOfferSingleRowLogger_do_not_log_if_update_contains_other_symbol_test()
        {
            this.orderBook.Update(this.rowIndex, "Si", 135, 10, 136, 15);

            Assert.AreEqual(0, this.handler.PreviousBidPrice);
            Assert.AreEqual(0, this.handler.PreviousOfferPrice);
            Assert.AreEqual(0, this.handler.LoggedRowsCounter);
        }

        [TestMethod]
        public void BidAndOfferSingleRowLogger_do_not_log_if_update_contains_other_row_index_test()
        {
            this.orderBook.Update(5, this.symbol, 135, 10, 136, 15);

            Assert.AreEqual(0, this.handler.PreviousBidPrice);
            Assert.AreEqual(0, this.handler.PreviousOfferPrice);
            Assert.AreEqual(0, this.handler.LoggedRowsCounter);
        }
    }
}
