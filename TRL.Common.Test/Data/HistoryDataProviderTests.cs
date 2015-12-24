using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Test.Mocks;

namespace TRL.Common.Test.Data
{
    [TestClass]
    public class HistoryDataProviderTests
    {
        private IDataContext tradingData;
        private IHistoryDataProvider historyDataProvider;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.historyDataProvider = new FakeHistoryDataProvider(this.tradingData);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Tick>>().Where(t => t.Symbol == "RTS-6.14").Count());
        }

        [TestMethod]
        public void HistoryDataProvider_send_tick_request_test()
        {
            IHistoryDataRequest tickRequest =
                new TickHistoryDataRequest("RTS-6.14", 10, DateTime.Now);

            this.historyDataProvider.Send(tickRequest);

            Assert.AreEqual(10, this.tradingData.Get<IEnumerable<Tick>>().Where(t => t.Symbol == "RTS-6.14").Count());
        }
    }
}
