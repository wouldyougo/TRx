using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common;
using System.IO;
using TRL.Common.Test;
using TRL.Common.TimeHelpers;

namespace TRL.Transaction.Test
{
    [TestClass]
    public class ExportOrdersTransactionTests
    {
        private IDataContext tradingData;
        private string path;

        [TestInitialize]
        public void Transaction_Setup()
        {
            this.tradingData = new TradingDataContext();
            this.path = String.Concat(ProjectRootFolderNameFactory.Make(), "\\export-orders.txt");
        }

        [TestCleanup]
        public void Transaction_TearDown()
        {
            if (File.Exists(this.path))
                File.Delete(this.path);
        }

        [TestMethod]
        public void Transaction_ExportOrdersTransaction_test()
        {
            StrategyHeader st1 = new StrategyHeader(1, "First strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st1);

            StrategyHeader st2 = new StrategyHeader(2, "Second strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 8);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st2);

            StrategyHeader st3 = new StrategyHeader(3, "Third strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 5);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st3);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(s2);

            Order o2 = new Order(s2);
            this.tradingData.Get<ICollection<Order>>().Add(o2);

            Signal s3 = new Signal(st3, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Stop, 150000, 150000, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(s3);

            Order o3 = new Order(s3);
            this.tradingData.Get<ICollection<Order>>().Add(o3);

            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Order>>().Count());

            ITransaction export = new ExportOrdersTransaction((IObservableHashSetFactory)this.tradingData, this.path);

            Assert.IsFalse(File.Exists(this.path));

            export.Execute();

            Assert.IsTrue(File.Exists(this.path));
            this.tradingData.Get<ICollection<Order>>().Clear();
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());

            ITransaction import = new ImportOrdersTransaction((IObservableHashSetFactory)this.tradingData, this.path);
            import.Execute();

            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Order>>().Count());
        }

        [TestMethod]
        public void Transaction_ExportOrdersTransaction_do_nothing_when_no_orders_exists()
        {
            StrategyHeader st1 = new StrategyHeader(1, "First strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st1);

            StrategyHeader st2 = new StrategyHeader(2, "Second strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 8);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st2);

            StrategyHeader st3 = new StrategyHeader(3, "Third strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 5);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st3);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Signal s2 = new Signal(st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(s2);

            Signal s3 = new Signal(st3, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Stop, 150000, 150000, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(s3);

            ITransaction export = new ExportOrdersTransaction((IObservableHashSetFactory)this.tradingData, this.path);
            export.Execute();
            Assert.IsFalse(File.Exists(this.path));
        }
    }
}
