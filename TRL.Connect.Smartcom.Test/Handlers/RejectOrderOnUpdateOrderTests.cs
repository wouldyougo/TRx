using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Data;
using TRL.Configuration;
using TRL.Common.Models;
using TRL.Connect.Smartcom.Models;
using TRL.Connect.Smartcom.Handlers;
using TRL.Common.TimeHelpers;
using TRL.Emulation;
using SmartCOM3Lib;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Test.Handlers
{
    [TestClass]
    public class RejectOrderOnUpdateOrderTests
    {
        private RawTradingDataContext rawData;
        private IDataContext tradingData;
        private StrategyHeader strategyHeader;
        private Signal signal;
        private DateTime rejectedDate;

        [TestInitialize]
        public void Setup()
        {
            this.rawData = new RawTradingDataContext();
            this.tradingData = new TradingDataContext();

            RejectOrderOnUpdateOrder handler =
                new RejectOrderOnUpdateOrder(this.tradingData, this.rawData, new NullLogger());

            this.strategyHeader = new StrategyHeader(1, "01", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 130000, 0, 129900);
            this.rejectedDate = BrokerDateTime.Make(DateTime.Now);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.rawData.GetData<UpdateOrder>().Count);
        }

        [TestMethod]
        public void reject_Order_on_ContragentReject_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update = 
                new UpdateOrder(order.Portfolio, 
                    order.Symbol, 
                    StOrder_State.StOrder_State_ContragentReject, 
                    StOrder_Action.StOrder_Action_Buy, 
                    StOrder_Type.StOrder_Type_Limit, 
                    StOrder_Validity.StOrder_Validity_Day, 
                    150000, 
                    order.Amount, 
                    0, 
                    0, 
                    this.rejectedDate, 
                    "100", 
                    "200", 
                    1, 
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsTrue(order.IsRejected);
            Assert.AreEqual(this.rejectedDate, order.RejectedDate);
        }

        [TestMethod]
        public void reject_Order_on_SystemReject_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update = 
                new UpdateOrder(order.Portfolio, 
                    order.Symbol, 
                    StOrder_State.StOrder_State_SystemReject, 
                    StOrder_Action.StOrder_Action_Buy, 
                    StOrder_Type.StOrder_Type_Limit, 
                    StOrder_Validity.StOrder_Validity_Day, 
                    150000, 
                    order.Amount, 
                    0, 
                    0, 
                    this.rejectedDate, 
                    "100", 
                    "200", 
                    1, 
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsTrue(order.IsRejected);
            Assert.AreEqual(this.rejectedDate, order.RejectedDate);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_wrong_Cookie_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update =
                new UpdateOrder(order.Portfolio,
                    order.Symbol,
                    StOrder_State.StOrder_State_SystemReject,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    order.Amount,
                    0,
                    0,
                    this.rejectedDate,
                    "100",
                    "200",
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsRejected);
        }

        [TestMethod]
        public void ignore_SystemCancel_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update = 
                new UpdateOrder(order.Portfolio, 
                    order.Symbol, 
                    StOrder_State.StOrder_State_SystemCancel, 
                    StOrder_Action.StOrder_Action_Buy, 
                    StOrder_Type.StOrder_Type_Limit, 
                    StOrder_Validity.StOrder_Validity_Day, 
                    150000, 
                    order.Amount, 
                    0, 
                    0, 
                    this.rejectedDate, 
                    "100", 
                    "200", 
                    1, 
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsRejected);
        }

        [TestMethod]
        public void ignore_ContragentCancel_update_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update = 
                new UpdateOrder(order.Portfolio, 
                    order.Symbol, 
                    StOrder_State.StOrder_State_ContragentCancel, 
                    StOrder_Action.StOrder_Action_Buy, 
                    StOrder_Type.StOrder_Type_Limit, 
                    StOrder_Validity.StOrder_Validity_Day, 
                    150000, 
                    order.Amount, 
                    0, 
                    0, this.rejectedDate, 
                    "100", 
                    "200", 
                    1, 
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsRejected);
        }

        [TestMethod]
        public void ignore_Pending_state_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update = 
                new UpdateOrder(order.Portfolio, 
                    order.Symbol, 
                    StOrder_State.StOrder_State_Pending, 
                    StOrder_Action.StOrder_Action_Buy, 
                    StOrder_Type.StOrder_Type_Limit, 
                    StOrder_Validity.StOrder_Validity_Day, 
                    150000, 
                    order.Amount, 
                    0, 
                    0, 
                    this.rejectedDate, 
                    "100", 
                    "200", 
                    1, 
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsRejected);
        }

        [TestMethod]
        public void ignore_Open_state_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update =
                new UpdateOrder(order.Portfolio,
                    order.Symbol,
                    StOrder_State.StOrder_State_Open,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    order.Amount,
                    0,
                    0,
                    this.rejectedDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsRejected);
        }

        [TestMethod]
        public void ignore_Expired_state_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update =
                new UpdateOrder(order.Portfolio,
                    order.Symbol,
                    StOrder_State.StOrder_State_Expired,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    order.Amount,
                    0,
                    0,
                    this.rejectedDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsRejected);
        }

        [TestMethod]
        public void ignore_Cancel_state_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update =
                new UpdateOrder(order.Portfolio,
                    order.Symbol,
                    StOrder_State.StOrder_State_Cancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    order.Amount,
                    0,
                    0,
                    this.rejectedDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsRejected);
        }

        [TestMethod]
        public void ignore_Filled_state_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update =
                new UpdateOrder(order.Portfolio,
                    order.Symbol,
                    StOrder_State.StOrder_State_Filled,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    order.Amount,
                    0,
                    0,
                    this.rejectedDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsRejected);
        }

        [TestMethod]
        public void ignore_Partial_state_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update =
                new UpdateOrder(order.Portfolio,
                    order.Symbol,
                    StOrder_State.StOrder_State_Partial,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    order.Amount,
                    0,
                    0,
                    this.rejectedDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsRejected);
        }

        [TestMethod]
        public void ignore_ContragentCancel_state_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update =
                new UpdateOrder(order.Portfolio,
                    order.Symbol,
                    StOrder_State.StOrder_State_ContragentCancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    order.Amount,
                    0,
                    0,
                    this.rejectedDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsRejected);
        }

        [TestMethod]
        public void ignore_SystemCancel_state_test()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update =
                new UpdateOrder(order.Portfolio,
                    order.Symbol,
                    StOrder_State.StOrder_State_SystemCancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    order.Amount,
                    0,
                    0,
                    this.rejectedDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsRejected);
        }

        [TestMethod]
        public void RejectOrderOnUpdateOrder_Ignore_Duplicate_Updates()
        {
            Order order = this.tradingData.AddSignalAndItsOrder(this.signal);

            UpdateOrder update = 
                new UpdateOrder(order.Portfolio, 
                    order.Symbol, 
                    StOrder_State.StOrder_State_ContragentReject, 
                    StOrder_Action.StOrder_Action_Buy, 
                    StOrder_Type.StOrder_Type_Limit, 
                    StOrder_Validity.StOrder_Validity_Day, 
                    150000, 
                    order.Amount, 
                    0, 
                    0, 
                    this.rejectedDate, 
                    "100", 
                    "200", 
                    1, 
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);
            Assert.IsTrue(order.IsRejected);
            Assert.AreEqual(this.rejectedDate, order.RejectedDate);

            DateTime duplicateDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(1);
            update = new UpdateOrder(order.Portfolio,
                        order.Symbol,
                        StOrder_State.StOrder_State_ContragentReject,
                        StOrder_Action.StOrder_Action_Buy,
                        StOrder_Type.StOrder_Type_Limit,
                        StOrder_Validity.StOrder_Validity_Day,
                        150000,
                        order.Amount,
                        0,
                        0, 
                        duplicateDate,
                        "100",
                        "200",
                        1,
                        order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);
            
            Assert.AreEqual(this.rejectedDate, order.RejectedDate);
            Assert.AreNotEqual(duplicateDate, order.RejectedDate);
        }
    }
}
