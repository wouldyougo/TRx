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
using TRL.Common.Handlers;
using TRL.Emulation;
using TRL.Logging;

namespace TRL.Common.Handlers.Test
{
    [TestClass]
    public class OrderQueueProcessorTests
    {
        private IDataContext tradingData;
        private ObservableQueue<Order> orderQueue;
        private StrategyHeader s1, s2, s3;
        private Signal sg1, sg2, sg3;
        private MockOrderManager manager;

        [TestInitialize]
        public void Handlers_Setup()
        {
            this.tradingData = new TradingDataContext();
            this.orderQueue = new ObservableQueue<Order>();
            this.manager = new MockOrderManager();

            OrderQueueProcessor processor = new OrderQueueProcessor(this.manager, this.tradingData, this.orderQueue, new NullLogger());
            UpdatePositionOnTrade tradeHandler = new UpdatePositionOnTrade(this.tradingData, new NullLogger());
            AddStrategies();

            AddSignals();
        }

        private void AddSignals()
        {
            this.sg1 = new Signal(this.s1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 120000, 0, 0);
            this.sg2 = new Signal(this.s2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 120000, 0, 119900);
            this.sg3 = new Signal(this.s3, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Stop, 120000, 118000, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(this.sg1);
            this.tradingData.Get<ICollection<Signal>>().Add(this.sg2);
            this.tradingData.Get<ICollection<Signal>>().Add(this.sg3);
        }

        private void AddStrategies()
        {
            this.s1 = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.s2 = new StrategyHeader(2, "Strategy 2", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.s3 = new StrategyHeader(3, "Strategy 3", "BP12345-RF-02", "RTS-12.13_FT", 10);

            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.s1);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.s2);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.s3);
        }

        [TestMethod]
        public void Handlers_place_new_long_positioni_open_order()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(0, this.manager.PlaceCounter);

            Order order = new Order(this.sg1);
            this.orderQueue.Enqueue(order);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.manager.PlaceCounter);
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OpenOrder>>().Count());
        }

        [TestMethod]
        public void Handlers_place_new_short_positioni_open_order()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(0, this.manager.PlaceCounter);

            Order order = new Order(this.sg2);
            this.orderQueue.Enqueue(order);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.manager.PlaceCounter);
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OpenOrder>>().Count());
        }

        [TestMethod]
        public void Handlers_place_long_position_open_and_close_orders()
        {
            StrategyHeader strategyHeader = new StrategyHeader(55, "Strategy 55", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            Signal openSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(0, this.manager.PlaceCounter);

            Order order = new Order(openSignal);
            this.orderQueue.Enqueue(order);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.manager.PlaceCounter);
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OpenOrder>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<CloseOrder>>().Count());

            Trade openTrade = new Trade(order, order.Portfolio, order.Symbol, 150010, order.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(openTrade);

            Signal closeSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(closeSignal);

            Order closeOrder = new Order(closeSignal);
            this.orderQueue.Enqueue(closeOrder);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OpenOrder>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<CloseOrder>>().Count());
        }

        [TestMethod]
        public void Handlers_place_long_position_open_and_open_orders()
        {
            StrategyHeader strategyHeader = new StrategyHeader(55, "Strategy 55", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            Signal openSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(0, this.manager.PlaceCounter);

            Order order = new Order(openSignal);
            this.orderQueue.Enqueue(order);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.manager.PlaceCounter);
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OpenOrder>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<CloseOrder>>().Count());

            Trade openTrade = new Trade(order, order.Portfolio, order.Symbol, 150010, order.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(openTrade);

            Signal buyMoreSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(buyMoreSignal);

            Order closeOrder = new Order(buyMoreSignal);
            this.orderQueue.Enqueue(closeOrder);

            Assert.AreEqual(2, this.tradingData.Get<IEnumerable<OpenOrder>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<CloseOrder>>().Count());
        }

        [TestMethod]
        public void Handlers_place_short_position_open_and_open_orders()
        {
            StrategyHeader strategyHeader = new StrategyHeader(55, "Strategy 55", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            Signal openSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(0, this.manager.PlaceCounter);

            Order order = new Order(openSignal);
            this.orderQueue.Enqueue(order);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.manager.PlaceCounter);
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OpenOrder>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<CloseOrder>>().Count());

            Trade openTrade = new Trade(order, order.Portfolio, order.Symbol, 150010, -order.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(openTrade);

            Signal sellMoreSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(sellMoreSignal);

            Order closeOrder = new Order(sellMoreSignal);
            this.orderQueue.Enqueue(closeOrder);

            Assert.AreEqual(2, this.tradingData.Get<IEnumerable<OpenOrder>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<CloseOrder>>().Count());
        }

        [TestMethod]
        public void Handlers_place_short_position_open_and_close_orders()
        {
            StrategyHeader strategyHeader = new StrategyHeader(55, "Strategy 55", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            Signal openSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(0, this.manager.PlaceCounter);

            Order order = new Order(openSignal);
            this.orderQueue.Enqueue(order);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.manager.PlaceCounter);
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OpenOrder>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<CloseOrder>>().Count());

            Trade openTrade = new Trade(order, order.Portfolio, order.Symbol, 150010, -order.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(openTrade);

            Signal closeSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(closeSignal);

            Order closeOrder = new Order(closeSignal);
            this.orderQueue.Enqueue(closeOrder);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OpenOrder>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<CloseOrder>>().Count());
        }

        [TestMethod]
        public void Handlers_ignore_existing_order()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(0, this.manager.PlaceCounter);

            Order order = new Order(this.sg1);
            this.orderQueue.Enqueue(order);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.manager.PlaceCounter);
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OpenOrder>>().Count());

            this.orderQueue.Enqueue(order);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.manager.PlaceCounter);
            Assert.AreEqual(0, this.orderQueue.Count);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OpenOrder>>().Count());
        }
    }
}
