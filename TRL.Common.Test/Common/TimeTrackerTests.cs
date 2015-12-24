using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Test
{
    [TestClass]
    public class TimeTrackerTests
    {
        [TestMethod]
        public void Common_TimeTracker_Test()
        {
            DateTime start = BrokerDateTime.Make(DateTime.Now);

            ITimeTrackable r = new TimeTracker();

            System.Threading.Thread.Sleep(10);

            Assert.AreEqual(start.Year, r.StartAt.Year);
            Assert.AreEqual(start.Month, r.StartAt.Month);
            Assert.AreEqual(start.Day, r.StartAt.Day);
            Assert.AreEqual(start.Hour, r.StartAt.Hour);
            Assert.AreEqual(start.Minute, r.StartAt.Minute);
            Assert.AreEqual(start.Second, r.StartAt.Second);

            Assert.IsTrue(r.Duration >= (start.AddMilliseconds(10) - start));
        }

        [TestMethod]
        public void Common_FakeTimeTracker_Test()
        {
            DateTime start = new DateTime(2013, 5, 10, 10, 0, 0);
            DateTime stop = new DateTime(2013, 5, 10, 15, 0, 0);

            ITimeTrackable r = new FakeTimeTracker(start, stop);

            System.Threading.Thread.Sleep(10);

            Assert.AreEqual(start.Year, r.StartAt.Year);
            Assert.AreEqual(start.Month, r.StartAt.Month);
            Assert.AreEqual(start.Day, r.StartAt.Day);
            Assert.AreEqual(start.Hour, r.StartAt.Hour);
            Assert.AreEqual(start.Minute, r.StartAt.Minute);
            Assert.AreEqual(start.Second, r.StartAt.Second);

            Assert.AreEqual(new TimeSpan(5, 0, 0), r.Duration);
        }
    }
}
