using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Test.Mocks;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Test
{
    [TestClass]
    public class SmartComOrderManagerTests
    {
        private BaseDataContext rawData;
        private StServerMockSingleton singleton;
        private StServerClassMock stServer;
        private SmartComOrderManager manager;

        [TestInitialize]
        public void Setup()
        {
            this.rawData = new RawTradingDataContext();
            this.singleton = new StServerMockSingleton();
            this.stServer = (StServerClassMock)this.singleton.Instance;

            this.manager = new SmartComOrderManager(this.singleton, this.rawData, new NullLogger());
        }

        [TestCleanup]
        public void Teardown()
        {
            this.singleton.Destroy();
        }

        [TestMethod]
        public void OrderManager_PlaceOrder_test()
        {
            int orders = this.stServer.OrdersPlaced;

            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "Portfolio", "Symbol", TradeAction.Buy, OrderType.Limit, 1, 0, 149000);

            this.manager.PlaceOrder(order);

            Assert.AreEqual(orders + 1, this.stServer.OrdersPlaced);
        }

        [TestMethod]
        public void OrderManager_CancelOrder_test()
        {
            int orders = this.stServer.OrdersCanceled;

            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy 1", "PRTFL", "SMBL", 10);

            Signal signal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 149000);

            Order order = new Order(signal);

            UpdateOrder open = new UpdateOrder("PRTFL", "SMBL", SmartCOM3Lib.StOrder_State.StOrder_State_Open, SmartCOM3Lib.StOrder_Action.StOrder_Action_Buy, SmartCOM3Lib.StOrder_Type.StOrder_Type_Market, SmartCOM3Lib.StOrder_Validity.StOrder_Validity_Day, 120000, 1, 0, 1, order.DateTime.AddSeconds(1), "100", "200", 0, order.Id);
            this.rawData.GetData<UpdateOrder>().Add(open);

            this.manager.CancelOrder(order);

            Assert.AreEqual(orders + 1, this.stServer.OrdersCanceled);
        }

        [TestMethod]
        public void OrderManager_do_not_try_to_cancel_order_for_which_no_any_UpdateOrder()
        {
            int orders = this.stServer.OrdersCanceled;
            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "Portfolio", "Symbol", TradeAction.Buy, OrderType.Limit, 1, 0, 149000);

            this.manager.CancelOrder(order);

            Assert.AreEqual(orders, this.stServer.OrdersCanceled);
        }
    }
}
