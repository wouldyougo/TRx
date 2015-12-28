using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Common.TimeHelpers;
using TRL.Connect.Smartcom.Models;
using TRL.Connect.Smartcom.Handlers;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Test.Handlers
{
    [TestClass]
    public class ConfirmMoveRequestOnOrderMoveSucceededTests
    {
        private IDataContext tradingData;
        private RawTradingDataContext rawData;
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.rawData = new RawTradingDataContext();
            
            this.strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-12.13_FT", 1);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            ConfirmMoveRequestOnOrderMoveSucceeded handler =
                new ConfirmMoveRequestOnOrderMoveSucceeded(this.tradingData, this.rawData, new NullLogger());
        }

        [TestMethod]
        public void ConfirmMoveRequest_on_OrderMoveSucceeded_test()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(signal);
            this.tradingData.Get<ICollection<Order>>().Add(order);

            OrderMoveRequest request = new OrderMoveRequest(order, 151000, 0, "Move order");
            this.tradingData.Get<ICollection<OrderMoveRequest>>().Add(request);

            Assert.IsFalse(request.IsDelivered);

            OrderMoveSucceeded confirmation = new OrderMoveSucceeded(request.OrderId, "588");
            this.rawData.GetData<OrderMoveSucceeded>().Add(confirmation);

            Assert.IsTrue(request.IsDelivered);
        }

        [TestMethod]
        public void ignore_OrderMoveSucceeded_if_request_already_confirmed_test()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(signal);
            this.tradingData.Get<ICollection<Order>>().Add(order);

            OrderMoveRequest request = new OrderMoveRequest(order, 151000, 0, "Move order");
            this.tradingData.Get<ICollection<OrderMoveRequest>>().Add(request);

            Assert.IsFalse(request.IsDelivered);

            OrderMoveSucceeded confirmation = new OrderMoveSucceeded(request.OrderId, "588");
            this.rawData.GetData<OrderMoveSucceeded>().Add(confirmation);

            Assert.IsTrue(request.IsDelivered);

            OrderMoveSucceeded duplicate = new OrderMoveSucceeded(request.OrderId, "588");
            duplicate.DateTime = duplicate.DateTime.AddMilliseconds(1);
            this.rawData.GetData<OrderMoveSucceeded>().Add(duplicate);

            Assert.AreNotEqual(request.DeliveryDate, duplicate.DateTime);
            Assert.AreEqual(request.DeliveryDate, confirmation.DateTime);

        }
    }
}
