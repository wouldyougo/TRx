using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Extensions.Data;
using TRL.Emulation;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataContextIsClosingTradeTests:TraderBaseInitializer
    {
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
        }

        [TestMethod]
        public void first_trade_is_closing_test()
        {
            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Trade trade = this.tradingData.Get<IEnumerable<Trade>>().Last();

            Assert.IsFalse(this.tradingData.IsClosing(trade));
        }

        [TestMethod]
        public void last_trade_is_not_closing_trade_test()
        {
            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Signal close = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 149000, 0, 0);
            EmulateTradeFor(close);

            Trade trade = this.tradingData.Get<IEnumerable<Trade>>().Last();

            Assert.IsTrue(this.tradingData.IsClosing(trade));
        }
    }
}
