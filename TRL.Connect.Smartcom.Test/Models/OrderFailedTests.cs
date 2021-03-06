﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Models;

namespace TRL.Connect.Smartcom.Test.Models
{
    [TestClass]
    public class OrderFailedTests
    {
        [TestMethod]
        public void OrderFailed_constructor_test()
        {
            OrderFailed item = new OrderFailed(100, "255", "Reason");

            Assert.AreEqual(100, item.Cookie);
            Assert.AreEqual("255", item.OrderId);
            Assert.AreEqual("Reason", item.Reason);
        }
    }
}
