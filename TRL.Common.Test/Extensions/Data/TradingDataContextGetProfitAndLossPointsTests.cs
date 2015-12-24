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
    public class TradingDataContextGetProfitAndLossPointsTests:TraderBaseInitializer
    {
        private StrategyHeader strategyHeader, strategy5;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 2);
            this.strategy5 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 5);
        }

        [TestMethod]
        public void TradingDataContext_GetCurrentPositionTradesCounter_returns_one_test()
        {
            Signal signalOpen = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 32000, 0, 0);
            EmulateTradeFor(signalOpen, 32000, 1);
            EmulateTradeFor(signalOpen, 32005, 1);

            Signal signalClose = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 32000, 0, 0);
            EmulateTradeFor(signalClose, 32050, 1);
            EmulateTradeFor(signalClose, 32039, 1);

            Signal signalOpen2 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 32000, 0, 0);
            EmulateTradeFor(signalOpen2);

            Assert.AreEqual(84, this.tradingData.GetProfitAndLossPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContext_GetCurrentPositionTradesCounter_returns_three_test()
        {
            Signal signalOpen = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 32000, 0, 0);
            EmulateTradeFor(signalOpen, 32008, 1);
            EmulateTradeFor(signalOpen, 32010, 1);

            Signal signalClose = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 32000, 0, 0);
            EmulateTradeFor(signalClose, 32000, 1);
            EmulateTradeFor(signalClose, 32000, 1);

            Signal signalOpen2 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 32000, 0, 0);
            EmulateTradeFor(signalOpen2, 32000, 1);
            EmulateTradeFor(signalOpen2, 32000, 1);

            Signal signalClose2 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 32000, 0, 0);
            EmulateTradeFor(signalClose2, 32010, 1);

            Assert.AreEqual(-28, this.tradingData.GetProfitAndLossPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContext_GetProfitAndLostPoints_test()
        {
            Signal signalOpen = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 32000, 0, 0);
            EmulateTradeFor(signalOpen, 32000, 1);
            EmulateTradeFor(signalOpen, 32001, 1);

            Signal signalClose = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 32000, 0, 0);
            EmulateTradeFor(signalClose, 32010, 1);
            EmulateTradeFor(signalClose, 32009, 1);

            Signal signalOpen2 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 32000, 0, 0);
            EmulateTradeFor(signalOpen2, 32000, 1);
            EmulateTradeFor(signalOpen2, 32000, 1);

            Assert.AreEqual(18, this.tradingData.GetProfitAndLossPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContext_partially_closed_long_position_GetProfitAndLossPoints_test()
        {
            Signal openLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Buy, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(openLongSignal);

            Signal closeLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Sell, OrderType.Market, 8058, 0, 0);
            EmulateTradeFor(closeLongSignal);

            Signal openMoreLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Buy, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(openMoreLongSignal);

            Assert.AreEqual(630, this.tradingData.GetProfitAndLossPoints(this.strategy5));
        }

    }
}
