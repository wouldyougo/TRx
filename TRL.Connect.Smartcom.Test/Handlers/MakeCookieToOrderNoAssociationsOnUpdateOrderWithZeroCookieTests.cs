using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Connect.Smartcom.Handlers;
using TRL.Connect.Smartcom.Data;
using SmartCOM3Lib;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Test.Handlers
{
    [TestClass]
    public class MakeCookieToOrderNoAssociationsOnUpdateOrderWithZeroCookieTests
    {
        private BaseDataContext rawData;
        private int cookie;
        private string stopOrderId, stopOrderNo, marketOrderNo;
        private UpdateOrder filledStopOrder, filledMarketOrder;
        private MakeCookieToOrderNoAssociationOnUpdateOrderWithZeroCookie handler;

        [TestInitialize]
        public void Setup()
        {
            this.rawData = new RawTradingDataContext();
            this.cookie = 180;
            this.stopOrderId = "4636001270";
            this.stopOrderNo = "4636001280";
            this.marketOrderNo = "2413543384";
            
            this.filledStopOrder =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_Filled,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Stop,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    this.stopOrderId,
                    this.stopOrderNo,
                    0,
                    this.cookie);

            this.filledMarketOrder =
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
                    this.stopOrderNo,
                    this.marketOrderNo,
                    0,
                    0);

            this.handler =
                new MakeCookieToOrderNoAssociationOnUpdateOrderWithZeroCookie(this.rawData, new NullLogger());

            Assert.AreEqual(0, this.rawData.GetData<UpdateOrder>().Count());
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }


        [TestMethod]
        public void make_CookieToOrderNoAssociation_for_Partial_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledStopOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            CookieToOrderNoAssociation association = this.rawData.GetData<CookieToOrderNoAssociation>().Last();
            Assert.AreEqual(this.cookie, association.Cookie);
            Assert.AreEqual(this.marketOrderNo, association.OrderNo);
        }

        [TestMethod]
        public void make_CookieToOrderNoAssociation_for_SystemCancel_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledStopOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.filledMarketOrder =
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
                    this.stopOrderNo,
                    this.marketOrderNo,
                    0,
                    0);

            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            CookieToOrderNoAssociation association = this.rawData.GetData<CookieToOrderNoAssociation>().Last();
            Assert.AreEqual(this.cookie, association.Cookie);
            Assert.AreEqual(this.marketOrderNo, association.OrderNo);
        }

        [TestMethod]
        public void make_CookieToOrderNoAssociation_for_Cancel_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledStopOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.filledMarketOrder =
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
                    this.stopOrderNo,
                    this.marketOrderNo,
                    0,
                    0);

            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            CookieToOrderNoAssociation association = this.rawData.GetData<CookieToOrderNoAssociation>().Last();
            Assert.AreEqual(this.cookie, association.Cookie);
            Assert.AreEqual(this.marketOrderNo, association.OrderNo);
        }

        [TestMethod]
        public void make_CookieToOrderNoAssociation_for_ContragentCancel_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledStopOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.filledMarketOrder =
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
                    this.stopOrderNo,
                    this.marketOrderNo,
                    0,
                    0);

            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            CookieToOrderNoAssociation association = this.rawData.GetData<CookieToOrderNoAssociation>().Last();
            Assert.AreEqual(this.cookie, association.Cookie);
            Assert.AreEqual(this.marketOrderNo, association.OrderNo);
        }

        [TestMethod]
        public void make_CookieToOrderNoAssociation_for_Expired_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledStopOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.filledMarketOrder =
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
                    this.stopOrderNo,
                    this.marketOrderNo,
                    0,
                    0);

            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            CookieToOrderNoAssociation association = this.rawData.GetData<CookieToOrderNoAssociation>().Last();
            Assert.AreEqual(this.cookie, association.Cookie);
            Assert.AreEqual(this.marketOrderNo, association.OrderNo);
        }

        [TestMethod]
        public void make_CookieToOrderNoAssociation_for_Filled_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledStopOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.filledMarketOrder =
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
                    this.stopOrderNo,
                    this.marketOrderNo,
                    0,
                    0);

            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            CookieToOrderNoAssociation association = this.rawData.GetData<CookieToOrderNoAssociation>().Last();
            Assert.AreEqual(this.cookie, association.Cookie);
            Assert.AreEqual(this.marketOrderNo, association.OrderNo);
        }

        [TestMethod]
        public void ignore_update_with_nonezero_cookie_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledStopOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.filledMarketOrder =
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
                    this.stopOrderNo,
                    this.marketOrderNo,
                    0,
                    1);

            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

        [TestMethod]
        public void ignore_ContragentReject_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledStopOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.filledMarketOrder =
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
                    this.stopOrderNo,
                    this.marketOrderNo,
                    0,
                    0);

            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

        [TestMethod]
        public void ignore_Open_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledStopOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.filledMarketOrder =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_Open,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    this.stopOrderNo,
                    this.marketOrderNo,
                    0,
                    0);

            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

        [TestMethod]
        public void ignore_Pending_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledStopOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.filledMarketOrder =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_Pending,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    this.stopOrderNo,
                    this.marketOrderNo,
                    0,
                    0);

            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

        [TestMethod]
        public void ignore_Submited_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledStopOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.filledMarketOrder =
                new UpdateOrder("BP12354-RF-01",
                    "RTS-9.14_FT",
                    StOrder_State.StOrder_State_Submited,
                    StOrder_Action.StOrder_Action_Buy,
                    StOrder_Type.StOrder_Type_Market,
                    StOrder_Validity.StOrder_Validity_Day,
                    150,
                    10,
                    0,
                    8,
                    DateTime.Now,
                    this.stopOrderNo,
                    this.marketOrderNo,
                    0,
                    0);

            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

        [TestMethod]
        public void ignore_SystemReject_update_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledStopOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());

            this.filledMarketOrder =
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
                    this.stopOrderNo,
                    this.marketOrderNo,
                    0,
                    0);

            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }

        [TestMethod]
        public void ignore_UpdateOrder_when_no_stop_UpdateOrder_exists_test()
        {
            this.rawData.GetData<UpdateOrder>().Add(this.filledMarketOrder);
            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count());
        }
    }
}
