using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Models;

namespace TRL.Connect.Smartcom.Test.Models
{
    [TestClass]
    public class TradeInfoTests
    {
        [TestMethod]
        public void TradeInfo_Tests()
        {
            DateTime date = new DateTime(2013, 5, 5, 10, 0, 0);
            TradeInfo trade = new TradeInfo("PRTFL", "RTS-6.13_FT", "234234", 150000, 1, date, "982374");

            Assert.AreEqual("PRTFL", trade.Portfolio);
            Assert.AreEqual("RTS-6.13_FT", trade.Symbol);
            Assert.AreEqual("234234", trade.OrderNo);
            Assert.AreEqual(150000, trade.Price);
            Assert.AreEqual(1, trade.Amount);
            Assert.AreEqual(date, trade.DateTime);
            Assert.AreEqual("982374", trade.TradeNo);
        }
    }
}
