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
    public class TradingDataContextGetFilledOpenOrdersTests : TraderBaseInitializer
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
        public void TradingDataContext_GetFilledOpenOrders_returns_nothing_when_no_trades_for_strategy()
        {
            Assert.AreEqual(0, this.tradingData.GetFilledOpenOrders(this.strategy1).Count());

            Signal signal = 
                new Signal(this.strategy1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.signalQueue.Enqueue(signal);

            Assert.AreEqual(0, this.tradingData.GetFilledOpenOrders(this.strategy1).Count());
        }

        [TestMethod]
        public void TradingDataContext_GetFilledOpenOrders_doesnt_returns_other_strategy_orders()
        {
            Assert.AreEqual(0, this.tradingData.GetFilledOpenOrders(this.strategy1).Count());

            Signal signal = new Signal(this.strategy2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Assert.AreEqual(0, this.tradingData.GetFilledOpenOrders(this.strategy1).Count());
        }

        [TestMethod]
        public void TradingDataContext_GetFilledOpenOrders_doesnt_returns_strategy_close_orders()
        {
            Assert.AreEqual(0, this.tradingData.GetFilledOpenOrders(this.strategy1).Count());

            Signal signal = new Signal(this.strategy1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Signal closeSignal = new Signal(this.strategy1, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 151000, 0, 0);
            EmulateTradeFor(closeSignal);
            
            Assert.AreEqual(1, this.tradingData.GetFilledOpenOrders(this.strategy1).Count());
            Assert.AreEqual(TradeAction.Buy, this.tradingData.GetFilledOpenOrders(this.strategy1).First().TradeAction);
        }

    }
}
