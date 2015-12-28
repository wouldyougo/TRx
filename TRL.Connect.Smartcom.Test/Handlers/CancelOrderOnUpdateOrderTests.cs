using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using TRL.Connect.Smartcom.Models;
using TRL.Connect.Smartcom.Handlers;
using TRL.Emulation;
using SmartCOM3Lib;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Test.Handlers
{
    [TestClass]
    public class CancelOrderOnUpdateOrderTests
    {
        private RawTradingDataContext rawData;
        private IDataContext tradingData;
        private StrategyHeader strategyHeader;
        private Signal signal;
        private DateTime cancellationDate;

        [TestInitialize]
        public void Setup()
        {
            this.rawData = new RawTradingDataContext();
            this.tradingData = new TradingDataContext();

            CancelOrderOnUpdateOrder handler =
                new CancelOrderOnUpdateOrder(this.tradingData, this.rawData, new NullLogger());

            this.strategyHeader = new StrategyHeader(1, "01", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.signal = 
                new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 130000, 0, 129900);

            this.cancellationDate = DateTime.Now;

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.rawData.GetData<UpdateOrder>().Count);
        }

        [TestMethod]
        public void cancel_order_on_state_cancel()
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
                    cancellationDate, 
                    "100", 
                    "200", 
                    1, 
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsTrue(order.IsCanceled);
            Assert.AreEqual(cancellationDate, order.CancellationDate);
            Assert.AreEqual("StOrder_State_Cancel", order.CancellationReason);
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_wrong_Cookie_test()
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
                    cancellationDate,
                    "100",
                    "200",
                    1,
                    0);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsCanceled);
        }

        [TestMethod]
        public void cancel_order_on_state_system_cancel()
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
                    cancellationDate, 
                    "100", 
                    "200", 
                    1, 
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsTrue(order.IsCanceled);
            Assert.AreEqual(cancellationDate, order.CancellationDate);
            Assert.AreEqual("StOrder_State_SystemCancel", order.CancellationReason);
        }


        [TestMethod]
        public void cancel_order_on_state_contragent_cancel()
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
                    cancellationDate, 
                    "100", 
                    "200", 
                    1, 
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsTrue(order.IsCanceled);
            Assert.AreEqual(cancellationDate, order.CancellationDate);
            Assert.AreEqual("StOrder_State_ContragentCancel", order.CancellationReason);
        }

        [TestMethod]
        public void ignore_state_system_reject()
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
                    this.cancellationDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsCanceled);
        }

        [TestMethod]
        public void ignore_state_contragent_reject()
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
                    this.cancellationDate, 
                    "100", 
                    "200", 
                    1, 
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsCanceled);
        }

        [TestMethod]
        public void ignore_state_pending_test()
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
                    this.cancellationDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsCanceled);
        }

        [TestMethod]
        public void ignore_state_open_test()
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
                    this.cancellationDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsCanceled);
        }

        [TestMethod]
        public void ignore_state_expired_test()
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
                    this.cancellationDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsCanceled);
        }

        [TestMethod]
        public void ignore_state_filled_test()
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
                    this.cancellationDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsCanceled);
        }

        [TestMethod]
        public void ignore_state_partial_test()
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
                    this.cancellationDate,
                    "100",
                    "200",
                    1,
                    order.Id);
            this.rawData.GetData<UpdateOrder>().Add(update);

            Assert.IsFalse(order.IsCanceled);
        }
    }
}
