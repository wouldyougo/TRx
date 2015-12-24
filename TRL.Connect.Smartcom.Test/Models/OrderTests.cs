using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Connect.Smartcom.Data;

namespace TRL.Connect.Smartcom.Test.Models
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void Order_IsOpenPosition()
        {
            RawOrder order = new RawOrder
            {
                Portfolio = "PRTF",
                Symbol = "SBER",
                Action = SmartCOM3Lib.StOrder_Action.StOrder_Action_Buy,
                Type = SmartCOM3Lib.StOrder_Type.StOrder_Type_Market,
                Validity = SmartCOM3Lib.StOrder_Validity.StOrder_Validity_Day,
                Price = 0,
                Amount = 1,
                Stop = 0,
                Cookie = SerialIntegerFactory.Make()
            };

            Assert.IsTrue(order.RequestsOpenPosition());
            Assert.IsFalse(order.RequestsClosePosition());
        }

        [TestMethod]
        public void Order_IsClosePosition()
        {
            RawOrder order = new RawOrder
            {
                Portfolio = "PRTF",
                Symbol = "SBER",
                Action = SmartCOM3Lib.StOrder_Action.StOrder_Action_Sell,
                Type = SmartCOM3Lib.StOrder_Type.StOrder_Type_Market,
                Validity = SmartCOM3Lib.StOrder_Validity.StOrder_Validity_Day,
                Price = 0,
                Amount = 1,
                Stop = 0,
                Cookie = SerialIntegerFactory.Make()
            };

            Assert.IsFalse(order.RequestsOpenPosition());
            Assert.IsTrue(order.RequestsClosePosition());
        }
    }
}
