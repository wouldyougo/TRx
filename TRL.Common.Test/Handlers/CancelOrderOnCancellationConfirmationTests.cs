using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using TRL.Common.Handlers;
using TRL.Common.Collections;
using TRL.Logging;

namespace TRL.Common.Handlers.Test
{
    [TestClass]
    public class CancelOrderOnCancellationConfirmationTests
    {
        private IDataContext tradingData;
        private StrategyHeader st1, st2, st3;
        private Signal s1, s2, s3;

        [TestInitialize]
        public void Handlers_Setup()
        {
            this.tradingData = new TradingDataContext();
            CancelOrderOnCancellationConfirmation handler = new CancelOrderOnCancellationConfirmation(this.tradingData, new NullLogger());

            AddStrategies();

            AddSignals();

        }

        private void AddSignals()
        {
            this.s1 = new Signal(this.st1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 130000, 0, 0);
            this.s2 = new Signal(this.st2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 31000, 0, 0);
            this.s3 = new Signal(this.st3, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 131000, 0, 131100);
            this.tradingData.Get<ICollection<Signal>>().Add(this.s1);
            this.tradingData.Get<ICollection<Signal>>().Add(this.s2);
            this.tradingData.Get<ICollection<Signal>>().Add(this.s3);
        }

        private void AddStrategies()
        {
            this.st1 = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.st2 = new StrategyHeader(2, "Strategy 2", "BP12345-RF-01", "Si-9.13_FT", 10);
            this.st3 = new StrategyHeader(3, "Strategy 3", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st1);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st2);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(st3);
        }

        [TestMethod]
        public void Handlers_Cancel_order_on_cancellation_confirmation()
        {
            Order order = new Order(this.s3);
            this.tradingData.Get<ICollection<Order>>().Add(order);

            OrderCancellationRequest request = new OrderCancellationRequest(order, "Cancel order request");
            this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Add(request);
            Assert.IsFalse(order.IsCanceled);

            OrderCancellationConfirmation confirmation = new OrderCancellationConfirmation(order, BrokerDateTime.Make(DateTime.Now), "Order canceled");
            this.tradingData.Get<ObservableHashSet<OrderCancellationConfirmation>>().Add(confirmation);

            Assert.IsTrue(order.IsCanceled);
            Assert.AreEqual(confirmation.DateTime, order.CancellationDate);
            Assert.AreEqual(confirmation.Description, order.CancellationReason);
        }

        [TestMethod]
        public void Handlers_Ignore_cancellation_confirmation_if_order_is_not_exists()
        {
            Order order = new Order(this.s1);

            OrderCancellationRequest request = new OrderCancellationRequest(order, "Cancel order request");
            this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Add(request);
            Assert.IsFalse(order.IsCanceled);

            OrderCancellationConfirmation confirmation = new OrderCancellationConfirmation(order, BrokerDateTime.Make(DateTime.Now), "Order canceled");
            this.tradingData.Get<ObservableHashSet<OrderCancellationConfirmation>>().Add(confirmation);

            Assert.IsFalse(order.IsCanceled);
        }
    }
}
