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
    public class ExportSignalsTransactionTests
    {
        private IDataContext tradingData;
        private string ef;

        [TestInitialize]
        public void Transaction_Setup()
        {
            this.tradingData = new TradingDataContext();
            this.ef = String.Concat(ProjectRootFolderNameFactory.Make(), "\\export-signals.txt");
        }

        [TestCleanup]
        public void Transaction_TearDown()
        {
            if (File.Exists(ef))
                File.Delete(ef);
        }

        [TestMethod]
        public void Transaction_export_three_signals()
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

            ITransaction export = new ExportSignalsTransaction((IObservableHashSetFactory)this.tradingData, this.ef);
            export.Execute();            
            Assert.IsTrue(File.Exists(this.ef));

            this.tradingData.Get<ICollection<Signal>>().Clear();
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());

            ITransaction import = new ImportSignalsTransaction(this.tradingData, this.ef);
            import.Execute();
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Signal>>().Count());
        }

        [TestMethod]
        public void Transaction_ExportSignalsTransaction_do_nothing_when_no_signals_exists()
        {
            StrategyHeader st1 = new StrategyHeader(1, "First strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st1);

            StrategyHeader st2 = new StrategyHeader(2, "Second strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 8);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st2);

            StrategyHeader st3 = new StrategyHeader(3, "Third strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 5);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st3);

            ITransaction export = new ExportSignalsTransaction((IObservableHashSetFactory)this.tradingData, this.ef);
            export.Execute();
            Assert.IsFalse(File.Exists(this.ef));
        }
    }
}
