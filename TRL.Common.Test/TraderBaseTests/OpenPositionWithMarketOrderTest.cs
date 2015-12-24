using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Common.TimeHelpers;
using TRL.Common.Test.Mocks;
using TRL.Emulation;
using TRL.Logging;
using TRL.Common.Handlers;

namespace TRL.Common.Test.TraderBaseTests
{
    [TestClass]
    public class OpenPositionWithMarketOrderTest
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ObservableQueue<Order> orderQueue;
        private IOrderManager orderManager;
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();
            this.orderQueue = new ObservableQueue<Order>();
            this.orderManager = new MockOrderManager();

            TraderBase traderBase = new TraderBase(this.tradingData, this.signalQueue, this.orderQueue, this.orderManager, new AlwaysTimeToTradeSchedule(), new NullLogger());

            this.strategyHeader = new StrategyHeader(1, "strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            AddStrategy(this.strategyHeader);
        }

        private void AddStrategy(StrategyHeader item)
        {
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(item);
        }

        [TestMethod]
        public void traderbase_opens_long_position_with_market_order()
        {
            StateBeforeSignalEnqueue();

            EnqueueSignal();

            Order order = StateAfterSignalEnqueue();

            Trade trade = AddTrade(order);

            Position position = StateAfterFirstTradeArrival(order, trade);

            SecondTradeArrival(order, position);
        }

        private void SecondTradeArrival(Order order, Position position)
        {
            Trade fill = new Trade(order, this.strategyHeader.Portfolio, this.strategyHeader.Symbol, 130050, 7, BrokerDateTime.Make(DateTime.Now));
            AddTrade(fill);

            Assert.AreEqual(10, position.Amount);
            Assert.AreEqual(10, order.FilledAmount);
        }

        private Position StateAfterFirstTradeArrival(Order order, Trade trade)
        {
            Assert.IsFalse(order.IsFilled);
            Assert.IsTrue(order.IsFilledPartially);
            Assert.AreEqual(3, order.FilledAmount);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());

            Position position = this.tradingData.Get<IEnumerable<Position>>().Last();

            Assert.AreEqual(this.strategyHeader.Portfolio, position.Portfolio);
            Assert.AreEqual(this.strategyHeader.Symbol, position.Symbol);
            Assert.AreEqual(trade.Amount, position.Amount);
            return position;
        }

        private Trade AddTrade(Order order)
        {
            Trade trade = new Trade(order, this.strategyHeader.Portfolio, this.strategyHeader.Symbol, 130050, 3, BrokerDateTime.Make(DateTime.Now));
            AddTrade(trade);
            return trade;
        }

        private Order StateAfterSignalEnqueue()
        {
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();
            Assert.IsFalse(order.IsFilled);
            Assert.IsFalse(order.IsFilledPartially);
            Assert.AreEqual(0, order.FilledAmount);
            return order;
        }

        private void EnqueueSignal()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 130000, 0, 0);
            EnqueueSignal(signal);
        }

        private void StateBeforeSignalEnqueue()
        {
            Assert.AreEqual(1, this.tradingData.Get<ICollection<StrategyHeader>>().Count);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());
        }

        private void AddTrade(Trade item)
        {
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(item);
        }


        private void EnqueueSignal(Signal item)
        {
            this.signalQueue.Enqueue(item);
        }

        [TestMethod]
        public void traderbase_close_long_position_with_market_order()
        {
            StateBeforeSignalEnqueue();

            EnqueueSignal();

            Order order = StateAfterSignalEnqueue();

            Trade trade = AddTrade(order);

            Position position = StateAfterFirstTradeArrival(order, trade);

            SecondTradeArrival(order, position);

            Signal close = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 132000, 0, 0);
            this.signalQueue.Enqueue(close);

            Assert.AreEqual(1, this.tradingData.Get<ICollection<StrategyHeader>>().Count);
            Assert.AreEqual(2, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(2, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
        }
    }
}
