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
    public class MakeOrderDeliveryConfirmationOnOrderSucceededTests
    {
        private BaseDataContext rawData;
        private IDataContext tradingData;

        [TestInitialize]
        public void Setup()
        {
            this.rawData = new RawTradingDataContext();
            this.tradingData = new TradingDataContext();

            MakeOrderDeliveryConfirmationOnOrderSucceeded handler =
                new MakeOrderDeliveryConfirmationOnOrderSucceeded(this.rawData, this.tradingData, new NullLogger());
        }

        [TestMethod]
        public void SetDeliveryDate_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Sample strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            Signal signal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(signal, 150);
            this.tradingData.Get<ICollection<Order>>().Add(order);
            Assert.IsFalse(order.IsDelivered);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<OrderDeliveryConfirmation>>().Count());
            
            OrderSucceeded os = new OrderSucceeded(order.Id, "12345");
            this.rawData.GetData<OrderSucceeded>().Add(os);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OrderDeliveryConfirmation>>().Count());
            OrderDeliveryConfirmation confirmation = this.tradingData.Get<IEnumerable<OrderDeliveryConfirmation>>().Last();
            Assert.AreEqual(order.Id, confirmation.OrderId);
            Assert.AreEqual(order, confirmation.Order);
            Assert.AreEqual(os.DateTime, confirmation.DateTime);

        }

        [TestMethod]
        public void IgnoreDeliveryDate_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Sample strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            Signal signal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(signal, 150);
            this.tradingData.Get<ICollection<Order>>().Add(order);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<OrderDeliveryConfirmation>>().Count());

            OrderSucceeded os = new OrderSucceeded(5, "12345");
            this.rawData.GetData<OrderSucceeded>().Add(os);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<OrderDeliveryConfirmation>>().Count());
        }
    }
}
