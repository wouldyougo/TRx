using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Data;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class StBarIntervalFactoryTests
    {
        [TestMethod]
        public void StBarIntervalFactory_Test()
        {
            Assert.AreEqual(StBarInterval.StBarInterval_1Min, StBarIntervalFactory.Make(60));
            Assert.AreEqual(StBarInterval.StBarInterval_5Min, StBarIntervalFactory.Make(300));
            Assert.AreEqual(StBarInterval.StBarInterval_10Min, StBarIntervalFactory.Make(600));
            Assert.AreEqual(StBarInterval.StBarInterval_15Min, StBarIntervalFactory.Make(900));
            Assert.AreEqual(StBarInterval.StBarInterval_30Min, StBarIntervalFactory.Make(1800));
            Assert.AreEqual(StBarInterval.StBarInterval_60Min, StBarIntervalFactory.Make(3600));
            Assert.AreEqual(StBarInterval.StBarInterval_2Hour, StBarIntervalFactory.Make(7200));
            Assert.AreEqual(StBarInterval.StBarInterval_4Hour, StBarIntervalFactory.Make(14400));
            Assert.AreEqual(StBarInterval.StBarInterval_Day, StBarIntervalFactory.Make(86400));
            Assert.AreEqual(StBarInterval.StBarInterval_Week, StBarIntervalFactory.Make(604800));
            Assert.AreEqual(StBarInterval.StBarInterval_Month, StBarIntervalFactory.Make(2592000));
            Assert.AreEqual(StBarInterval.StBarInterval_Quarter, StBarIntervalFactory.Make(7776000));
            Assert.AreEqual(StBarInterval.StBarInterval_Year, StBarIntervalFactory.Make(31536000));
        }
    }
}
