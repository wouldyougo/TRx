using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Extensions.Data;
using TRL.Common.TimeHelpers;
using TRL.Emulation;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataContextGetAmountTests:TraderBaseInitializer
    {
        private StrategyHeader strategy1, strategy2;

        [TestInitialize]
        public void Setup()
        {
            this.strategy1 = this.tradingData.Get<ICollection<StrategyHeader>>().Single(s => s.Id == 1);
            this.strategy2 = this.tradingData.Get<ICollection<StrategyHeader>>().Single(s => s.Id == 2);
            Assert.IsNotNull(this.strategy1);
        }

        [TestMethod]
        public void TradingDataContext_GetAmount_returns_zero_when_here_is_no_trades_yet()
        {
            Assert.AreEqual(0, this.tradingData.GetAmount(this.strategy1));
        }

        [TestMethod]
        public void TradingDataContext_GetAmount_returns_zero_when_here_is_no_trades_for_strategy()
        {
            Signal signal = 
                new Signal(strategy2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 32000, 0, 0);

            EmulateTradeFor(signal);

            Assert.AreEqual(0, this.tradingData.GetAmount(this.strategy1));
        }

        [TestMethod]
        public void TradingDataContext_GetAmount_returns_positive_double_when_long_position_exists()
        {
            Signal signal = 
                new Signal(this.strategy1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 32000, 0, 0);

            EmulateTradeFor(signal);

            Assert.AreEqual(1, this.tradingData.GetAmount(this.strategy1));
        }

        [TestMethod]
        public void TradingDataContext_GetAmount_returns_negative_double_when_short_position_exists()
        {
            Signal signal = 
                new Signal(this.strategy1, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 32000, 0, 0);

            EmulateTradeFor(signal);

            Assert.AreEqual(-1, this.tradingData.GetAmount(this.strategy1));
        }

        [TestMethod]
        public void TradingDataContext_GetAmount_returns_zero_when_no_any_open_position_exists()
        {
            Signal signal = new Signal(this.strategy1, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 32000, 0, 0);
            EmulateTradeFor(signal);
            Assert.AreEqual(-1, this.tradingData.GetAmount(this.strategy1));

            Signal signalToClose = new Signal(this.strategy1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 31000, 0, 0);
            EmulateTradeFor(signalToClose);
            Assert.AreEqual(0, this.tradingData.GetAmount(this.strategy1));
        }


    }
}
