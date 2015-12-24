using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;

namespace TRL.Common.Test.Data
{
    [TestClass]
    public class SymbolsSummaryTests
    {
        [TestMethod]
        public void SymbolsSummary_Is_Singleton()
        {
            SymbolsSummary d = SymbolsSummary.Instance;
            SymbolsSummary d2 = SymbolsSummary.Instance;

            Assert.AreSame(d, d2);
        }

        [TestMethod]
        public void SymbolsSummary_Inherits_SymbolDataContext()
        {
            Assert.IsInstanceOfType(SymbolsSummary.Instance, typeof(SymbolDataContext));
        }

        [TestMethod]
        public void SymbolsSummary_Is_DataContext()
        {
            Assert.IsTrue(SymbolsSummary.Instance is RawBaseDataContext);
            Assert.IsTrue(SymbolsSummary.Instance is IDataContext);
        }
    }
}
