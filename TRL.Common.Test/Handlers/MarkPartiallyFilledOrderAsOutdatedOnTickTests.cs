using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Handlers;
using TRL.Common.Collections;
using TRL.Logging;

namespace TRL.Common.Handlers.Test
{
    [TestClass]
    public class MarkPartiallyFilledOrderAsOutdatedOnTickTests
    {
        private IDataContext tradingData;
        private int outdateSeconds;
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Handlers_Setup()
        {
            this.tradingData = new TradingDataContext();
            this.outdateSeconds = 10;

            this.strategyHeader = new StrategyHeader(1, "Scalping", "BP12345-RF-01", "RTS-9.13_FT", 5);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            UpdatePositionOnTrade updatePosition = new UpdatePositionOnTrade(this.tradingData, new NullLogger());
            MarkPartiallyFilledOrderAsOutdatedOnTick cancelHandler = new MarkPartiallyFilledOrderAsOutdatedOnTick(this.strategyHeader, this.outdateSeconds, this.tradingData, new NullLogger());
        }

        [TestMethod]
        public void Handlers_cancel_partially_filled_outdated_strategy_order_to_open_long()
        {

            Signal signal = new Signal(this.strategyHeader, new DateTime(2013, 1, 1, 10, 0, 0), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(1, new DateTime(2013, 1, 1, 10, 0, 0), this.strategyHeader.Portfolio, this.strategyHeader.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Price, signal.Stop);
            order.Signal = signal;
            order.SignalId = signal.Id;
            this.tradingData.Get<ICollection<Order>>().Add(order);

            Trade t1 = new Trade(order, order.Portfolio, order.Symbol, order.Price, 2, new DateTime(2013, 1, 1, 10, 0, 1));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t1);

            Assert.AreEqual(2, order.FilledAmount);
            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);

            Tick tick = new Tick("RTS-9.13_FT", new DateTime(2013, 1, 1, 10, 0, 11), 150000, 10);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);

            Assert.AreEqual(1, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);
            OrderCancellationRequest request = this.tradingData.Get<ICollection<OrderCancellationRequest>>().Last();

