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
    public class TradingDataContextGetProfitAndLossTests:TraderBaseInitializer
    {
        private StrategyHeader strategy1, strategy5;

        [TestInitialize]
        public void Setup()
        {
            this.strategy1 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            this.strategy5 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 5);
        }

        [TestMethod]
        public void TradingDataContext_closed_long_position_with_profit_GetProfitAndLoss_test()
        {
            Signal openSignal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(openSignal);

            Signal closeSignal = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 151000, 0, 0);
            EmulateTradeFor(closeSignal);

            Assert.AreEqual(656.92, Math.Round(this.tradingData.GetProfitAndLoss(this.strategy1), 2));
        }

        [TestMethod]
        public void TradingDataContext_closed_long_position_with_loss_GetProfitAndLoss_test()
        {
            Signal openSignal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(openSignal);

            Signal closeSignal = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 149000, 0, 0);
            EmulateTradeFor(closeSignal);

            Assert.AreEqual(-656.92, Math.Round(this.tradingData.GetProfitAndLoss(this.strategy1), 2));
        }

        [TestMethod]
        public void TradingDataContext_closed_long_and_open_one_more_with_profit_GetProfitAndLoss_test()
        {
            Signal openLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Buy, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(openLongSignal);

            Signal closeLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Sell, OrderType.Market, 8058, 0, 0);
            EmulateTradeFor(closeLongSignal);

            Signal openMoreLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Buy, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(openMoreLongSignal);

            Assert.AreEqual(630, this.tradingData.GetProfitAndLoss(this.strategy5));
        }

        [TestMethod]
        public void TradingDataContext_partially_closed_and_partially_closed_long_positions_with_profit_GetProfitAndLoss_test()
        {
            Signal openLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Buy, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(openLongSignal);

            Signal closeLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Sell, OrderType.Market, 8058, 0, 0);
            EmulateTradeFor(closeLongSignal);

            Signal openMoreLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Buy, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(openMoreLongSignal);

            Signal closeMoreLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Sell, OrderType.Market, 8058, 0, 0);
            EmulateTradeFor(closeMoreLongSignal, 8058, 10);

            Assert.AreEqual(930, this.tradingData.GetProfitAndLoss(this.strategy5));
        }

        [TestMethod]
        public void TradingDataContext_closed_short_position_with_profit_GetProfitAndLoss_test()
        {
            Signal openSignal = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(openSignal);

            Signal closeSignal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 149000, 0, 0);
            EmulateTradeFor(closeSignal);

            Assert.AreEqual(656.92, Math.Round(this.tradingData.GetProfitAndLoss(this.strategy1), 2));
        }

        [TestMethod]
        public void TradingDataContext_closed_short_position_with_loss_GetProfitAndLoss_test()
        {
            Signal openSignal = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(openSignal);

            Signal closeSignal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 151000, 0, 0);
            EmulateTradeFor(closeSignal);

            Assert.AreEqual(-656.92, Math.Round(this.tradingData.GetProfitAndLoss(this.strategy1), 2));
        }

        [TestMethod]
        public void TradingDataContext_closed_short_and_open_one_more_with_profit_GetProfitAndLoss_test()
        {
            Signal openLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Sell, OrderType.Market, 8058, 0, 0);
            EmulateTradeFor(openLongSignal);

            Signal closeLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Buy, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(closeLongSignal);

            Signal openMoreLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Sell, OrderType.Market, 8058, 0, 0);
            EmulateTradeFor(openMoreLongSignal);

            Assert.AreEqual(630, this.tradingData.GetProfitAndLoss(this.strategy5));
        }

        [TestMethod]
        public void TradingDataContext_closed_short_and_open_one_more_with_loss_GetProfitAndLoss_test()
        {
            Signal openLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Sell, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(openLongSignal);

            Signal closeLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Buy, OrderType.Market, 8058, 0, 0);
            EmulateTradeFor(closeLongSignal);

            Signal openMoreLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Sell, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(openMoreLongSignal);

            Assert.AreEqual(-630, this.tradingData.GetProfitAndLoss(this.strategy5));
        }

        [TestMethod]
        public void TradingDataContext_partially_closed_and_partially_closed_short_positions_with_profit_GetProfitAndLoss_test()
        {
            Signal openLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Sell, OrderType.Market, 8058, 0, 0);
            EmulateTradeFor(openLongSignal);

            Signal closeLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Buy, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(closeLongSignal);

            Signal openMoreLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Sell, OrderType.Market, 8058, 0, 0);
            EmulateTradeFor(openMoreLongSignal);

            Signal closeMoreLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Buy, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(closeMoreLongSignal, 8028, 10);

            Assert.AreEqual(930, this.tradingData.GetProfitAndLoss(this.strategy5));
        }

        [TestMethod]
        public void TradingDataContext_partially_closed_and_partially_closed_short_positions_with_loss_GetProfitAndLoss_test()
        {
            Signal openLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Sell, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(openLongSignal);

            Signal closeLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Buy, OrderType.Market, 8058, 0, 0);
            EmulateTradeFor(closeLongSignal);

            Signal openMoreLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Sell, OrderType.Market, 8028, 0, 0);
            EmulateTradeFor(openMoreLongSignal);

            Signal closeMoreLongSignal = new Signal(this.strategy5, DateTime.Now, TradeAction.Buy, OrderType.Market, 8058, 0, 0);
            EmulateTradeFor(closeMoreLongSignal, 8058, 10);

            Assert.AreEqual(-930, this.tradingData.GetProfitAndLoss(this.strategy5));
        }

    }
}
