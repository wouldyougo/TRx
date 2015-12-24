using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class BidAskTests
    {
        [TestMethod]
        public void BidAsk_Constructor_test()
        {
            DateTime date = BrokerDateTime.Make(DateTime.Now);

            BidAsk item = new BidAsk(1, date, "RTS-9.13_FT", 0, 10, 150, 10, 151, 15);

            Assert.AreEqual(1, item.Id);
            Assert.AreEqual(date, item.DateTime);
            Assert.AreEqual("RTS-9.13_FT", item.Symbol);
            Assert.AreEqual(0, item.Row);
            Assert.AreEqual(10, item.NRows);
            Assert.AreEqual(150, item.Bid);
            Assert.AreEqual(10, item.BidSize);
            Assert.AreEqual(151, item.Ask);
            Assert.AreEqual(15, item.AskSize);
        }
    }
}
