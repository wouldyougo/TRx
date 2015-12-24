using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Connect.Smartcom.Test.Mocks;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Events;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class MarketDataProviderTests:FakeAdapterBase
    {
        private IDataContext tradingData;
        private MarketDataProvider provider;
        private StServerClassMock stServer;
        private OrderBookContext orderBook;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.orderBook = new OrderBookContext();
            this.provider = new MarketDataProvider(this.Handlers, this.tradingData, this.orderBook, new NullLogger());
            this.stServer = (StServerClassMock)this.StServerMockSingleton.Instance;

            this.Binder.Bind();
        }

        [TestCleanup]
        public void Teardown()
        {
            this.Binder.Unbind();
            this.StServerMockSingleton.Destroy();
        }


        [TestMethod]
        public void DataProvider_Receive_Ticks()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Tick>>().Count());

            this.stServer.ReadTicksFrom("ticks.txt");

            Assert.IsTrue(this.tradingData.Get<IEnumerable<Tick>>().Count() > 0);
        }

        [TestMethod]
        public void Add_Tick_Into_MarketDataContext()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Tick>>().Count());

            this.stServer.EmulateTickArrival("RTS-6.16_FT", new DateTime(2013, 5, 5, 10, 1, 1), 145000, 100, StOrder_Action.StOrder_Action_Buy);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Tick>>().Count());

            this.stServer.EmulateTickArrival("RTS-6.16_FT", new DateTime(2013, 5, 5, 10, 1, 1), 145100, 200, StOrder_Action.StOrder_Action_Sell);
            Assert.AreEqual(2, this.tradingData.Get<IEnumerable<Tick>>().Count());

            this.stServer.EmulateTickArrival("Si-6.16_FT", new DateTime(2013, 5, 5, 10, 1, 1), 30255, 20, StOrder_Action.StOrder_Action_Sell);
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Tick>>().Count());
        }

        [TestMethod]
        public void Add_BidAsk_With_Zero_Row_Into_MarketDataContext()
        {
            Assert.AreEqual(0, this.orderBook.GetBidPrice("RTS-6.16_FT", 0));

            this.stServer.EmulateBidAskArrival("RTS-6.16_FT", 0, 1, 149000, 100, 150000, 200);
            Assert.AreEqual(149000, this.orderBook.GetBidPrice("RTS-6.16_FT", 0));

            this.stServer.EmulateBidAskArrival("RTS-6.16_FT", 0, 2, 148000, 200, 150000, 100);
            Assert.AreEqual(148000, this.orderBook.GetBidPrice("RTS-6.16_FT", 0));

            this.stServer.EmulateBidAskArrival("RTS-6.16_FT", 1, 2, 147990, 200, 150000, 100);
            Assert.AreEqual(147990, this.orderBook.GetBidPrice("RTS-6.16_FT", 1));

            this.stServer.EmulateBidAskArrival("Si-6.16_FT", 0, 1, 30000, 100, 31000, 200);
            Assert.AreEqual(30000, this.orderBook.GetBidPrice("Si-6.16_FT", 0));

            this.stServer.EmulateBidAskArrival("Si-6.16_FT", 1, 10, 31000, 100, 32000, 200);
            Assert.AreEqual(31000, this.orderBook.GetBidPrice("Si-6.16_FT", 1));
        }

        [TestMethod]
        public void Add_Bar_Into_MarketDataContext()
        {
            this.tradingData.Get<ICollection<Bar>>().Clear();

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Bar>>().Count());

            this.stServer.EmulateBarArrival("RTS-6.16_FT", StBarInterval.StBarInterval_60Min, DateTime.Now, 150000, 152000, 149000, 151000, 1000, 100);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Bar>>().Count());

            this.stServer.EmulateBarArrival("RTS-6.16_FT", StBarInterval.StBarInterval_60Min, DateTime.Now, 150000, 152000, 149000, 151000, 1000, 100);
            Assert.AreEqual(2, this.tradingData.Get<IEnumerable<Bar>>().Count());

            this.stServer.EmulateBarArrival("Si-6.16_FT", StBarInterval.StBarInterval_60Min, DateTime.Now, 30000, 30500, 29900, 30200, 100, 10);
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Bar>>().Count());
        }

    }
}
