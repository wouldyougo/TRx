using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common;
using TRL.Emulation;
using TRL.Logging;

namespace TRx.Trader.BackTest.Test
{
    [TestClass]
    public class FollowTrendOnTickTests:TraderBaseInitializer
    {

        private StrategyHeader strategyHeader;
        private OrderBookContext orderBook;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            this.orderBook = new OrderBookContext();
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick("RTS-12.13_FT", new DateTime(2013, 12, 3, 10, 0, 0), 138000, 3600));

            this.orderBook.Update(0, this.strategyHeader.Symbol, 138000, 100, 138010, 50);

            FollowTrendOnTick handler =
                new FollowTrendOnTick(this.strategyHeader, 
                    30,
                    50,
                    this.orderBook, 
                    this.tradingData, 
                    this.signalQueue, 
                    new NullLogger());
        }

        [TestMethod]
        public void open_long_position_on_tick_test()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick("RTS-12.13_FT", new DateTime(2013, 12, 3, 10, 0, 1), 138090, 3000, TradeAction.Sell));

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();
            Assert.AreEqual(TradeAction.Buy, order.TradeAction);
            Assert.AreEqual(OrderType.Limit, order.OrderType);
            Assert.AreEqual(138010, order.Price);
        }

        [TestMethod]
        public void open_short_position_on_tick_test()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick("RTS-12.13_FT", new DateTime(2013, 12, 3, 10, 0, 1), 137950, 3000, TradeAction.Sell));

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();
            Assert.AreEqual(TradeAction.Sell, order.TradeAction);
            Assert.AreEqual(OrderType.Limit, order.OrderType);
            Assert.AreEqual(138000, order.Price);
        }
    }
}
