using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Data
{
    [TestClass]
    public class TimePeriodTests
    {
        [TestMethod]
        public void TimePeriod_Valid()
        {
            DateTime before = new DateTime(2013, 1, 1, 10, 0, 0);
            DateTime after = new DateTime(2013, 1, 1, 11, 0, 0);

            TimePeriod timePeriod = new TimePeriod(before, after);

            Assert.AreEqual(new DateTime(2013, 1, 1, 10, 0, 0), timePeriod.Begin);
            Assert.AreEqual(new DateTime(2013, 1, 1, 11, 0, 0), timePeriod.End);
            Assert.AreEqual(1, timePeriod.TimeSpan.Hours);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Invalid_TimePeriod()
        {
            DateTime before = new DateTime(2013, 1, 1, 10, 0, 0);
            DateTime after = new DateTime(2013, 1, 1, 11, 0, 0);

            TimePeriod timePeriod = new TimePeriod(after, before);
        }
    }
}
