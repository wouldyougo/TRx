using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using TRL.Connect.Smartcom.Events;
using TRL.Common.Collections;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class RawUpdateBidAskHandlerTests
    {
        private ObservableCollection<UpdateBidAsk> rawData;
        private IDataContext tradingData;

        [TestInitialize]
        public void Setup()
        {
            this.rawData = new ObservableCollection<UpdateBidAsk>();
            this.tradingData = new TradingDataContext();
            RawUpdateBidAskHandler handler = new RawUpdateBidAskHandler(this.rawData, this.tradingData, new NullLogger());
            this.rawData.RegisterObserver(handler);
        }

        [TestMethod]
        public void Ignore_UpdateBidAsk_With_Non_Zero_Row()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<BidAsk>>().Count());

            this.rawData.Add(new UpdateBidAsk("RTS-6.13_FT", 1, 1, 150000, 100, 151000, 50));

            Assert.AreEqual(0, this.rawData.Count);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<BidAsk>>().Count());
        }

        [TestMethod]
        public void Add_BidAsk_Record_On_UpdateBidAsk_With_Non_Zero_Row()
        {
            DateTime itemDate = BrokerDateTime.Make(DateTime.Now);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<BidAsk>>().Count());

            this.rawData.Add(new UpdateBidAsk("RTS-6.13_FT", 0, 1, 150000, 100, 151000, 50));

            Assert.AreEqual(0, this.rawData.Count);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<BidAsk>>().Count());

            BidAsk item = this.tradingData.Get<IEnumerable<BidAsk>>().Last();

            Assert.AreEqual("RTS-6.13_FT", item.Symbol);
            Assert.AreEqual(0, item.Row);
            Assert.AreEqual(1, item.NRows);
            Assert.AreEqual(150000, item.Bid);
            Assert.AreEqual(100, item.BidSize);
            Assert.AreEqual(151000, item.Ask);
            Assert.AreEqual(50, item.AskSize);

            Assert.AreEqual(itemDate.Year, item.DateTime.Year);
            Assert.AreEqual(itemDate.Month, item.DateTime.Month);
            Assert.AreEqual(itemDate.Day, item.DateTime.Day);
            Assert.AreEqual(itemDate.Hour, item.DateTime.Hour);
            Assert.AreEqual(itemDate.Minute, item.DateTime.Minute);
            Assert.AreEqual(itemDate.Second, item.DateTime.Second);
        }

        [TestMethod]
        public void Update_BidAsk_Record_On_UpdateBidAsk()
        {
            DateTime itemDate = BrokerDateTime.Make(DateTime.Now);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<BidAsk>>().Count());

            this.rawData.Add(new UpdateBidAsk("RTS-6.13_FT", 0, 1, 150000, 100, 151000, 50));

            Assert.AreEqual(0, this.rawData.Count);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<BidAsk>>().Count());

            BidAsk item = this.tradingData.Get<IEnumerable<BidAsk>>().Last();

            Assert.AreEqual("RTS-6.13_FT", item.Symbol);
            Assert.AreEqual(0, item.Row);
            Assert.AreEqual(1, item.NRows);
            Assert.AreEqual(150000, item.Bid);
            Assert.AreEqual(100, item.BidSize);
            Assert.AreEqual(151000, item.Ask);
            Assert.AreEqual(50, item.AskSize);

            Assert.AreEqual(itemDate.Year, item.DateTime.Year);
            Assert.AreEqual(itemDate.Month, item.DateTime.Month);
            Assert.AreEqual(itemDate.Day, item.DateTime.Day);
            Assert.AreEqual(itemDate.Hour, item.DateTime.Hour);
            Assert.AreEqual(itemDate.Minute, item.DateTime.Minute);

            itemDate = BrokerDateTime.Make(DateTime.Now);

            this.rawData.Add(new UpdateBidAsk("RTS-6.13_FT", 0, 3, 155000, 300, 153000, 80));

            Assert.AreEqual(0, this.rawData.Count);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<BidAsk>>().Count());

            item = this.tradingData.Get<IEnumerable<BidAsk>>().Last();

            Assert.AreEqual("RTS-6.13_FT", item.Symbol);
            Assert.AreEqual(0, item.Row);
            Assert.AreEqual(3, item.NRows);
            Assert.AreEqual(155000, item.Bid);
            Assert.AreEqual(300, item.BidSize);
            Assert.AreEqual(153000, item.Ask);
            Assert.AreEqual(80, item.AskSize);

            Assert.AreEqual(itemDate.Year, item.DateTime.Year);
            Assert.AreEqual(itemDate.Month, item.DateTime.Month);
            Assert.AreEqual(itemDate.Day, item.DateTime.Day);
            Assert.AreEqual(itemDate.Hour, item.DateTime.Hour);
            Assert.AreEqual(itemDate.Minute, item.DateTime.Minute);
            Assert.AreEqual(itemDate.Second, item.DateTime.Second);
        }

    }
}
