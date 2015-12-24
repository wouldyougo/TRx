using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.TimeHelpers;
using TRL.Common.Collections;
using TRL.Transaction;

namespace TRL.Common.Test
{
    [TestClass]
    public class TickCollectionFactoryTests
    {
        private IDataContext tradingData;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
        }

        [TestMethod]
        public void TickCollectionFactory_Import()
        {
            IGenericFactory<IEnumerable<Tick>> factory = new TickCollectionFactory(ProjectRootFolderNameFactory.Make() + "\\TestData\\SPFB.SBRF_130802_130802.txt");
            
            IEnumerable<Tick> collection = factory.Make();

            Assert.AreEqual(57256, collection.Count());
            Assert.AreEqual(9826, collection.ElementAt(0).Price);
            Assert.AreEqual(9837, collection.ElementAt(28).Price);
            Assert.AreEqual(9798, collection.ElementAt(57254).Price);
            Assert.AreEqual(9795, collection.ElementAt(57255).Price);
        }

        [TestMethod]
        public void Tick_Import_with_assign_symbol_name_test()
        {
            string symbol = "SBRF-3.14_FT";

            ITransaction tickImportTransaction = new ImportTicksTransaction(this.tradingData.Get<ObservableCollection<Tick>>(),
                ProjectRootFolderNameFactory.Make() + "\\TestData\\SPFB.SBRF_130802_130802.txt",
                symbol);

            tickImportTransaction.Execute();

            Assert.AreEqual(57256, this.tradingData.Get<IEnumerable<Tick>>().Count());
            Assert.AreEqual(symbol, this.tradingData.Get<IEnumerable<Tick>>().First().Symbol);
            Assert.AreEqual(9826, this.tradingData.Get<IEnumerable<Tick>>().First().Price);
            Assert.AreEqual(9795, this.tradingData.Get<IEnumerable<Tick>>().Last().Price);
        }

        [TestMethod]
        public void Tick_Import()
        {
            string symbol = "";

            ITransaction tickImportTransaction = new ImportTicksTransaction(this.tradingData.Get<ObservableCollection<Tick>>(),
                ProjectRootFolderNameFactory.Make() + "\\TestData\\SPFB.Si-9.15_150601_150602.txt",
                symbol);

            tickImportTransaction.Execute();

            Assert.AreNotEqual(0, this.tradingData.Get<IEnumerable<Tick>>().Count());
            Assert.AreEqual("SPFB.Si-9.15", this.tradingData.Get<IEnumerable<Tick>>().First().Symbol);
            Assert.AreEqual(54260, this.tradingData.Get<IEnumerable<Tick>>().First().Price);
            Assert.AreEqual(54632, this.tradingData.Get<IEnumerable<Tick>>().Last().Price);
            Assert.AreEqual(9, this.tradingData.Get<IEnumerable<Tick>>().Last().Volume);
            Assert.AreEqual("20150602,234840", this.tradingData.Get<IEnumerable<Tick>>().Last().DateTime.ToString("yyyyMMdd,HHmmss"));
        }
    }
}
