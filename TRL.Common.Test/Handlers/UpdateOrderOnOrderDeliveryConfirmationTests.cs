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
    public class UpdateOrderOnOrderDeliveryConfirmationTests
    {
        private IDataContext tradingData;
        private StrategyHeader st1, st2, st3;
        private Signal s1, s2, s3;

        [TestInitialize]
        public void Handlers_Setup()
        {
            this.tradingData = new TradingDataContext();
            UpdateOrderOnOrderDeliveryConfirmation handler = new UpdateOrderOnOrderDeliveryConfirmation(this.tradingData, new NullLogger());

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
        public void Handlers_update_order_on_delivery_confirmation()
        {
            Order o = new Order(this.s1);
            Assert.IsFalse(o.IsDelivered);
            this.tradingData.Get<ICollection<Order>>().Add(o);

            OrderDeliveryConfirmation c = new OrderDeliveryConfirmation(o, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(c);
            Assert.IsTrue(o.IsDelivered);
        }

        [TestMethod]
        public void Handlers_do_not_update_order_that_already_delivered()
        {
            Order o = new Order(this.s1);
            Assert.IsFalse(o.IsDelivered);
            this.tradingData.Get<ICollection<Order>>().Add(o);

            DateTime date = BrokerDateTime.Make(DateTime.Now);
            OrderDeliveryConfirmation c = new OrderDeliveryConfirmation(o, date);
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(c);
            Assert.IsTrue(o.IsDelivered);
            Assert.AreEqual(date, o.DeliveryDate);

            OrderDeliveryConfirmation c1 = new OrderDeliveryConfirmation(o, BrokerDateTime.Make(DateTime.Now).AddSeconds(5));
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(c1);
            Assert.AreEqual(date, o.DeliveryDate);
        }
    }
}
