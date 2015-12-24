using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.TimeHelpers;
using TRL.Common.Extensions.Data;
using TRL.Emulation;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataContextGetFilledCloseOrdersTests : TraderBaseInitializer
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
        public void TradingDataContext_GetFilledCloseOrders_returns_filled_close_orders_for_strategy()
        {
            Assert.AreEqual(0, this.tradingData.GetCloseOrders(this.strategy1).Count());

            Signal openSignal = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(openSignal);

            Signal closeSignal = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 150010, 0, 0);
            EmulateTradeFor(closeSignal);

            Assert.AreEqual(1, this.tradingData.GetFilledCloseOrders(this.strategy1).Count());
        }


        [TestMethod]
        public void TradingDataContext_GetFilledCloseOrders_doesnt_returns_other_strategy_orders()
        {
            Assert.AreEqual(0, this.tradingData.GetCloseOrders(this.strategy1).Count());

            Signal openSignal = new Signal(this.strategy2, DateTime.Now, TradeAction.Buy, OrderType.Market, 32000, 0, 0);
            EmulateTradeFor(openSignal);

            Signal closeSignal = new Signal(this.strategy2, DateTime.Now, TradeAction.Sell, OrderType.Market, 32001, 0, 0);
            EmulateTradeFor(closeSignal);

            Assert.AreEqual(0, this.tradingData.GetCloseOrders(this.strategy1).Count());
        }
    }
}
