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
    public class TradingDataContextGetTradesForOrderTests:TraderBaseInitializer
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
        public void TradingDataContext_GetTrades_returns_empty_collection_when_here_is_no_trades_yet()
        {
            Assert.AreEqual(0, this.tradingData.GetTrades(this.strategy1).Count());
        }

        [TestMethod]
        public void TradingDataContext_GetTrades_returns_empty_collection_when_here_is_no_trades_for_order()
        {
            Signal signal = new Signal(this.strategy2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 32000, 0, 0);
            
            this.signalQueue.Enqueue(signal);

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();

            Assert.AreEqual(0, this.tradingData.GetTrades(order).Count());
        }

        [TestMethod]
        public void TradingDataContext_GetTrades_returns_collection_of_trades_for_order()
        {
            Signal signal = new Signal(this.strategy2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 32000, 0, 0);

            EmulateTradeFor(signal, 32001, 1);
            EmulateTradeFor(signal, 32002, 1);

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();

            Assert.AreEqual(2, this.tradingData.GetTrades(order).Count());
        }

        [TestMethod]
        public void TradingDataContext_GetTrades_returns_collection_of_trades_just_for_order()
        {
            Signal signal = new Signal(this.strategy2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 32000, 0, 0);

            EmulateTradeFor(signal, 32001, 1);
            EmulateTradeFor(signal, 32002, 1);

            Signal signalToClose = new Signal(this.strategy2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 32010, 0, 0);
            EmulateTradeFor(signalToClose);

            Order order = this.tradingData.Get<IEnumerable<Order>>().First();

            Assert.AreEqual(2, this.tradingData.GetTrades(order).Count());
        }
    }
}
