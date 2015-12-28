using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Handlers;
using TRL.Common.TimeHelpers;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Test.Handlers
{
    [TestClass]
    public class UpdateOrderBookOnBidAskTests
    {
        private IDataContext tradingData;
        private OrderBookContext storage;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.storage = new OrderBookContext();

            UpdateOrderBookOnBidAsk handler = new UpdateOrderBookOnBidAsk(this.storage, this.tradingData, new NullLogger());
        }

        [TestMethod]
        public void UpdateQuotesOnBidAsk_handle_add_records_for_registered_symbol()
        {
            BidAsk bidAskOne = new BidAsk { Id = SerialIntegerFactory.Make(), Symbol = "RTS-9.13_FT", DateTime = BrokerDateTime.Make(DateTime.Now), Row = 0, NRows = 10, Ask = 150010, AskSize = 300, Bid = 150000, BidSize = 100 };

            this.tradingData.Get<ObservableCollection<BidAsk>>().Add(bidAskOne);

            double resultBidPrice = storage.GetBidPrice(bidAskOne.Symbol, 0);
            double resultBidVolume = storage.GetBidVolume(bidAskOne.Symbol, 0);

            double resultOfferPrice = storage.GetOfferPrice(bidAskOne.Symbol, 0);
            double resultOfferVolume = storage.GetOfferVolume(bidAskOne.Symbol, 0);

            Assert.AreEqual(bidAskOne.Bid,resultBidPrice);
            Assert.AreEqual(bidAskOne.BidSize, resultBidVolume);

            Assert.AreEqual(bidAskOne.Ask, resultOfferPrice);
            Assert.AreEqual(bidAskOne.AskSize, resultOfferVolume);
        }
    }
}
