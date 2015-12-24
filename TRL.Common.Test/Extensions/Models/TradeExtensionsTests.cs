using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Extensions.Models;

namespace TRL.Common.Extensions.Models.Test
{
    [TestClass]
    public class TradeExtensionsTests
    {
        [TestMethod]
        public void TradeExtensions_GetTradingResult_positive_for_long_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, 1, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150100, -1, date.AddSeconds(10));

            Assert.AreEqual(100, open.GetTradeResult(close));
        }

        [TestMethod]
        public void TradeExtensions_GetTradingResult_negative_for_long_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, 1, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 149900, -1, date.AddSeconds(10));

            Assert.AreEqual(-100, open.GetTradeResult(close));
        }

        [TestMethod]
        public void TradeExtensions_GetTradingResult_positive_for_short_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, -1, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 149900, 1, date.AddSeconds(10));

            Assert.AreEqual(100, open.GetTradeResult(close));
        }

        [TestMethod]
        public void TradeExtensions_GetTradingResult_negative_for_short_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, -1, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150100, 1, date.AddSeconds(10));

            Assert.AreEqual(-100, open.GetTradeResult(close));
        }

        [TestMethod]
        public void TradeExtensions_GetTradingResult_returns_zero_if_portfolios_are_different_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, -1, date);
            Trade close = new Trade(null, "BP12345-RF-02", "RTS-12.13_FT", 150100, 1, date.AddSeconds(10));

            Assert.AreEqual(0, open.GetTradeResult(close));
        }

        [TestMethod]
        public void TradeExtensions_GetTradingResult_returns_zero_if_symbols_are_different_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, -1, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-3.14_FT", 150100, 1, date.AddSeconds(10));

            Assert.AreEqual(0, open.GetTradeResult(close));
        }

        [TestMethod]
        public void TradeExtensions_GetTradingResult_returns_zero_if_trades_directions_equals_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, -1, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150100, -1, date.AddSeconds(10));

            Assert.AreEqual(0, open.GetTradeResult(close));
        }

        [TestMethod]
        public void TradeExtensions_GetTradingResult_returns_zero_if_open_trade_is_older_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, -1, date.AddSeconds(10));
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150100, 1, date);

            Assert.AreEqual(0, open.GetTradeResult(close));
        }
    }
}
