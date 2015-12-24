using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Test.Mocks;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Common.TimeHelpers;
using TRL.Emulation;
using TRL.Transaction;

namespace TRL.Common.Test.TraderBaseTests
{
    [TestClass]
    public class RestoreAtNotTradingTimePriceSignalBasedTests:TraderBaseInitializer
    {

        [TestInitialize]
        public void Setup()
        {
            StrategyHeader strategyHeader = new StrategyHeader(6, "Strategy Description", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(0, this.signalQueue.Count);
            Assert.AreEqual(0, this.orderQueue.Count);
        }

        [TestMethod]
        public void ignore_signals_when_backup_loaded_at_not_trading_time()
        {
            ITransaction importSignals = new ImportSignalsTransaction(this.tradingData, ProjectRootFolderNameFactory.Make() + "\\TestData\\signals-backup.txt");
            ITransaction importOrders = new ImportOrdersTransaction((IObservableHashSetFactory)this.tradingData, ProjectRootFolderNameFactory.Make() + "\\TestData\\orders-backup.txt");
            ITransaction importTrades = new ImportTradesTransaction((IObservableHashSetFactory)this.tradingData, ProjectRootFolderNameFactory.Make() + "\\TestData\\trades-backup.txt");

            importSignals.Execute();
            importOrders.Execute();
            importTrades.Execute();

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(0, this.signalQueue.Count);
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(0, ((MockOrderManager)this.orderManager).PlaceCounter);
        }
    }
}
