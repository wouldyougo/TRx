using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartCOM3Lib;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Data;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class BarIntervalFactoryTests
    {
        [TestMethod]
        public void BarIntervalFactory_Make()
        {
            Assert.AreEqual(0, BarIntervalFactory.Make(StBarInterval.StBarInterval_Tick));
            Assert.AreEqual(60, BarIntervalFactory.Make(StBarInterval.StBarInterval_1Min));
            Assert.AreEqual(300, BarIntervalFactory.Make(StBarInterval.StBarInterval_5Min));
            Assert.AreEqual(600, BarIntervalFactory.Make(StBarInterval.StBarInterval_10Min));
            Assert.AreEqual(900, BarIntervalFactory.Make(StBarInterval.StBarInterval_15Min));
            Assert.AreEqual(1800, BarIntervalFactory.Make(StBarInterval.StBarInterval_30Min));
            Assert.AreEqual(3600, BarIntervalFactory.Make(StBarInterval.StBarInterval_60Min));
            Assert.AreEqual(7200, BarIntervalFactory.Make(StBarInterval.StBarInterval_2Hour));
            Assert.AreEqual(14400, BarIntervalFactory.Make(StBarInterval.StBarInterval_4Hour));
            Assert.AreEqual(86400, BarIntervalFactory.Make(StBarInterval.StBarInterval_Day));
            Assert.AreEqual(604800, BarIntervalFactory.Make(StBarInterval.StBarInterval_Week));
            Assert.AreEqual(2592000, BarIntervalFactory.Make(StBarInterval.StBarInterval_Month));
            Assert.AreEqual(7776000, BarIntervalFactory.Make(StBarInterval.StBarInterval_Quarter));
            Assert.AreEqual(31536000, BarIntervalFactory.Make(StBarInterval.StBarInterval_Year));
        }
    }
}