            Assert.AreEqual(order.Id, request.OrderId);
        }

        [TestMethod]
        public void Handlers_cancel_partially_filled_outdated_strategy_order_to_open_short()
        {

            Signal signal = new Signal(this.strategyHeader, new DateTime(2013, 1, 1, 10, 0, 0), TradeAction.Sell, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(1, new DateTime(2013, 1, 1, 10, 0, 0), this.strategyHeader.Portfolio, this.strategyHeader.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Price, signal.Stop);
            order.Signal = signal;
            order.SignalId = signal.Id;
            this.tradingData.Get<ICollection<Order>>().Add(order);

            Trade t1 = new Trade(order, order.Portfolio, order.Symbol, order.Price, -2, new DateTime(2013, 1, 1, 10, 0, 1));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t1);

            Assert.AreEqual(2, order.FilledAmount);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);

            Tick tick = new Tick("RTS-9.13_FT", new DateTime(2013, 1, 1, 10, 0, 11), 150000, 10);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);

            Assert.AreEqual(1, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);
            OrderCancellationRequest request = this.tradingData.Get<ICollection<OrderCancellationRequest>>().Last();

            Assert.AreEqual(order.Id, request.OrderId);
        }

        [TestMethod]
        public void Handlers_ignore_non_filled_non_outdated_strategy_order_to_open_long()
        {

            Signal signal = new Signal(this.strategyHeader, new DateTime(2013, 1, 1, 10, 0, 0), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(1, new DateTime(2013, 1, 1, 10, 0, 0), this.strategyHeader.Portfolio, this.strategyHeader.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Price, signal.Stop);
            order.Signal = signal;
            order.SignalId = signal.Id;
            this.tradingData.Get<ICollection<Order>>().Add(order);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);

            Tick tick = new Tick("RTS-9.13_FT", new DateTime(2013, 1, 1, 10, 0, 8), 150000, 10);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);
        }

        [TestMethod]
        public void Handlers_ingore_non_filled_outdated_strategy_order_to_close_long()
        {

            Signal signal = new Signal(this.strategyHeader, new DateTime(2013, 1, 1, 10, 0, 0), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(1, new DateTime(2013, 1, 1, 10, 0, 0), this.strategyHeader.Portfolio, this.strategyHeader.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Price, signal.Stop);
            order.Signal = signal;
            order.SignalId = signal.Id;
            this.tradingData.Get<ICollection<Order>>().Add(order);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);

            Trade t1 = new Trade(order, order.Portfolio, order.Symbol, 150000, 5, new DateTime(2013, 1, 1, 10, 0, 3));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t1);

            Signal s2 = new Signal(this.strategyHeader, new DateTime(2013, 1, 1, 10, 0, 4), TradeAction.Sell, OrderType.Limit, 150010, 0, 150010);
            this.tradingData.Get<ICollection<Signal>>().Add(s2);

            Order o2 = new Order(2, new DateTime(2013, 1, 1, 10, 0, 4), this.strategyHeader.Portfolio, this.strategyHeader.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Price, signal.Stop);
            o2.Signal = s2;
            o2.SignalId = s2.Id;
            this.tradingData.Get<ICollection<Order>>().Add(o2);

            Tick tick = new Tick("RTS-9.13_FT", new DateTime(2013, 1, 1, 10, 0, 11), 150000, 10);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);
        }

        [TestMethod]
        public void Handlers_ingore_partially_filled_outdated_strategy_order_to_close_long()
        {

            Signal signal = new Signal(this.strategyHeader, new DateTime(2013, 1, 1, 10, 0, 0), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(1, new DateTime(2013, 1, 1, 10, 0, 0), this.strategyHeader.Portfolio, this.strategyHeader.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Price, signal.Stop);
            order.Signal = signal;
            order.SignalId = signal.Id;
            this.tradingData.Get<ICollection<Order>>().Add(order);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);

            Trade t1 = new Trade(order, order.Portfolio, order.Symbol, 150000, 5, new DateTime(2013, 1, 1, 10, 0, 3));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t1);

            Signal s2 = new Signal(this.strategyHeader, new DateTime(2013, 1, 1, 10, 0, 4), TradeAction.Sell, OrderType.Limit, 150010, 0, 150010);
            this.tradingData.Get<ICollection<Signal>>().Add(s2);

            Order o2 = new Order(2, new DateTime(2013, 1, 1, 10, 0, 4), this.strategyHeader.Portfolio, this.strategyHeader.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Price, signal.Stop);
            o2.Signal = s2;
            o2.SignalId = s2.Id;
            this.tradingData.Get<ICollection<Order>>().Add(o2);

            Trade t2 = new Trade(o2, o2.Portfolio, o2.Symbol, o2.Price, -1, new DateTime(2013, 1, 1, 10, 0, 5));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t2);

            Tick tick = new Tick("RTS-9.13_FT", new DateTime(2013, 1, 1, 10, 0, 11), 150000, 10);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);
        }

        [TestMethod]
        public void Handlers_ignore_non_filled_non_outdated_strategy_order_to_open_short()
        {

            Signal signal = new Signal(this.strategyHeader, new DateTime(2013, 1, 1, 10, 0, 0), TradeAction.Sell, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(1, new DateTime(2013, 1, 1, 10, 0, 0), this.strategyHeader.Portfolio, this.strategyHeader.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Price, signal.Stop);
            order.Signal = signal;
            order.SignalId = signal.Id;
            this.tradingData.Get<ICollection<Order>>().Add(order);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);

            Tick tick = new Tick("RTS-9.13_FT", new DateTime(2013, 1, 1, 10, 0, 8), 150000, 10);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);
        }

        [TestMethod]
        public void Handlers_ingore_non_filled_outdated_strategy_order_to_close_short()
        {

            Signal signal = new Signal(this.strategyHeader, new DateTime(2013, 1, 1, 10, 0, 0), TradeAction.Sell, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(1, new DateTime(2013, 1, 1, 10, 0, 0), this.strategyHeader.Portfolio, this.strategyHeader.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Price, signal.Stop);
            order.Signal = signal;
            order.SignalId = signal.Id;
            this.tradingData.Get<ICollection<Order>>().Add(order);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);

            Trade t1 = new Trade(order, order.Portfolio, order.Symbol, 150000, 5, new DateTime(2013, 1, 1, 10, 0, 3));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t1);

            Signal s2 = new Signal(this.strategyHeader, new DateTime(2013, 1, 1, 10, 0, 4), TradeAction.Buy, OrderType.Limit, 150010, 0, 150010);
            this.tradingData.Get<ICollection<Signal>>().Add(s2);

            Order o2 = new Order(2, new DateTime(2013, 1, 1, 10, 0, 4), this.strategyHeader.Portfolio, this.strategyHeader.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Price, signal.Stop);
            o2.Signal = s2;
            o2.SignalId = s2.Id;
            this.tradingData.Get<ICollection<Order>>().Add(o2);

            Tick tick = new Tick("RTS-9.13_FT", new DateTime(2013, 1, 1, 10, 0, 11), 150000, 10);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);
        }

        [TestMethod]
        public void Handlers_ingore_partially_filled_outdated_strategy_order_to_close_short()
        {

            Signal signal = new Signal(this.strategyHeader, new DateTime(2013, 1, 1, 10, 0, 0), TradeAction.Sell, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(1, new DateTime(2013, 1, 1, 10, 0, 0), this.strategyHeader.Portfolio, this.strategyHeader.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Price, signal.Stop);
            order.Signal = signal;
            order.SignalId = signal.Id;
            this.tradingData.Get<ICollection<Order>>().Add(order);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);

            Trade t1 = new Trade(order, order.Portfolio, order.Symbol, 150000, 5, new DateTime(2013, 1, 1, 10, 0, 3));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t1);

            Signal s2 = new Signal(this.strategyHeader, new DateTime(2013, 1, 1, 10, 0, 4), TradeAction.Buy, OrderType.Limit, 150010, 0, 150010);
            this.tradingData.Get<ICollection<Signal>>().Add(s2);

            Order o2 = new Order(2, new DateTime(2013, 1, 1, 10, 0, 4), this.strategyHeader.Portfolio, this.strategyHeader.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Price, signal.Stop);
            o2.Signal = s2;
            o2.SignalId = s2.Id;
            this.tradingData.Get<ICollection<Order>>().Add(o2);

            Trade t2 = new Trade(o2, o2.Portfolio, o2.Symbol, o2.Price, 1, new DateTime(2013, 1, 1, 10, 0, 5));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t2);

            Tick tick = new Tick("RTS-9.13_FT", new DateTime(2013, 1, 1, 10, 0, 11), 150000, 10);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<OrderCancellationRequest>>().Count);
        }


    }
}
