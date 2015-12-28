using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Models;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Test.Models
{
    [TestClass]
    public class UpdateOrderTests
    {
        [TestMethod]
        public void UpdateOrder_Constructor()
        {
            DateTime date = BrokerDateTime.Make(DateTime.Now);

            UpdateOrder update = new UpdateOrder("ST30151-RF-01", "RTS-12.12_FT", StOrder_State.StOrder_State_Open, StOrder_Action.StOrder_Action_Buy, StOrder_Type.StOrder_Type_Market, StOrder_Validity.StOrder_Validity_Day, 150000, 1, 0, 0, date, "id", "no", 0, 1);

            Assert.AreEqual("ST30151-RF-01", update.Portfolio);
            Assert.AreEqual("RTS-12.12_FT", update.Symbol);
            Assert.AreEqual(StOrder_State.StOrder_State_Open, update.State);
            Assert.AreEqual(StOrder_Action.StOrder_Action_Buy, update.Action);
            Assert.AreEqual(StOrder_Type.StOrder_Type_Market, update.Type);
            Assert.AreEqual(StOrder_Validity.StOrder_Validity_Day, update.Validity);
            Assert.AreEqual(150000, update.Price);
            Assert.IsInstanceOfType(update.Price, typeof(double));
            Assert.AreEqual(1, update.OrderAmount);
            Assert.IsInstanceOfType(update.OrderAmount, typeof(double));
            Assert.AreEqual(0, update.Stop);
            Assert.IsInstanceOfType(update.Stop, typeof(double));
            Assert.AreEqual(0, update.OrderUnfilled);
            Assert.IsInstanceOfType(update.OrderUnfilled, typeof(double));
            Assert.AreEqual(date, update.Datetime);
            Assert.AreEqual("id", update.OrderId);
            Assert.AreEqual("no", update.OrderNo);
            Assert.AreEqual(0, update.StatusMask);
            Assert.IsInstanceOfType(update.StatusMask, typeof(int));
            Assert.AreEqual(1, update.Cookie);
            Assert.IsInstanceOfType(update.Cookie, typeof(int));
        }
    }
}
