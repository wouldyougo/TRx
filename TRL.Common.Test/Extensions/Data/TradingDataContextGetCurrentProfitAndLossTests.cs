using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Extensions.Data;
using System.Collections.Generic;
using TRL.Emulation;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataContextGetCurrentProfitAndLossTests: TraderBaseInitializer
    {
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 4);
        }

        [TestMethod]
        public void TradingDataContext_GetCurrentProfitAndLoss_test()
        {
            Signal s1 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(s1);

            Signal s2 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 10600, 0, 0);
            EmulateTradeFor(s2, 10600, 5);

            this.tradingData.Get<ICollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Buy, 10600, 500));

            Assert.AreEqual(1500, this.tradingData.GetCurrentProfitAndLoss(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContext_GetCurrentProfitAndLoss_when_last_tick_has_another_symbol_than_strategy_test()
        {
            Signal s1 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(s1);

            Signal s2 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 10600, 0, 0);
            EmulateTradeFor(s2, 10600, 5);

            this.tradingData.Get<ICollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Buy, 10600, 500));
            this.tradingData.Get<ICollection<Tick>>().Add(new Tick("AnotherSymbol", DateTime.Now, TradeAction.Buy, 10700, 500));

            Assert.AreEqual(1500, this.tradingData.GetCurrentProfitAndLoss(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContext_GetCurrentProfitAndLoss_multiple_trades_test()
        {
            Signal s1 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(s1, 10500, 2);
            EmulateTradeFor(s1, 10500, 8);
            EmulateTradeFor(s1, 10500, 10);

            Signal s2 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(s2, 10500, 10);
            EmulateTradeFor(s2, 10500, 10);

            Signal s3 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 10600, 0, 0);
            EmulateTradeFor(s3, 10600, 10);

            Signal s4 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 10600, 0, 0);
            EmulateTradeFor(s4, 10600, 10);

            this.tradingData.Get<ICollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Buy, 10600, 500));

            Assert.AreEqual(3000, this.tradingData.GetCurrentProfitAndLoss(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContext_GetCurrentProfitAndLoss_multiple_trades_with_different_ordrer_of_trades_test()
        {
            Signal s1 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(s1, 10500, 10);
            EmulateTradeFor(s1, 10500, 10);

            Signal s2 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(s2, 10500, 4);
            EmulateTradeFor(s2, 10500, 8);
            EmulateTradeFor(s2, 10500, 8);
            
            Signal s3 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 10600, 0, 0);
            EmulateTradeFor(s3, 10600, 10);

            Signal s4 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 10600, 0, 0);
            EmulateTradeFor(s4, 10600, 10);

            this.tradingData.Get<ICollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Buy, 10600, 500));

            Assert.AreEqual(3000, this.tradingData.GetCurrentProfitAndLoss(this.strategyHeader));
        }
    }
}
