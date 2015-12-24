using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common;
using TRL.Common.Test;
using TRL.Common.TimeHelpers;

namespace TRL.Transaction.Test
{
    [TestClass]
    public class ImportTradesTransactionTests
    {
        private IDataContext tradingData;
        private string path;

        [TestInitialize]
        public void Transaction_Setup()
        {
            this.tradingData = new TradingDataContext();
            this.path = String.Concat(ProjectRootFolderNameFactory.Make(), "\\TestData\\trades.txt");
        }

        [TestMethod]
        public void Transaction_ImportTradesTransaction_import_three_orders()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "First strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            StrategyHeader strategy1 = new StrategyHeader(2, "Second strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategy1);

            StrategyHeader strategy2 = new StrategyHeader(3, "Third strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategy2);

            Signal s1 = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            s1.Id = 1;
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Signal s2 = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            s2.Id = 2;
            this.tradingData.Get<ICollection<Signal>>().Add(s2);

            Signal s3 = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Stop, 150000, 150000, 0);
            s3.Id = 3;
            this.tradingData.Get<ICollection<Signal>>().Add(s3);

            Order o1 = new Order(s1);
            o1.Id = 1;
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Order o2 = new Order(s2);
            o2.Id = 2;
            this.tradingData.Get<ICollection<Order>>().Add(o2);

            Order o3 = new Order(s3);
            o3.Id = 3;
            this.tradingData.Get<ICollection<Order>>().Add(o3);

            ITransaction import = new ImportTradesTransaction((IObservableHashSetFactory)this.tradingData, this.path);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());

            import.Execute();

            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Trade>>().Count());
        }

        [TestMethod]
        public void Transaction_import_trades_only_for_existent_orderss()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "First strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            Signal signal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            signal.Id = 1;
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order o1 = new Order(signal);
            o1.Id = 1;
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());

            ITransaction import = new ImportTradesTransaction((IObservableHashSetFactory)this.tradingData, this.path);

            import.Execute();

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());
        }

    }
}
