using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Models;
using TRL.Emulation;
using TRL.Common.Extensions;
using System.Collections.Generic;
using TRL.Common.TimeHelpers;
using TRL.Connect.Smartcom.Models;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Handlers;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Test.Handlers
{
    [TestClass]
    public class CancelOrderOnUpdateOrderWithWrongCookieTests
    {
        private IDataContext tradingData;
        private RawTradingDataContext rawData;
        private StrategyHeader strategyHeader;
        private Signal signal;
        private Order order;
        private string orderNo;
        private DateTime cancellationDate;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.rawData = new RawTradingDataContext();

            this.strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-9.14_FT", 5);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Stop, 0, 125000, 0);
            this.order = this.tradingData.AddSignalAndItsOrder(this.signal);

            this.orderNo = "8899000";
            this.cancellationDate = BrokerDateTime.Make(DateTime.Now);
            this.rawData.GetData<CookieToOrderNoAssociation>().Add(new CookieToOrderNoAssociation(this.order.Id, this.orderNo));

            CancelOrderOnUpdateOrderWithWrongCookie handler =
                new CancelOrderOnUpdateOrderWithWrongCookie(this.tradingData, this.rawData, new NullLogger());

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count); 
            Assert.AreEqual(0, this.rawData.GetData<UpdateOrder>().Count);
            Assert.IsFalse(this.order.IsCanceled);
        }

        [TestMethod]
        public void cancel_partially_filled_order_on_state_cancel_test()
        {
            UpdateOrder update =
                new UpdateOrder(this.order.Portfolio,
                    this.order.Symbol,
                    StOrder_State.StOrder_State_Cancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    this.order.Amount,
                    0,
                    1,
                    this.cancellationDate,
                    "100",
                    this.orderNo,
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsTrue(this.order.IsCanceled);
            Assert.AreEqual(this.cancellationDate, this.order.CancellationDate);
            Assert.AreEqual("StOrder_State_Cancel", this.order.CancellationReason);
        }

        [TestMethod]
        public void cancel_order_on_state_cancel_test()
        {
            UpdateOrder update =
                new UpdateOrder(this.order.Portfolio,
                    this.order.Symbol,
                    StOrder_State.StOrder_State_Cancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    this.order.Amount,
                    0,
                    this.order.Amount,
                    this.cancellationDate,
                    this.orderNo,
                    "0",
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsTrue(this.order.IsCanceled);
            Assert.AreEqual(this.cancellationDate, this.order.CancellationDate);
            Assert.AreEqual("StOrder_State_Cancel", this.order.CancellationReason);
        }


        [TestMethod]
        public void cancel_partially_filled_order_on_state_system_cancel_test()
        {
            UpdateOrder update =
                new UpdateOrder(this.order.Portfolio,
                    this.order.Symbol,
                    StOrder_State.StOrder_State_SystemCancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    this.order.Amount,
                    0,
                    1,
                    this.cancellationDate,
                    "100",
                    this.orderNo,
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsTrue(this.order.IsCanceled);
            Assert.AreEqual(this.cancellationDate, this.order.CancellationDate);
            Assert.AreEqual("StOrder_State_SystemCancel", this.order.CancellationReason);
        }

        [TestMethod]
        public void cancel_order_on_state_system_cancel_test()
        {
            UpdateOrder update =
                new UpdateOrder(this.order.Portfolio,
                    this.order.Symbol,
                    StOrder_State.StOrder_State_SystemCancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    this.order.Amount,
                    0,
                    this.order.Amount,
                    this.cancellationDate,
                    this.orderNo,
                    "100",
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsTrue(this.order.IsCanceled);
            Assert.AreEqual(this.cancellationDate, this.order.CancellationDate);
            Assert.AreEqual("StOrder_State_SystemCancel", this.order.CancellationReason);
        }

        [TestMethod]
        public void cancel_partially_filled_order_on_state_contragent_cancel_test()
        {
            UpdateOrder update =
                new UpdateOrder(this.order.Portfolio,
                    this.order.Symbol,
                    StOrder_State.StOrder_State_ContragentCancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    this.order.Amount,
                    0,
                    1,
                    this.cancellationDate,
                    "100",
                    this.orderNo,
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsTrue(this.order.IsCanceled);
            Assert.AreEqual(this.cancellationDate, this.order.CancellationDate);
            Assert.AreEqual("StOrder_State_ContragentCancel", this.order.CancellationReason);
        }

        [TestMethod]
        public void cancel_order_on_state_contragent_cancel_test()
        {
            UpdateOrder update =
                new UpdateOrder(this.order.Portfolio,
                    this.order.Symbol,
                    StOrder_State.StOrder_State_ContragentCancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    this.order.Amount,
                    0,
                    this.order.Amount,
                    this.cancellationDate,
                    this.orderNo,
                    "100",
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsTrue(this.order.IsCanceled);
            Assert.AreEqual(this.cancellationDate, this.order.CancellationDate);
            Assert.AreEqual("StOrder_State_ContragentCancel", this.order.CancellationReason);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_state_pending_test()
        {
            UpdateOrder update =
                new UpdateOrder(this.order.Portfolio,
                    this.order.Symbol,
                    StOrder_State.StOrder_State_Pending,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    this.order.Amount,
                    0,
                    0,
                    this.cancellationDate,
                    "100",
                    this.orderNo,
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(this.order.IsCanceled);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_state_open_test()
        {
            UpdateOrder update =
                new UpdateOrder(this.order.Portfolio,
                    this.order.Symbol,
                    StOrder_State.StOrder_State_Open,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    this.order.Amount,
                    0,
                    0,
                    this.cancellationDate,
                    "100",
                    this.orderNo,
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(this.order.IsCanceled);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_state_expired_test()
        {
            UpdateOrder update =
                new UpdateOrder(this.order.Portfolio,
                    this.order.Symbol,
                    StOrder_State.StOrder_State_Expired,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    this.order.Amount,
                    0,
                    0,
                    this.cancellationDate,
                    "100",
                    this.orderNo,
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(this.order.IsCanceled);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_state_filled_test()
        {
            UpdateOrder update =
                new UpdateOrder(this.order.Portfolio,
                    this.order.Symbol,
                    StOrder_State.StOrder_State_Filled,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    this.order.Amount,
                    0,
                    0,
                    this.cancellationDate,
                    "100",
                    this.orderNo,
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(this.order.IsCanceled);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_state_partial_test()
        {
            UpdateOrder update =
                new UpdateOrder(this.order.Portfolio,
                    this.order.Symbol,
                    StOrder_State.StOrder_State_Partial,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    this.order.Amount,
                    0,
                    0,
                    this.cancellationDate,
                    "100",
                    this.orderNo,
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(this.order.IsCanceled);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_state_system_reject_test()
        {
            UpdateOrder update =
                new UpdateOrder(this.order.Portfolio,
                    this.order.Symbol,
                    StOrder_State.StOrder_State_SystemReject,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Limit,
                    StOrder_Validity.StOrder_Validity_Day,
                    150000,
                    this.order.Amount,
                    0,
                    0,
                    this.cancellationDate,
                    "100",
                    this.orderNo,
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(this.order.IsCanceled);
        }

    }
}
