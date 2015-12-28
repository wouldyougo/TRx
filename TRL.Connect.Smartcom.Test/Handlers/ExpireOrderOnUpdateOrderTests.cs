using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common.Extensions;
using TRL.Emulation;
using TRL.Connect.Smartcom.Models;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Handlers;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Test.Handlers
{
    [TestClass]
    public class ExpireOrderOnUpdateOrderTests
    {
        private IDataContext tradingData;
        private BaseDataContext rawData;

        private StrategyHeader strategyHeader;
        private Signal signal;
        private Order order;

        private ExpireOrderOnUpdateOrder handler;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.rawData = new RawTradingDataContext();

            this.strategyHeader = new StrategyHeader(1, "Strategy", "ST12345-RF-01", "RTS-9.14", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Stop, 121000, 121000, 0);
            this.order = this.tradingData.AddSignalAndItsOrder(this.signal);

            this.handler =
                new ExpireOrderOnUpdateOrder(this.tradingData, this.rawData, new NullLogger());

            Assert.IsFalse(this.order.IsExpired);
        }

        [TestMethod]
        public void mark_order_as_expired_on_UpdateOrder_with_Expired_state_test()
        {
            UpdateOrder updateOrder = 
                new UpdateOrder(this.strategyHeader.Portfolio,
                    this.strategyHeader.Symbol,
                    StOrder_State.StOrder_State_Expired,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    121000,
                    this.strategyHeader.Amount,
                    121000,
                    0,
                    BrokerDateTime.Make(DateTime.Now.AddSeconds(-5)),
                    "1",
                    "2",
                    0,
                    this.order.Id);
            this.rawData.GetData<UpdateOrder>().Add(updateOrder);

            Assert.IsTrue(this.order.IsExpired);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_Expired_state_and_nonexistent_cookie_test()
        {
            UpdateOrder updateOrder =
                new UpdateOrder(this.strategyHeader.Portfolio,
                    this.strategyHeader.Symbol,
                    StOrder_State.StOrder_State_Expired,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    121000,
                    this.strategyHeader.Amount,
                    121000,
                    0,
                    BrokerDateTime.Make(DateTime.Now.AddSeconds(-5)),
                    "1",
                    "2",
                    0,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(updateOrder);

            Assert.IsFalse(this.order.IsExpired);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_Cancel_state_test()
        {
            UpdateOrder updateOrder =
                new UpdateOrder(this.strategyHeader.Portfolio,
                    this.strategyHeader.Symbol,
                    StOrder_State.StOrder_State_Cancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    121000,
                    this.strategyHeader.Amount,
                    121000,
                    0,
                    BrokerDateTime.Make(DateTime.Now.AddSeconds(-5)),
                    "1",
                    "2",
                    0,
                    this.order.Id);
            this.rawData.GetData<UpdateOrder>().Add(updateOrder);

            Assert.IsFalse(this.order.IsExpired);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_ContragentCancel_state_test()
        {
            UpdateOrder updateOrder =
                new UpdateOrder(this.strategyHeader.Portfolio,
                    this.strategyHeader.Symbol,
                    StOrder_State.StOrder_State_ContragentCancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    121000,
                    this.strategyHeader.Amount,
                    121000,
                    0,
                    BrokerDateTime.Make(DateTime.Now.AddSeconds(-5)),
                    "1",
                    "2",
                    0,
                    this.order.Id);
            this.rawData.GetData<UpdateOrder>().Add(updateOrder);

            Assert.IsFalse(this.order.IsExpired);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_ContragentReject_state_test()
        {
            UpdateOrder updateOrder =
                new UpdateOrder(this.strategyHeader.Portfolio,
                    this.strategyHeader.Symbol,
                    StOrder_State.StOrder_State_ContragentReject,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    121000,
                    this.strategyHeader.Amount,
                    121000,
                    0,
                    BrokerDateTime.Make(DateTime.Now.AddSeconds(-5)),
                    "1",
                    "2",
                    0,
                    this.order.Id);
            this.rawData.GetData<UpdateOrder>().Add(updateOrder);

            Assert.IsFalse(this.order.IsExpired);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_Filled_state_test()
        {
            UpdateOrder updateOrder =
                new UpdateOrder(this.strategyHeader.Portfolio,
                    this.strategyHeader.Symbol,
                    StOrder_State.StOrder_State_Filled,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    121000,
                    this.strategyHeader.Amount,
                    121000,
                    0,
                    BrokerDateTime.Make(DateTime.Now.AddSeconds(-5)),
                    "1",
                    "2",
                    0,
                    this.order.Id);
            this.rawData.GetData<UpdateOrder>().Add(updateOrder);

            Assert.IsFalse(this.order.IsExpired);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_Open_state_test()
        {
            UpdateOrder updateOrder =
                new UpdateOrder(this.strategyHeader.Portfolio,
                    this.strategyHeader.Symbol,
                    StOrder_State.StOrder_State_Open,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    121000,
                    this.strategyHeader.Amount,
                    121000,
                    0,
                    BrokerDateTime.Make(DateTime.Now.AddSeconds(-5)),
                    "1",
                    "2",
                    0,
                    this.order.Id);
            this.rawData.GetData<UpdateOrder>().Add(updateOrder);

            Assert.IsFalse(this.order.IsExpired);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_Partial_state_test()
        {
            UpdateOrder updateOrder =
                new UpdateOrder(this.strategyHeader.Portfolio,
                    this.strategyHeader.Symbol,
                    StOrder_State.StOrder_State_Partial,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    121000,
                    this.strategyHeader.Amount,
                    121000,
                    0,
                    BrokerDateTime.Make(DateTime.Now.AddSeconds(-5)),
                    "1",
                    "2",
                    0,
                    this.order.Id);
            this.rawData.GetData<UpdateOrder>().Add(updateOrder);

            Assert.IsFalse(this.order.IsExpired);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_Pending_state_test()
        {
            UpdateOrder updateOrder =
                new UpdateOrder(this.strategyHeader.Portfolio,
                    this.strategyHeader.Symbol,
                    StOrder_State.StOrder_State_Pending,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    121000,
                    this.strategyHeader.Amount,
                    121000,
                    0,
                    BrokerDateTime.Make(DateTime.Now.AddSeconds(-5)),
                    "1",
                    "2",
                    0,
                    this.order.Id);
            this.rawData.GetData<UpdateOrder>().Add(updateOrder);

            Assert.IsFalse(this.order.IsExpired);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_Submited_state_test()
        {
            UpdateOrder updateOrder =
                new UpdateOrder(this.strategyHeader.Portfolio,
                    this.strategyHeader.Symbol,
                    StOrder_State.StOrder_State_Submited,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    121000,
                    this.strategyHeader.Amount,
                    121000,
                    0,
                    BrokerDateTime.Make(DateTime.Now.AddSeconds(-5)),
                    "1",
                    "2",
                    0,
                    this.order.Id);
            this.rawData.GetData<UpdateOrder>().Add(updateOrder);

            Assert.IsFalse(this.order.IsExpired);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_SystemCancel_state_test()
        {
            UpdateOrder updateOrder =
                new UpdateOrder(this.strategyHeader.Portfolio,
                    this.strategyHeader.Symbol,
                    StOrder_State.StOrder_State_SystemCancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    121000,
                    this.strategyHeader.Amount,
                    121000,
                    0,
                    BrokerDateTime.Make(DateTime.Now.AddSeconds(-5)),
                    "1",
                    "2",
                    0,
                    this.order.Id);
            this.rawData.GetData<UpdateOrder>().Add(updateOrder);

            Assert.IsFalse(this.order.IsExpired);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_SystemReject_state_test()
        {
            UpdateOrder updateOrder =
                new UpdateOrder(this.strategyHeader.Portfolio,
                    this.strategyHeader.Symbol,
                    StOrder_State.StOrder_State_SystemReject,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    121000,
                    this.strategyHeader.Amount,
                    121000,
                    0,
                    BrokerDateTime.Make(DateTime.Now.AddSeconds(-5)),
                    "1",
                    "2",
                    0,
                    this.order.Id);
            this.rawData.GetData<UpdateOrder>().Add(updateOrder);

            Assert.IsFalse(this.order.IsExpired);
        }
    }
}
