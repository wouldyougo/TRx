using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.TimeHelpers;
using TRL.Common.Extensions.Data;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataContextGetFilledPartiallyTests
    {
        private IDataContext tradingData;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
        }

        [TestMethod]
        public void TradingDataContext_GetFilledPartially_limit_orders()
        {
            StrategyHeader st1 = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 8);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st1);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            o1.FilledAmount = 2;
            Assert.IsFalse(o1.IsFilled);
            Assert.IsTrue(o1.IsFilledPartially);
            Assert.IsFalse(o1.IsCanceled);
            Assert.IsFalse(o1.IsExpired);
            Assert.IsFalse(o1.IsRejected);

            this.tradingData.Get<ICollection<Order>>().Add(o1);

            IEnumerable<Order> unfilled = this.tradingData.GetFilledPartially(st1, OrderType.Limit);

            Assert.AreEqual(1, unfilled.Count());

            Assert.AreSame(unfilled.Last(), o1);

        }

        [TestMethod]
        public void TradingDataContext_GetFilledPartially_market_orders()
        {
            StrategyHeader st1 = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 8);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st1);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            o1.FilledAmount = 2;
            Assert.IsFalse(o1.IsFilled);
            Assert.IsTrue(o1.IsFilledPartially);
            Assert.IsFalse(o1.IsCanceled);
            Assert.IsFalse(o1.IsExpired);
            Assert.IsFalse(o1.IsRejected);

            this.tradingData.Get<ICollection<Order>>().Add(o1);

            IEnumerable<Order> unfilled = this.tradingData.GetFilledPartially(st1, OrderType.Market);

            Assert.AreEqual(1, unfilled.Count());

            Assert.AreSame(unfilled.Last(), o1);

        }

        [TestMethod]
        public void TradingDataContext_GetFilledPartially_stop_orders()
        {
            StrategyHeader st1 = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 8);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st1);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Stop, 150000, 150000, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            o1.FilledAmount = 2;
            Assert.IsFalse(o1.IsFilled);
            Assert.IsTrue(o1.IsFilledPartially);
            Assert.IsFalse(o1.IsCanceled);
            Assert.IsFalse(o1.IsExpired);
            Assert.IsFalse(o1.IsRejected);

            this.tradingData.Get<ICollection<Order>>().Add(o1);

            IEnumerable<Order> unfilled = this.tradingData.GetFilledPartially(st1, OrderType.Stop);

            Assert.AreEqual(1, unfilled.Count());

            Assert.AreSame(unfilled.Last(), o1);

        }

        [TestMethod]
        public void TradingDataContext_GetFilledPartially_returns_null()
        {
            StrategyHeader st1 = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 8);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st1);

            Signal s1 = new Signal(st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Stop, 150000, 150000, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            o1.FilledAmount = 2;
            Assert.IsFalse(o1.IsFilled);
            Assert.IsTrue(o1.IsFilledPartially);
            Assert.IsFalse(o1.IsCanceled);
            Assert.IsFalse(o1.IsExpired);
            Assert.IsFalse(o1.IsRejected);

            this.tradingData.Get<ICollection<Order>>().Add(o1);

            IEnumerable<Order> unfilled = this.tradingData.GetFilledPartially(st1, OrderType.Limit);

            Assert.AreEqual(0, unfilled.Count());

        }
    }
}
