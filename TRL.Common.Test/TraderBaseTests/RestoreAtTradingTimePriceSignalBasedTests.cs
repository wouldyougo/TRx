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
using TRL.Handlers.StopLoss;
using TRL.Handlers.TakeProfit;
using TRL.Transaction;
using TRL.Logging;
using TRL.Common.Handlers;

namespace TRL.Common.Test.TraderBaseTests
{
    [TestClass]
    public class RestoreAtTradingTimePriceSignalBasedTests
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ObservableQueue<Order> orderQueue;
        private IOrderManager orderManager;
        private TraderBase tb;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();
            this.orderQueue = new ObservableQueue<Order>();
            this.orderManager = new MockOrderManager();
            this.tb = new TraderBase(this.tradingData,
                this.signalQueue,
                this.orderQueue,
                this.orderManager,
                new AlwaysTimeToTradeSchedule(),
                new NullLogger());

            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy Description", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            StopPointsSettings spSettings = new StopPointsSettings(strategyHeader, 50, false);
            this.tradingData.Get<ICollection<StopPointsSettings>>().Add(spSettings);

            StopLossOrderSettings sloSettings = new StopLossOrderSettings(strategyHeader, 100);
            this.tradingData.Get<ICollection<StopLossOrderSettings>>().Add(sloSettings);

            ProfitPointsSettings ppSettings = new ProfitPointsSettings(strategyHeader, 100, false);
            this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(ppSettings);

            TakeProfitOrderSettings tpoSettings = new TakeProfitOrderSettings(strategyHeader, 100);
            this.tradingData.Get<ICollection<TakeProfitOrderSettings>>().Add(tpoSettings);

            StrategyStopLossByPointsOnTick stopLossHandler =
                new StrategyStopLossByPointsOnTick(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());
            StrategyTakeProfitByPointsOnTick takeProfitHandler =
                new StrategyTakeProfitByPointsOnTick(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());

            PlaceStrategyStopLossByPointsOnTrade placeStopOnTradeHandler =
                new PlaceStrategyStopLossByPointsOnTrade(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());
            PlaceStrategyTakeProfitByPointsOnTrade placeTakeProfitOnTradeHandler =
                new PlaceStrategyTakeProfitByPointsOnTrade(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());


            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(0, this.signalQueue.Count);
            Assert.AreEqual(0, this.orderQueue.Count);
        }

        [TestMethod]
        public void make_signals_when_backup_loaded_at_trading_time()
        {
            ITransaction importSignals = new ImportSignalsTransaction(this.tradingData, ProjectRootFolderNameFactory.Make() + "\\TestData\\signals-backup.txt");
            ITransaction importOrders = new ImportOrdersTransaction((IObservableHashSetFactory)this.tradingData, ProjectRootFolderNameFactory.Make() + "\\TestData\\orders-backup.txt");
            ITransaction importTrades = new ImportTradesTransaction((IObservableHashSetFactory)this.tradingData, ProjectRootFolderNameFactory.Make() + "\\TestData\\trades-backup.txt");

            importSignals.Execute();
            importOrders.Execute();
            importTrades.Execute();

            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(0, this.signalQueue.Count);
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(2, ((MockOrderManager)this.orderManager).PlaceCounter);
        }
    }
}
