using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Common.Models;
using TRL.Common.Handlers;
using TRL.Common.Test.Mocks;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Handlers.Test
{
    public class MockGenericHashSetObserver : GenericHashSetObserver<Trade>
    {
        public MockGenericHashSetObserver(IObservableHashSetFactory tradingData)
            : base(tradingData)
        {
        }

        public int ShotCounter { get; set; }

        public override void Update(Trade item)
        {
            this.ShotCounter++;
        }
    }

    [TestClass]
    public class GenericHashSetObserverTests
    {
        private IDataContext tradingData;

        [TestInitialize]
        public void Handlers_Setup()
        {
            this.tradingData = new TradingDataContext();

            
        }

        [TestMethod]
        public void Handlers_MockObserver_Test()
        {

            GenericHashSetObserver<Trade> trigger = new MockGenericHashSetObserver((IObservableHashSetFactory)this.tradingData);
            MockGenericHashSetObserver mockTrigger = (MockGenericHashSetObserver)trigger;

            StrategyHeader s = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-9.13_FT", 10);
            Signal signal = new Signal(s, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 130000, 0, 0);
            Order order = new Order(signal);

            Assert.AreEqual(0, mockTrigger.ShotCounter);

            this.tradingData.Get<ObservableHashSet<Trade>>().Add(new Trade(order, s.Portfolio, s.Symbol, 131000, 4, BrokerDateTime.Make(DateTime.Now)));

            Assert.AreEqual(1, mockTrigger.ShotCounter);
        }
    }
}
