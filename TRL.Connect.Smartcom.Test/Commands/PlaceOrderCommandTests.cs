using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Test.Mocks;
using TRL.Common.Models;
using TRL.Connect.Smartcom.Commands;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

/*
namespace TRL.Connect.Smartcom.Test.Commands
{
    [TestClass]
    public class PlaceOrderCommandTests
    {
        private StServerMockSingleton singleton;
        private StServerClassMock stServer;

        [TestInitialize]
        public void Setup()
        {
            this.singleton = new StServerMockSingleton();
            this.stServer = (StServerClassMock)this.singleton.Instance;
        }

        [TestMethod]
        public void PlaceOrderCommand_Test()
        {
            int orders = this.stServer.OrdersPlaced;

            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "Portfolio", "Symbol", TradeAction.Buy, OrderType.Limit, 1, 0, 149000);

            PlaceOrderCommand cmd = new PlaceOrderCommand(order, this.singleton, new NullLogger());

            cmd.Execute();

            Assert.AreEqual(orders + 1, this.stServer.OrdersPlaced);
        }
    }
}
*/