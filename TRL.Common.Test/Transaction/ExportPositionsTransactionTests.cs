using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using System.IO;
using TRL.Common.Test;

namespace TRL.Transaction.Test
{
    [TestClass]
    public class ExportPositionsTransactionTests
    {
        private IDataContext tradingData;
        private string ef;

        [TestInitialize]
        public void Transaction_Setup()
        {
            this.tradingData = new TradingDataContext();
            this.ef = String.Concat(ProjectRootFolderNameFactory.Make(), "\\export-positions.txt");
        }

        [TestCleanup]
        public void Transaction_TearDown()
        {
            if (File.Exists(ef))
                File.Delete(ef);
        }

        [TestMethod]
        public void Transaction_export_positions()
        {
            Position p1 = new Position(1, "BP12345-RF-01", "RTS-9.13_FT", 8);
            this.tradingData.Get<ICollection<Position>>().Add(p1);

            Position p2 = new Position(2, "BP12345-RF-01", "Si-9.13_FT", 8);
            this.tradingData.Get<ICollection<Position>>().Add(p2);

            Position p3 = new Position(3, "BP12345-RF-01", "SBRF-9.13_FT", 8);
            this.tradingData.Get<ICollection<Position>>().Add(p3);

            ITransaction export = new ExportPositionsTransaction((IObservableHashSetFactory)this.tradingData, this.ef);
            export.Execute();
            Assert.IsTrue(File.Exists(this.ef));

            this.tradingData.Get<ICollection<Position>>().Clear();
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());

            ITransaction import = new ImportPositionsTransaction((IObservableHashSetFactory)this.tradingData, this.ef);
            import.Execute();
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Position>>().Count());
        }
    }
}
