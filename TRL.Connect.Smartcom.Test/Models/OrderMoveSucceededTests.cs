using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Models;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Test.Models
{
    [TestClass]
    public class OrderMoveSucceededTests
    {
        [TestMethod]
        public void OrderMoveSucceeded_constructor_test()
        {
            OrderMoveSucceeded os = new OrderMoveSucceeded(35, "9028347");

            Assert.AreEqual(35, os.Cookie);
            Assert.AreEqual("9028347", os.OrderId);
            Assert.AreEqual(BrokerDateTime.Make(DateTime.Now).Date, os.DateTime.Date);
        }
    }
}
