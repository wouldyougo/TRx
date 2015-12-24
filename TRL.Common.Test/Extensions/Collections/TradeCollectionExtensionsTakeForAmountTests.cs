using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Extensions.Data;
//using TRL.Common.Extensions.Models;
using TRL.Common.Extensions.Collections;
using TRL.Emulation;

namespace TRL.Common.Extensions.Collections.Test
{
    [TestClass]
    public class TradeCollectionExtensionsTakeForAmountTests:TraderBaseInitializer
    {
        private StrategyHeader strategy1;

        [TestInitialize]
        public void Setup()
        {
            this.strategy1 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 4);
        }

        [TestMethod]
        public void TradeCollectionExtensions_TakeForAmount_for_buyTrades_aligned_test()
        {
            Signal first = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(first, 10499, 3);
            EmulateTradeFor(first, 10499, 8);
            EmulateTradeFor(first, 10500, 9);

            Signal second = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 10550, 0, 0);
            EmulateTradeFor(second, 10550, 10);
            EmulateTradeFor(second, 10555, 3);
            EmulateTradeFor(second, 10558, 7);

            Signal third = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 10560, 0, 0);
            EmulateTradeFor(third);

            IEnumerable<Trade> buyTrades = this.tradingData.GetBuyTrades(this.strategy1);

            Assert.AreEqual(40, buyTrades.Sum(t=>Math.Abs(t.Amount)));
            Assert.AreEqual(20, buyTrades.TakeForAmount(20).Sum(t => Math.Abs(t.Amount)));
        }

        [TestMethod]
        public void TradeCollectionExtensions_TakeForAmount_for_buyTrades_shifted_test()
        {
            Signal first = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(first, 10499, 3);
            EmulateTradeFor(first, 10499, 8);
            EmulateTradeFor(first, 10500, 9);

            Signal second = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 10550, 0, 0);
            EmulateTradeFor(second, 10550, 10);
            EmulateTradeFor(second, 10555, 3);
            EmulateTradeFor(second, 10558, 7);

            Signal third = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 10560, 0, 0);
            EmulateTradeFor(third);

            Signal fourth = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 10600, 0, 0);
            EmulateTradeFor(fourth, 10600, 3);

            IEnumerable<Trade> buyTrades = this.tradingData.GetBuyTrades(this.strategy1);

            Assert.AreEqual(40, buyTrades.Sum(t => Math.Abs(t.Amount)));
            Assert.AreEqual(23, buyTrades.TakeForAmount(23).Sum(t => Math.Abs(t.Amount)));
        }

        [TestMethod]
        public void TradeCollectionExtensions_TakeForAmount_for_sell_trades_shifted_test()
        {
            Signal first = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(first, 10499, 3);
            EmulateTradeFor(first, 10499, 8);
            EmulateTradeFor(first, 10500, 9);

            Signal second = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 10550, 0, 0);
            EmulateTradeFor(second, 10550, 10);
            EmulateTradeFor(second, 10555, 3);
            EmulateTradeFor(second, 10558, 7);

            Signal third = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 10560, 0, 0);
            EmulateTradeFor(third);

            Signal fourth = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 10600, 0, 0);
            EmulateTradeFor(fourth, 10600, 3);

            IEnumerable<Trade> sellTrades = this.tradingData.GetSellTrades(this.strategy1);

            Assert.AreEqual(40, sellTrades.Sum(t => Math.Abs(t.Amount)));
            Assert.AreEqual(23, sellTrades.TakeForAmount(23).Sum(t => Math.Abs(t.Amount)));
        }

        [TestMethod]
        public void TradeCollectionExtensions_TakeForAmount_for_sell_trades_aligned_test()
        {
            Signal first = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 10500, 0, 0);
            EmulateTradeFor(first, 10499, 3);
            EmulateTradeFor(first, 10499, 8);
            EmulateTradeFor(first, 10500, 9);

            Signal second = new Signal(this.strategy1, DateTime.Now, TradeAction.Buy, OrderType.Market, 10550, 0, 0);
            EmulateTradeFor(second, 10550, 10);
            EmulateTradeFor(second, 10555, 3);
            EmulateTradeFor(second, 10558, 7);

            Signal third = new Signal(this.strategy1, DateTime.Now, TradeAction.Sell, OrderType.Market, 10560, 0, 0);
            EmulateTradeFor(third);

            IEnumerable<Trade> sellTrades = this.tradingData.GetSellTrades(this.strategy1);

            Assert.AreEqual(40, sellTrades.Sum(t => Math.Abs(t.Amount)));
            Assert.AreEqual(20, sellTrades.TakeForAmount(20).Sum(t => Math.Abs(t.Amount)));
        }
    }
}
