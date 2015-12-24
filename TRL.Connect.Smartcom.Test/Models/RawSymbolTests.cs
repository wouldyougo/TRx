using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Models;

namespace TRL.Connect.Smartcom.Test.Models
{
    [TestClass]
    public class RawSymbolTests
    {
        [TestMethod]
        public void RawSymbol_constructor_test()
        {
            RawSymbol symbol = new RawSymbol("Symbol", "Symbol short name", "Symbol full name", "Type", 3, 100, 0.5, 10, "EXCH", "Exchange name", new DateTime(2013, 6, 15, 0, 0, 0), 3, 150000);

            Assert.AreEqual("Symbol", symbol.Symbol);
            Assert.AreEqual("Symbol short name", symbol.ShortName);
            Assert.AreEqual("Symbol full name", symbol.LongName);
            Assert.AreEqual("Type", symbol.Type);
            Assert.AreEqual(3, symbol.Decimals);
            Assert.IsInstanceOfType(symbol.Decimals, typeof(int));
            Assert.AreEqual(100, symbol.LotSize);
            Assert.IsInstanceOfType(symbol.LotSize, typeof(int));
            Assert.AreEqual(0.5, symbol.Punkt);
            Assert.AreEqual(10, symbol.Step);
            Assert.AreEqual("EXCH", symbol.SecExtId);
            Assert.AreEqual("Exchange name", symbol.SecExchName);
            Assert.AreEqual(new DateTime(2013, 6, 15, 0, 0, 0), symbol.ExpirationDate);
            Assert.AreEqual(3, symbol.DaysBeforeExpiration);
            Assert.IsInstanceOfType(symbol.DaysBeforeExpiration, typeof(double));
            Assert.AreEqual(150000, symbol.Strike);
            Assert.IsInstanceOfType(symbol.Strike, typeof(double));
        }
    }
}
