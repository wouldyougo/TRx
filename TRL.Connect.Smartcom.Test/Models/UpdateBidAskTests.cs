using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Models;

namespace TRL.Connect.Smartcom.Test.Models
{
    [TestClass]
    public class UpdateBidAskTests
    {
        [TestMethod]
        public void UpdateBidAsk_Test()
        {
            UpdateBidAsk item = new UpdateBidAsk("RTS-6.13_FT", 0, 100, 150000, 300, 151000, 100);

            Assert.AreEqual("RTS-6.13_FT", item.Symbol);
            Assert.AreEqual(0, item.Row);
            Assert.IsInstanceOfType(item.Row, typeof(int));
            Assert.AreEqual(100, item.NRows);
            Assert.IsInstanceOfType(item.NRows, typeof(int));
            Assert.AreEqual(150000, item.Bid);
            Assert.IsInstanceOfType(item.Bid, typeof(double));
            Assert.AreEqual(300, item.BidSize);
            Assert.IsInstanceOfType(item.BidSize, typeof(double));
            Assert.AreEqual(151000, item.Ask);
            Assert.IsInstanceOfType(item.Ask, typeof(double));
            Assert.AreEqual(100, item.AskSize);
            Assert.IsInstanceOfType(item.AskSize, typeof(double));
        }
    }
}
