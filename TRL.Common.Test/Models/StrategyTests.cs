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
    public class StrategyTests
    {
        [TestMethod]
        public void Strategy_constructor_test()
        {
            int id = 1;
            string description = "Вход по пробою. Выход по трейлин стопу в пунктах.";
            string portfolio = "BP12345-RF-01";
            string symbol = "RTS-9.13_FT";
            double amount = 10;
            StrategyHeader s = new StrategyHeader(id, description, portfolio, symbol, amount);

            Assert.AreEqual(id, s.Id);
            Assert.AreEqual(description, s.Description);
            Assert.AreEqual(portfolio, s.Portfolio);
            Assert.AreEqual(symbol, s.Symbol);
            Assert.AreEqual(amount, s.Amount);
        }

        [TestMethod]
        public void Strategy_ToImportString_test()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            StrategyHeader s = new StrategyHeader(1, "First strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);

            string result = String.Format("{0},{1},{2},{3},{4}", s.Id, s.Description, s.Portfolio, s.Symbol, s.Amount.ToString("0.0000", ci));

            Assert.AreEqual(result, s.ToImportString());
        }

        [TestMethod]
        public void Strategy_String_test()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            StrategyHeader s = new StrategyHeader(1, "First strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);

            string result = String.Format("Strategy Id: {0}, Description: {1}, Portfolio: {2}, Symbol: {3}, Amount: {4}", s.Id, s.Description, s.Portfolio, s.Symbol, s.Amount.ToString("0.0000", ci));

            Assert.AreEqual(result, s.ToString());
        }
    }
}
