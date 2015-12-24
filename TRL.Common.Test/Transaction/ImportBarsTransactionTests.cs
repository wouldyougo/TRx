using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using TRL.Common.Collections;
using TRL.Common.Test;

namespace TRL.Transaction.Test
{
    [TestClass]
    public class ImportBarsTransactionTests
    {
        private IDataContext marketData;
        
        [TestInitialize]
        public void Transaction_Setup()
        {
            this.marketData = new TradingDataContext();
        }

        [TestMethod]
        public void Transaction_import_bars_when_file_exists()
        {
            string path = String.Concat(ProjectRootFolderNameFactory.Make(), "\\TestData\\SPFB.RTS_130807_130807.txt");            
            
            Assert.AreEqual(0, this.marketData.Get<ObservableCollection<Bar>>().Count);

            ITransaction import = new ImportBarsTransaction(this.marketData.Get<ObservableCollection<Bar>>(), path);
            import.Execute();

            Assert.AreEqual(7, this.marketData.Get<ObservableCollection<Bar>>().Count);
        }

        [TestMethod]
        public void Transaction_do_nothing_when_file_not_exists()
        {
            ITransaction import = new ImportBarsTransaction(this.marketData.Get<ObservableCollection<Bar>>(), "no-such-file.txt");
            import.Execute();

            Assert.AreEqual(0, this.marketData.Get<ObservableCollection<Bar>>().Count);
        }
    }
}
