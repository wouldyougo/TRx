using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
//using TRL.Common.Extensions;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Emulation;

namespace TRx.Trader.Converter.Test
{
    [TestClass]
    public class TradingDataContextTradesTests
    {
        private IDataContext tradingData;
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.strategyHeader = new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS-3.14_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

        }

        [TestMethod]
        public void GetAllStrategyTrades_for_long_position_test()
        {
            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(signal, 150010, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(signal, 150000, 4);
            this.tradingData.AddSignalAndItsOrderAndTrade(signal, 149990, 3);

            Assert.AreEqual(3, this.tradingData.GetTrades(this.strategyHeader).Count());
        }

        [TestMethod]
        public void Get_All_Order_Trades_for_short_position_test()
        {
            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            
            Order order = this.tradingData.AddSignalAndItsOrderAndTrade(signal, 150010, 3).Order;

            this.tradingData.AddSignalAndItsOrderAndTrade(signal, 150000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(signal, 149990, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(signal, 149980, 1);

            Assert.AreEqual(4, this.tradingData.GetTrades(order).Count());
        }
    }
}
