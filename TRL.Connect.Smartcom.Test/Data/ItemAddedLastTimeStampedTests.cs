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
    public class ItemAddedLastTimeStampedTests
    {
        private IDataContext dataContext;
        private IDateTime lastItemAdded;

        [TestInitialize]
        public void Setup()
        {
            this.dataContext = new TradingDataContext();
            this.lastItemAdded =
                new ItemAddedLastTimeStamped<Tick>(this.dataContext.Get<ObservableCollection<Tick>>());

            Assert.IsTrue(this.lastItemAdded.DateTime == DateTime.MinValue);
        }

        [TestMethod]
        public void notify_DateTime_of_last_incoming_tick_test()
        {
            this.dataContext.Get<ObservableCollection<Tick>>().Add(new Tick("RTS-9.14", DateTime.Now, TradeAction.Buy, 125000, 3));
            Assert.AreNotEqual(DateTime.MinValue, this.lastItemAdded.DateTime);
        }
    }
}
