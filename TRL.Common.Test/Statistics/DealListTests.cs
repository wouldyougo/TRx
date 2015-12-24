using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Extensions.Models;
using TRL.Common.Statistics;

namespace TRL.Common.Statistics.Test
{
    [TestClass]
    public class DealListTests
    {
        string symbol;
        DealList dealList;

        [TestInitialize]
        public void Setup()
        {
            symbol = "RTS-12.13_FT";
            StrategyHeader sh = new StrategyHeader(1, "1", "1", symbol, 1);
            //dealList = new DealList(symbol);
            dealList = new DealList(sh);
        }

        [TestMethod]
        public void DealList_GetTradingResult_positive_for_long_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, 1, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150100, -1, date.AddSeconds(10));

            Assert.AreEqual(100, open.GetTradeResult(close));

            dealList.OnItemAdded(open);
            dealList.OnItemAdded(close);
            Assert.AreEqual(100, dealList.Sum);

            open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, 2, date);
            close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150100, -2, date.AddSeconds(10));

            dealList.OnItemAdded(open);
            dealList.OnItemAdded(close);
            Assert.AreEqual(300, dealList.Sum);
        }

        [TestMethod]
        public void DealList_GetTradingResult_negative_for_long_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, 1, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 149900, -1, date.AddSeconds(10));

            Assert.AreEqual(-100, open.GetTradeResult(close));

            dealList.OnItemAdded(open);
            dealList.OnItemAdded(close);
            Assert.AreEqual(-100, dealList.Sum);

            open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, 3, date);
            close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 149900, -3, date.AddSeconds(10));

            dealList.OnItemAdded(open);
            dealList.OnItemAdded(close);
            Assert.AreEqual(-400, dealList.Sum);

        }

        [TestMethod]
        public void DealList_GetTradingResult_returns_zero_if_trades_directions_equals_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, -1, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150100, -1, date.AddSeconds(10));

            Assert.AreEqual(0, open.GetTradeResult(close));

            dealList.OnItemAdded(open);
            dealList.OnItemAdded(close);
            Assert.AreEqual(0, dealList.Sum);
        }

        [TestMethod]
        public void DealList_GetTradingResult_returns_zero_if_trades_directions_equals_test2()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, 1, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150100, 1, date.AddSeconds(10));

            Assert.AreEqual(0, open.GetTradeResult(close));

            dealList.OnItemAdded(open);
            dealList.OnItemAdded(close);
            Assert.AreEqual(0, dealList.Sum);
        }


        [TestMethod]
        public void DealList_GetTradingResult_positive_for_short_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, -3, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 149900, 3, date.AddSeconds(10));

            Assert.AreEqual(100, open.GetTradeResult(close));

            dealList.OnItemAdded(open);
            dealList.OnItemAdded(close);
            Assert.AreEqual(300, dealList.Sum);
        }

        [TestMethod]
        public void DealList_GetTradingResult_negative_for_short_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, -2, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150100, 2, date.AddSeconds(10));

            Assert.AreEqual(-100, open.GetTradeResult(close));

            dealList.OnItemAdded(open);
            dealList.OnItemAdded(close);
            Assert.AreEqual(-200, dealList.Sum);
        }

        [TestMethod]
        public void DealList_GetTradingResult_returns_zero_if_portfolios_are_different_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, -1, date);
            Trade close = new Trade(null, "BP12345-RF-02", "RTS-12.13_FT", 150100, 1, date.AddSeconds(10));

            Assert.AreEqual(0, open.GetTradeResult(close));
            dealList.OnItemAdded(open);
            dealList.OnItemAdded(close);
            // переделать проверку на разные портфели
            Assert.AreEqual(0, dealList.Sum);
        }

        [TestMethod]
        public void DealList_GetTradingResult_returns_zero_if_symbols_are_different_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, -1, date);
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-3.14_FT", 150100, 1, date.AddSeconds(10));

            Assert.AreEqual(0, open.GetTradeResult(close));
            dealList.OnItemAdded(open);
            dealList.OnItemAdded(close);
            Assert.AreEqual(0, dealList.Sum);
        }

        [TestMethod]
        public void TradeExtensions_GetTradingResult_returns_zero_if_open_trade_is_older_test()
        {
            DateTime date = DateTime.Now;

            Trade open = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150000, -1, date.AddSeconds(10));
            Trade close = new Trade(null, "BP12345-RF-01", "RTS-12.13_FT", 150100, 1, date);

            Assert.AreEqual(0, open.GetTradeResult(close));

            //Assert.AreEqual(0, dealList.PnL);
            try
            {
                dealList.OnItemAdded(open);
                dealList.OnItemAdded(close);
                Assert.Fail("no exception thrown");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Deal DateTime Exception");
            }
        }
    }
}
