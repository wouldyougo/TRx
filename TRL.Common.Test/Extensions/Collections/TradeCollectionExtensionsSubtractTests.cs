using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
//using TRL.Common.Extensions.Data;
//using TRL.Common.Extensions.Models;
using TRL.Common.Extensions.Collections;
using TRL.Emulation;

namespace TRL.Common.Extensions.Collections.Test
{
    [TestClass]
    public class TradeCollectionExtensionsSubtractTests:TraderBaseInitializer
    {
        private StrategyHeader strategyHeader;
        private List<Trade> trades;

        [TestInitialize]
        public void Setup()
        {
            this.trades = new List<Trade>();
            this.strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
        }

        [TestMethod]
        public void TradeCollectionExtensions_Combine_clean_entire_collection_test()
        {
            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 138000, 0, 0);
            EmulateTradeFor(signal);
            this.trades.Add(this.tradingData.Get<IEnumerable<Trade>>().Last());
            Assert.AreEqual(1, this.trades.Count);

            Signal cClose = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 139000, 0, 0);
            EmulateTradeFor(cClose);
            Trade lastTrade = this.tradingData.Get<IEnumerable<Trade>>().Last();

            this.trades.Combine(lastTrade);
            Assert.AreEqual(0, this.trades.Count);
        }

        [TestMethod]
        public void TradeCollectionExtensions_Combine_remove_just_first_trade_from_collection_test()
        {
            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 138000, 0, 0);
            EmulateTradeFor(signal);
            
            this.trades.Add(this.tradingData.Get<IEnumerable<Trade>>().Last());
            Assert.AreEqual(1, this.trades.Count);

            Signal s2 = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 138000, 0, 0);
            EmulateTradeFor(s2);

            Trade second = this.tradingData.Get<IEnumerable<Trade>>().Last();
            this.trades.Add(second);
            Assert.AreEqual(2, this.trades.Count);

            Signal cClose = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 139000, 0, 0);
            EmulateTradeFor(cClose);
            Trade lastTrade = this.tradingData.Get<IEnumerable<Trade>>().Last();

            this.trades.Combine(lastTrade);
            Assert.AreEqual(1, this.trades.Count);

            Assert.AreSame(second, this.trades.Last());
        }

        [TestMethod]
        public void TradeCollectionExtensions_Combine_remove_first_three_trades_from_collection_test()
        {
            StrategyHeader strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 4);
            Signal signal = new Signal(strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(signal, 10500, 10);
            EmulateTradeFor(signal, 10500, 5);
            EmulateTradeFor(signal, 10500, 5);

            this.trades.Add(this.tradingData.Get<IEnumerable<Trade>>().Last());
            Assert.AreEqual(1, this.trades.Count);

            Signal s2 = new Signal(strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(s2);

            Trade second = this.tradingData.Get<IEnumerable<Trade>>().Last();
            this.trades.Add(second);
            Assert.AreEqual(2, this.trades.Count);

            Signal cClose = new Signal(strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(cClose);
            Trade lastTrade = this.tradingData.Get<IEnumerable<Trade>>().Last();

            this.trades.Combine(lastTrade);
            Assert.AreEqual(1, this.trades.Count);

            Assert.AreSame(second, this.trades.Last());
        }
    }
}
