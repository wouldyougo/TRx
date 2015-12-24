using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;

namespace TRL.Common.Test.Data
{
    [TestClass]
    public class HistoryDataRequestTests
    {
        [TestMethod]
        public void TenMinutesBarHistoryDataRequest_constructor_tests()
        {
            DateTime fromDate = new DateTime(2014, 1, 1);

            IHistoryDataRequest request =
                new TenMinutesBarHistoryDataRequest("RTS-6.14",
                    10,
                    fromDate);

            Assert.AreEqual("RTS-6.14", request.Symbol);
            Assert.AreEqual(600, request.Interval);
            Assert.AreEqual(10, request.Quantity);
            Assert.AreEqual(fromDate, request.FromDate);
        }

        [TestMethod]
        public void TickHistoryDataRequest_test()
        {
            DateTime fromDate = new DateTime(2014, 1, 1);

            IHistoryDataRequest request =
                new TickHistoryDataRequest("RTS-6.14", 100, fromDate);

            Assert.AreEqual("RTS-6.14", request.Symbol);
            Assert.AreEqual(0, request.Interval);
            Assert.AreEqual(100, request.Quantity);
            Assert.AreEqual(fromDate, request.FromDate);
        }
    }
}
