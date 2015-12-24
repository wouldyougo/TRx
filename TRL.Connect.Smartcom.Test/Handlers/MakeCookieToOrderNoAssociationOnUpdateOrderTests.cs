using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Models;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Handlers;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Test.Handlers
{
    [TestClass]
    public class MakeCookieToOrderNoAssociationOnUpdateOrderTests
    {
        private BaseDataContext rawData;
        private int cookie;
        private string orderNo;
        private UpdateOrder filledUpdate, partialUpdate;
        private MakeCookieToOrderNoAssociationOnUpdateOrder handler;

        [TestInitialize]
        public void Setup()
        {
            this.rawData = new RawTradingDataContext();
            this.cookie = 180;
            this.orderNo = "1928347";
            this.filledUpdate =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_Filled,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    "0",
                    this.orderNo,
                    0,
                    this.cookie);

            this.partialUpdate =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_Partial,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    "0",
                    this.orderNo,
                    0,
                    this.cookie);

            this.handler =
                new MakeCookieToOrderNoAssociationOnUpdateOrder(this.rawData, new NullLogger());

            Assert.AreEqual(0, this.rawData.GetData<UpdateOrder>().Count());
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

        [TestMethod]
        public void make_CookieToOrderNoAssociation_for_filled_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledUpdate);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            CookieToOrderNoAssociation expectedTradeInfo = this.rawData.GetData<CookieToOrderNoAssociation>().Last();
            Assert.AreEqual(this.cookie, expectedTradeInfo.Cookie);
            Assert.AreEqual(this.orderNo, expectedTradeInfo.OrderNo);
        }

        [TestMethod]
        public void make_CookieToOrderNoAssociation_for_partial_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.partialUpdate);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            CookieToOrderNoAssociation expectedTradeInfo = this.rawData.GetData<CookieToOrderNoAssociation>().Last();
            Assert.AreEqual(this.cookie, expectedTradeInfo.Cookie);
            Assert.AreEqual(this.orderNo, expectedTradeInfo.OrderNo);
        }

        [TestMethod]
        public void make_CookieToOrderNoAssociation_for_expired_update_test()
        {
            this.partialUpdate =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_Expired,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    "0",
                    this.orderNo,
                    0,
                    this.cookie);

            this.rawData.GetData<UpdateOrder>().Add(this.partialUpdate);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            CookieToOrderNoAssociation expectedTradeInfo = this.rawData.GetData<CookieToOrderNoAssociation>().Last();
            Assert.AreEqual(this.cookie, expectedTradeInfo.Cookie);
            Assert.AreEqual(this.orderNo, expectedTradeInfo.OrderNo);
        }

        [TestMethod]
        public void make_CookieToOrderNoAssociation_for_cancel_update_test()
        {
            this.partialUpdate =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_Cancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    "0",
                    this.orderNo,
                    0,
                    this.cookie);

            this.rawData.GetData<UpdateOrder>().Add(this.partialUpdate);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            CookieToOrderNoAssociation expectedTradeInfo = this.rawData.GetData<CookieToOrderNoAssociation>().Last();
            Assert.AreEqual(this.cookie, expectedTradeInfo.Cookie);
            Assert.AreEqual(this.orderNo, expectedTradeInfo.OrderNo);
        }

        [TestMethod]
        public void make_CookieToOrderNoAssociation_for_SystemCancel_update_test()
        {
            this.partialUpdate =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_SystemCancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    "0",
                    this.orderNo,
                    0,
                    this.cookie);

            this.rawData.GetData<UpdateOrder>().Add(this.partialUpdate);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            CookieToOrderNoAssociation expectedTradeInfo = this.rawData.GetData<CookieToOrderNoAssociation>().Last();
            Assert.AreEqual(this.cookie, expectedTradeInfo.Cookie);
            Assert.AreEqual(this.orderNo, expectedTradeInfo.OrderNo);
        }

        [TestMethod]
        public void make_CookieToOrderNoAssociation_for_ContragentCancel_update_test()
        {
            this.partialUpdate =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_ContragentCancel,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    "0",
                    this.orderNo,
                    0,
                    this.cookie);

            this.rawData.GetData<UpdateOrder>().Add(this.partialUpdate);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            CookieToOrderNoAssociation expectedTradeInfo = this.rawData.GetData<CookieToOrderNoAssociation>().Last();
            Assert.AreEqual(this.cookie, expectedTradeInfo.Cookie);
            Assert.AreEqual(this.orderNo, expectedTradeInfo.OrderNo);
        }

        [TestMethod]
        public void ignore_duplicate_partial_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.partialUpdate);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.rawData.GetData<UpdateOrder>().Add(this.partialUpdate);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

        [TestMethod]
        public void ignore_duplicate_filled_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledUpdate);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.rawData.GetData<UpdateOrder>().Add(this.filledUpdate);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

        [TestMethod]
        public void ignore_filled_UpdateOrder_with_zero_cookie_value_test()
        {
            this.filledUpdate =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_Filled,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    "0",
                    this.orderNo,
                    0,
                    0);

            this.rawData.GetData<UpdateOrder>().Add(this.filledUpdate);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_zero_orderNo_value_test()
        {
            this.partialUpdate =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_Partial,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    "0",
                    "0",
                    0,
                    this.cookie);

            this.rawData.GetData<UpdateOrder>().Add(this.partialUpdate);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_zero_with_SystemReject_state_test()
        {
            this.partialUpdate =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_SystemReject,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    "0",
                    this.orderNo,
                    0,
                    this.cookie);

            this.rawData.GetData<UpdateOrder>().Add(this.partialUpdate);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

        [TestMethod]
        public void ignore_UpdateOrder_with_zero_with_ContragentReject_state_test()
        {
            this.partialUpdate =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_ContragentReject,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    "0",
                    this.orderNo,
                    0,
                    this.cookie);

            this.rawData.GetData<UpdateOrder>().Add(this.partialUpdate);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

    }
}
