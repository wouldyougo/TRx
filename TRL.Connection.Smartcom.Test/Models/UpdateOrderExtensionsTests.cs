using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Models;
using SmartCOM3Lib;

namespace TRL.Connect.Smartcom.Test.Models
{
    [TestClass]
    public class UpdateOrderExtensionsTests
    {
        [TestMethod]
        public void Partial_CanContainFillingMark_test()
        {
            UpdateOrder updateOrder =
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
                    0);
            Assert.IsTrue(updateOrder.CanContainFillingMark());
        }

        [TestMethod]
        public void Filled_CanContainFillingMark_test()
        {
            UpdateOrder updateOrder =
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
                    "0",
                    0,
                    0);
            Assert.IsTrue(updateOrder.CanContainFillingMark());
        }

        [TestMethod]
        public void Cancel_CanContainFillingMark_test()
        {
            UpdateOrder updateOrder =
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
                    "0",
                    0,
                    0);
            Assert.IsTrue(updateOrder.CanContainFillingMark());
        }

        [TestMethod]
        public void ContragentCancel_CanContainFillingMark_test()
        {
            UpdateOrder updateOrder =
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
                    "0",
                    0,
                    0);
            Assert.IsTrue(updateOrder.CanContainFillingMark());
        }

        [TestMethod]
        public void Expired_CanContainFillingMark_test()
        {
            UpdateOrder updateOrder =
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
                    "0",
                    0,
                    0);
            Assert.IsTrue(updateOrder.CanContainFillingMark());
        }

        [TestMethod]
        public void ContragentReject_not_CanContainFillingMark_test()
        {
            UpdateOrder updateOrder =
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
                    "0",
                    0,
                    0);
            Assert.IsFalse(updateOrder.CanContainFillingMark());
        }

        [TestMethod]
        public void Open_not_CanContainFillingMark_test()
        {
            UpdateOrder updateOrder =
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
                    "0",
                    "0",
                    0,
                    0);
            Assert.IsFalse(updateOrder.CanContainFillingMark());
        }

        [TestMethod]
        public void Pending_not_CanContainFillingMark_test()
        {
            UpdateOrder updateOrder =
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
                    "0",
                    "0",
                    0,
                    0);
            Assert.IsFalse(updateOrder.CanContainFillingMark());
        }

        [TestMethod]
        public void Submited_not_CanContainFillingMark_test()
        {
            UpdateOrder updateOrder =
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
                    "0",
                    "0",
                    0,
                    0);
            Assert.IsFalse(updateOrder.CanContainFillingMark());
        }

        [TestMethod]
        public void SystemCancel_CanContainFillingMark_test()
        {
            UpdateOrder updateOrder =
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
                    "0",
                    0,
                    0);
            Assert.IsTrue(updateOrder.CanContainFillingMark());
        }

        [TestMethod]
        public void SystemReject_not_CanContainFillingMark_test()
        {
            UpdateOrder updateOrder =
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
                    "0",
                    0,
                    0);
            Assert.IsFalse(updateOrder.CanContainFillingMark());
        }
    }
}
