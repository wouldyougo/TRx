using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common.Collections;
using TRL.Common.TimeHelpers;
using TRL.Common.Test.Mocks;
using TRL.Handlers.Cancel;
using TRL.Emulation;
using TRL.Logging;

namespace TRL.Common.Handlers.Test.Cancel
{
    [TestClass]
    public class MakeOrderCancellationRequestOnQuoteChangeTests : TraderBaseInitializer
    {
        private OrderBookContext quotesProvider;
        private MakeOrderCancellationRequestOnQuote handler;
        private StrategyHeader strategyHeader;
        private double priceShiftPoints;

        [TestInitialize]
        public void Setup()
        {
            this.quotesProvider = new OrderBookContext(3);
            this.strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            this.priceShiftPoints = 50;

            this.handler =
                new MakeOrderCancellationRequestOnQuote(this.strategyHeader,
                    this.priceShiftPoints,
                    this.quotesProvider,
                    this.tradingData,
                    new NullLogger());
        }

        [TestMethod]
        public void MakeOrderCancellationRequest_adds_order_cancellaction_request_for_long()
        {
            MockOrderManager mom = (MockOrderManager)this.orderManager;
            Assert.AreEqual(0, mom.CancelCounter);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());

            Signal signal = new Signal(this.strategyHeader,
                BrokerDateTime.Make(DateTime.Now),
                TradeAction.Buy,
                OrderType.Limit,
                145000,
                0,
                145100);
            this.signalQueue.Enqueue(signal);

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();

            OrderDeliveryConfirmation odc = new OrderDeliveryConfirmation(order, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(odc);

            this.quotesProvider.Update(0, this.strategyHeader.Symbol, 145150, 150, 145160, 150);

            Assert.AreEqual(1, this.tradingData.CancellationRequests.Count());
            Assert.AreEqual(1, mom.CancelCounter);
        }

        [TestMethod]
        public void MakeOrderCancellationRequest_does_not_add_order_cancellaction_request_for_long()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());

            Signal signal = new Signal(this.strategyHeader,
                BrokerDateTime.Make(DateTime.Now),
                TradeAction.Buy,
                OrderType.Limit,
                145000,
                0,
                145100);
            this.signalQueue.Enqueue(signal);

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();

            OrderDeliveryConfirmation odc = new OrderDeliveryConfirmation(order, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(odc);

            this.quotesProvider.Update(0, this.strategyHeader.Symbol, 145140, 150, 145150, 150);

            Assert.AreEqual(0, this.tradingData.CancellationRequests.Count());
        }

        [TestMethod]
        public void MakeOrderCancellationRequest_adds_order_cancellaction_request_for_short()
        {
            MockOrderManager mom = (MockOrderManager)this.orderManager;
            Assert.AreEqual(0, mom.CancelCounter);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());

            Signal signal = new Signal(this.strategyHeader,
                BrokerDateTime.Make(DateTime.Now),
                TradeAction.Sell,
                OrderType.Limit,
                145000,
                0,
                145100);
            this.signalQueue.Enqueue(signal);

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();

            OrderDeliveryConfirmation odc = new OrderDeliveryConfirmation(order, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(odc);

            this.quotesProvider.Update(0, this.strategyHeader.Symbol, 145040, 150, 145050, 150);

            Assert.AreEqual(1, this.tradingData.CancellationRequests.Count());
            Assert.AreEqual(1, mom.CancelCounter);
        }

        [TestMethod]
        public void MakeOrderCancellationRequest_does_not_add_order_cancellaction_request_for_short()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());

            Signal signal = new Signal(this.strategyHeader,
                BrokerDateTime.Make(DateTime.Now),
                TradeAction.Sell,
                OrderType.Limit,
                145000,
                0,
                145100);
            this.signalQueue.Enqueue(signal);

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();

            OrderDeliveryConfirmation odc = new OrderDeliveryConfirmation(order, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(odc);

            this.quotesProvider.Update(0, this.strategyHeader.Symbol, 145050, 150, 145060, 150);

            Assert.AreEqual(0, this.tradingData.CancellationRequests.Count());
        }

        [TestMethod]
        public void MakeOrderCancellationRequest_does_nothing_when_no_unfilled_orders_exists()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());

            this.quotesProvider.Update(0, this.strategyHeader.Symbol, 144990, 150, 145140, 150);

            Assert.AreEqual(0, this.tradingData.CancellationRequests.Count());
        }

        [TestMethod]
        public void make_buy_order_cancellation_request_on_quote_test()
        {
            MockOrderManager mom = (MockOrderManager)this.orderManager;
            Assert.AreEqual(0, mom.CancelCounter);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.CancellationRequests.Count());

            Signal signal = new Signal(this.strategyHeader,
                BrokerDateTime.Make(DateTime.Now),
                TradeAction.Buy,
                OrderType.Limit,
                145000,
                0,
                145100);
            this.signalQueue.Enqueue(signal);

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();

            OrderDeliveryConfirmation odc = new OrderDeliveryConfirmation(order, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(odc);

            this.quotesProvider.Update(0, this.strategyHeader.Symbol, 146000, 150, 146010, 150);

            Assert.AreEqual(1, this.tradingData.CancellationRequests.Count());
            Assert.AreEqual(1, mom.CancelCounter);
        }

        [TestMethod]
        public void make_order_cancellation_request_for_sell_test()
        {
            MockOrderManager mom = (MockOrderManager)this.orderManager;
            Assert.AreEqual(0, mom.CancelCounter);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.CancellationRequests.Count());

            Signal signal = new Signal(this.strategyHeader,
                BrokerDateTime.Make(DateTime.Now),
                TradeAction.Sell,
                OrderType.Limit,
                145000,
                0,
                145100);
            this.signalQueue.Enqueue(signal);

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();

            OrderDeliveryConfirmation odc = new OrderDeliveryConfirmation(order, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(odc);

            this.quotesProvider.Update(0, this.strategyHeader.Symbol, 144800, 150, 144810, 150);

            Assert.AreEqual(1, this.tradingData.CancellationRequests.Count());
            Assert.AreEqual(1, mom.CancelCounter);
        }

        [TestMethod]
        public void repeat_cancellation_request_if_previous_is_older_than_sixty_seconds_for_sell_test()
        {
            MockOrderManager mom = (MockOrderManager)this.orderManager;
            Assert.AreEqual(0, mom.CancelCounter);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.CancellationRequests.Count());

            Signal signal = new Signal(this.strategyHeader,
                BrokerDateTime.Make(DateTime.Now),
                TradeAction.Sell,
                OrderType.Limit,
                145000,
                0,
                145100);
            this.signalQueue.Enqueue(signal);

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();

            OrderCancellationRequest firstRequest = new OrderCancellationRequest(order, "First request");
            firstRequest.DateTime = BrokerDateTime.Make(DateTime.Now.AddSeconds(-61));
            this.tradingData.Get<ICollection<OrderCancellationRequest>>().Add(firstRequest);

            OrderDeliveryConfirmation odc = new OrderDeliveryConfirmation(order, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(odc);

            this.quotesProvider.Update(0, this.strategyHeader.Symbol, 144800, 150, 144810, 150);

            Assert.AreEqual(2, this.tradingData.CancellationRequests.Count());
            Assert.AreEqual(1, mom.CancelCounter);
        }

    }
}
