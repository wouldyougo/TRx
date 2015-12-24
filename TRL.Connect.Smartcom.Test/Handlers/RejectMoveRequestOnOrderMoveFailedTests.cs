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
    public class RejectMoveRequestOnOrderMoveFailedTests
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

            RejectMoveRequestOnOrderMoveFailed handler =
                new RejectMoveRequestOnOrderMoveFailed(this.tradingData, this.rawData, new NullLogger());
        }

        [TestMethod]
        public void RejectMoveRequest_on_OrderMoveFailed_test()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(signal);
            this.tradingData.Get<ICollection<Order>>().Add(order);

            OrderMoveRequest request = new OrderMoveRequest(order, 151000, 0, "Move order");
            this.tradingData.Get<ICollection<OrderMoveRequest>>().Add(request);

            Assert.IsFalse(request.IsFailed);

            OrderMoveFailed fault = new OrderMoveFailed(order.Id, "268", "Reason");
            this.rawData.GetData<OrderMoveFailed>().Add(fault);

            Assert.IsTrue(request.IsFailed);
        }

        [TestMethod]
        public void ignore_duplicated_OrderMoveFailed_test()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(signal);
            this.tradingData.Get<ICollection<Order>>().Add(order);

            OrderMoveRequest request = new OrderMoveRequest(order, 151000, 0, "Move order");
            this.tradingData.Get<ICollection<OrderMoveRequest>>().Add(request);

            Assert.IsFalse(request.IsFailed);

            OrderMoveFailed fault = new OrderMoveFailed(order.Id, "268", "Reason");
            this.rawData.GetData<OrderMoveFailed>().Add(fault);

            Assert.IsTrue(request.IsFailed);

            OrderMoveFailed duplicate = new OrderMoveFailed(order.Id, "268", "Reason duplicate");
            this.rawData.GetData<OrderMoveFailed>().Add(duplicate);

            Assert.AreEqual(request.FaultDescription, fault.Reason);
            Assert.AreNotEqual(request.FaultDescription, duplicate.Reason);
        }

    }
}
