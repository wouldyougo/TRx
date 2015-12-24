using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using System.Globalization;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class SymbolTests
    {
        [TestMethod]
        public void Symbol_constructor_test()
        {
            DateTime baseDate = DateTime.Now;

            Symbol symbol = new Symbol("RTS-6.13_FT", 1, 6.479, 10, 7623.13, 2, 139210, 150070, baseDate);

            Assert.IsInstanceOfType(symbol, typeof(INamed));
            Assert.IsInstanceOfType(symbol, typeof(IMutable<Symbol>));

            Assert.AreEqual("RTS-6.13_FT", symbol.Name);
            Assert.AreEqual(1, symbol.LotSize);
            Assert.AreEqual(6.479, symbol.StepPrice);
            Assert.AreEqual(10, symbol.Step);
            Assert.IsInstanceOfType(symbol.Step, typeof(double));
            Assert.AreEqual(7623.13, symbol.Margin);
            Assert.AreEqual(2, symbol.TransactionFee);
            Assert.AreEqual(139210, symbol.LowLimit);
            Assert.AreEqual(150070, symbol.HighLimit);
            Assert.AreEqual(baseDate, symbol.ExpirationDate);
        }

        [TestMethod]
        public void Symbol_Update_test()
        {
            DateTime baseDate = DateTime.Now;

            Symbol symbol = new Symbol("RTS-6.13_FT", 1, 6.479, 10, baseDate);

            Assert.AreEqual("RTS-6.13_FT", symbol.Name);
            Assert.AreEqual(1, symbol.LotSize);
            Assert.AreEqual(6.479, symbol.StepPrice);
            Assert.AreEqual(10, symbol.Step);
            Assert.AreEqual(0, symbol.Margin);
            Assert.AreEqual(0, symbol.TransactionFee);
            Assert.AreEqual(0, symbol.LowLimit);
            Assert.AreEqual(0, symbol.HighLimit);
            Assert.AreEqual(baseDate, symbol.ExpirationDate);

            Symbol oneMoreSymbol = new Symbol("RTS-6.13_FT", 1, 6.478, 10, 7623.13, 2, 139210, 150070, baseDate);

            symbol.Update(oneMoreSymbol);

            Assert.AreEqual("RTS-6.13_FT", symbol.Name);
            Assert.AreEqual(1, symbol.LotSize);
            Assert.AreEqual(6.478, symbol.StepPrice);
            Assert.AreEqual(10, symbol.Step);
            Assert.AreEqual(7623.13, symbol.Margin);
            Assert.AreEqual(2, symbol.TransactionFee);
            Assert.AreEqual(139210, symbol.LowLimit);
            Assert.AreEqual(150070, symbol.HighLimit);
            Assert.AreEqual(baseDate, symbol.ExpirationDate);
        }

        [TestMethod]
        public void Symbol_ingore_Update_test()
        {
            DateTime baseDate = DateTime.Now;

            Symbol symbol = new Symbol("RTS-6.13_FT", 1, 6.479, 10, baseDate);

            Assert.AreEqual("RTS-6.13_FT", symbol.Name);
            Assert.AreEqual(1, symbol.LotSize);
            Assert.AreEqual(6.479, symbol.StepPrice);
            Assert.AreEqual(10, symbol.Step);
            Assert.AreEqual(0, symbol.Margin);
            Assert.AreEqual(0, symbol.TransactionFee);
            Assert.AreEqual(0, symbol.LowLimit);
            Assert.AreEqual(0, symbol.HighLimit);
            Assert.AreEqual(baseDate, symbol.ExpirationDate);

            Symbol oneMoreSymbol = new Symbol("RTS-12.13_FT", 1, 6.478, 10, 7623.13, 2, 139210, 150070, baseDate);

            symbol.Update(oneMoreSymbol);

            Assert.AreEqual("RTS-6.13_FT", symbol.Name);
            Assert.AreEqual(1, symbol.LotSize);
            Assert.AreEqual(6.479, symbol.StepPrice);
            Assert.AreEqual(10, symbol.Step);
            Assert.AreEqual(0, symbol.Margin);
            Assert.AreEqual(0, symbol.TransactionFee);
            Assert.AreEqual(0, symbol.LowLimit);
            Assert.AreEqual(0, symbol.HighLimit);
            Assert.AreEqual(baseDate, symbol.ExpirationDate);
        }

        [TestMethod]
        public void Symbol_ToString_test()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            DateTime baseDate = DateTime.Now;

            Symbol symbol = new Symbol("RTS-6.13_FT", 1, 6.479, 10, 7623.13, 2, 139210, 150070, baseDate);

            string result = String.Format("Symbol: {0}, LotSize: {1}, StepPrice: {2}, PriceStep: {3}, Margin: {4}, TransactionFee: {5}, LowLimit: {6}, HighLimit: {7}, ExpirationDate: {8}",
                symbol.Name, 
                symbol.LotSize, 
                symbol.StepPrice.ToString("0.0000", ci), 
                symbol.Step.ToString("0.0000", ci), 
                symbol.Margin.ToString("0.0000", ci),
                symbol.TransactionFee.ToString("0.0000", ci),
                symbol.LowLimit.ToString("0.0000", ci),
                symbol.HighLimit.ToString("0.0000", ci),
                symbol.ExpirationDate.ToString(ci));

            Assert.AreEqual(result, symbol.ToString());
        }

    }
}
