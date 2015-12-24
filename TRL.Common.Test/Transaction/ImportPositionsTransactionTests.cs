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
    public class ImportPositionsTransactionTests
    {
        private IDataContext tradingData;

        [TestInitialize]
        public void Transaction_Setup()
        {
            this.tradingData = new TradingDataContext();
        }

        [TestMethod]
        public void Transaction_import_positions()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());

            string ef = String.Concat(ProjectRootFolderNameFactory.Make(), "\\TestData\\positions.txt");
            ITransaction import = new ImportPositionsTransaction((IObservableHashSetFactory)this.tradingData, ef);

            import.Execute();

            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Position>>().Count());
        }
    }
}
