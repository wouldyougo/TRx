using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;

namespace TRL.Common.Test.Data
{
    [TestClass]
    public class SymbolDataContextTests
    {
        [TestMethod]
        public void SymbolDataContext_constructor_test()
        {
            SymbolDataContext symbolData = new SymbolDataContext();

            Assert.IsInstanceOfType(symbolData, typeof(IDataContext));
            Assert.IsInstanceOfType(symbolData, typeof(RawBaseDataContext));
        }

        [TestMethod]
        public void SymbolDataContext_contains_collection_of_SymbolSettings_test()
        {
            IDataContext symbolData = new SymbolDataContext();

            Assert.IsNotNull(symbolData.Get<IEnumerable<SymbolSettings>>());
            Assert.AreEqual(0, symbolData.Get<IEnumerable<SymbolSettings>>().Count());
        }

        [TestMethod]
        public void SymbolDataContext_contains_collection_of_SymbolSummary_test()
        {
            IDataContext symbolData = new SymbolDataContext();

            Assert.IsNotNull(symbolData.Get<IEnumerable<SymbolSummary>>());
            Assert.AreEqual(0, symbolData.Get<IEnumerable<SymbolSummary>>().Count());
        }
    }
}
