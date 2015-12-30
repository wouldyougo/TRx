using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common;
using TRL.Common.TimeHelpers;
using TRL.Emulation;
using TRL.Logging;

namespace TRx.Trader.BackTest.Test
{
    [TestClass]
    public class OpenPositionOnOrderBookChangeTests:TraderBaseInitializer
    {
        // Стратегия
        private StrategyHeader strategyHeader;

        // Контекст очереди заявок (стакана)
        private OrderBookContext orderBook;

        [TestInitialize]
        public void Setup()
        {
            this.orderBook = new OrderBookContext();
            this.strategyHeader = this.tradingData.Make<StrategyHeader>().Single(s => s.Id == 1);

            OpenPositionOnOrderBookChange handler =
                new OpenPositionOnOrderBookChange(this.strategyHeader,
                    this.orderBook,
                    this.signalQueue,
                    this.tradingData,
                    new NullLogger());
        }
        [TestMethod]        
        public void make_signal_to_open_long_position_test()
        {
            Assert.AreEqual(0, this.tradingData.Make<Signal>().Count);
            Assert.AreEqual(0, this.tradingData.Make<Order>().Count);

            //эмуляция изменения очереди заявок стакана
            this.orderBook.Update(0, this.strategyHeader.Symbol, 149990, 100, 150000, 50);

            Assert.AreEqual(1, this.tradingData.Make<Signal>().Count);
            Assert.AreEqual(1, this.tradingData.Make<Order>().Count);

            Signal signal = this.tradingData.Make<Signal>().Last();

            Assert.AreEqual(this.strategyHeader.Id, signal.StrategyId);
            Assert.AreEqual(TradeAction.Buy, signal.TradeAction);
            Assert.AreEqual(OrderType.Limit, signal.OrderType);
            Assert.AreEqual(149990, signal.Limit);
        }
        [TestMethod]
        public void make_signal_to_open_short_position_if_previous_long_closed_by_stop_test()
        {
            Assert.AreEqual(0, this.tradingData.Make<Signal>().Count);
            Assert.AreEqual(0, this.tradingData.Make<Order>().Count);

            Signal signalToOpen = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signalToOpen);

            Signal signalToClose = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Stop, 149900, 149900, 0);
            EmulateTradeFor(signalToClose);

            this.orderBook.Update(0, this.strategyHeader.Symbol, 149990, 100, 150000, 50);

            Assert.AreEqual(3, this.tradingData.Make<Signal>().Count);
            Assert.AreEqual(3, this.tradingData.Make<Order>().Count);

            Signal signal = this.tradingData.Make<Signal>().Last();

            Assert.AreEqual(this.strategyHeader.Id, signal.StrategyId);
            Assert.AreEqual(TradeAction.Sell, signal.TradeAction);
            Assert.AreEqual(OrderType.Limit, signal.OrderType);
            Assert.AreEqual(150000, signal.Limit);
        }
    }
}
