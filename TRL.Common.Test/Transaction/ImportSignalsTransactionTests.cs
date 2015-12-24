using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using TRL.Common.Test;

namespace TRL.Transaction.Test
{
    [TestClass]
    public class ImportSignalsTransactionTests
    {
        private IDataContext tradingData;

        [TestInitialize]
        public void Transaction_Setup()
        {
            this.tradingData = new TradingDataContext();
        }

        [TestMethod]
        public void Transaction_import_signals()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "First strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            StrategyHeader strategy1 = new StrategyHeader(2, "Second strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategy1);

            StrategyHeader strategy2 = new StrategyHeader(3, "Third strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategy2);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());

            string ef = String.Concat(ProjectRootFolderNameFactory.Make(), "\\TestData\\signals.txt");
            ITransaction import = new ImportSignalsTransaction(this.tradingData, ef);

            import.Execute();

            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Signal>>().Count());
        }

        [TestMethod]
        public void Transaction_import_signal_only_for_existent_strategy()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "First strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());

            string ef = String.Concat(ProjectRootFolderNameFactory.Make(), "\\TestData\\signals.txt");
            ITransaction import = new ImportSignalsTransaction(this.tradingData, ef);

            import.Execute();

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
        }
    }
}
