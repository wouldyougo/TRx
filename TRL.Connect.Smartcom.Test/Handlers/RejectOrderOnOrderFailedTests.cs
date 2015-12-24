using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using TRL.Connect.Smartcom.Models;
using TRL.Connect.Smartcom.Handlers;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Test.Handlers
{
    [TestClass]
    public class RejectOrderOnOrderFailedTests
    {
        private IDataContext tradingData;
        private BaseDataContext rawTradingData;
        private StrategyHeader s1, s2, s3;
        private Signal sg1, sg2, sg3;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.rawTradingData = new RawTradingDataContext();

            RejectOrderOnOrderFailed handler = new RejectOrderOnOrderFailed(this.tradingData, this.rawTradingData, new NullLogger());

            this.s1 = new StrategyHeader(1, "01", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.s2 = new StrategyHeader(2, "02", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.s3 = new StrategyHeader(3, "03", "BP12345-RF-01", "Si-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.s1);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.s2);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.s3);

            this.sg1 = new Signal(this.s1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 130000, 0, 129900);
            this.sg2 = new Signal(this.s2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 130000, 0, 0);
            this.sg3 = new Signal(this.s3, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 33000, 0, 32990);
            this.tradingData.Get<ICollection<Signal>>().Add(this.sg1);
            this.tradingData.Get<ICollection<Signal>>().Add(this.sg2);
            this.tradingData.Get<ICollection<Signal>>().Add(this.sg3);
        }

        [TestMethod]
        public void reject_order_test()
        {
            Order order = new Order(this.sg1);
            this.tradingData.Get<ICollection<Order>>().Add(order);
            Assert.IsFalse(order.IsRejected);

            OrderFailed failed = new OrderFailed { OrderId = "111", Reason = "Failed", Cookie = order.Id };
            this.rawTradingData.GetData<OrderFailed>().Add(failed);

            Assert.IsTrue(order.IsRejected);
            Assert.AreEqual("Failed", order.RejectReason);
            Assert.IsTrue(order.RejectedDate <= BrokerDateTime.Make(DateTime.Now));
        }

        [TestMethod]
        public void ignore_duplicate_OrderFailed()
        {
            Order order = new Order(this.sg1);
            this.tradingData.Get<ICollection<Order>>().Add(order);
            Assert.IsFalse(order.IsRejected);

            OrderFailed failed = new OrderFailed { OrderId = "111", Reason = "Failed", Cookie = order.Id };
            this.rawTradingData.GetData<OrderFailed>().Add(failed);

            Assert.IsTrue(order.IsRejected);
            Assert.AreEqual("Failed", order.RejectReason);
            Assert.IsTrue(order.RejectedDate <= BrokerDateTime.Make(DateTime.Now));

            OrderFailed failedTwice = new OrderFailed { OrderId = "111", Reason = "Failed twice", Cookie = order.Id };
            this.rawTradingData.GetData<OrderFailed>().Add(failedTwice);

            Assert.IsTrue(order.IsRejected);
            Assert.AreEqual("Failed", order.RejectReason);
            Assert.IsTrue(order.RejectedDate <= BrokerDateTime.Make(DateTime.Now));
        }
    }
}
