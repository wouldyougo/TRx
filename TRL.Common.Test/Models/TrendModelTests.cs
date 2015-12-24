using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class TrendModelTests
    {
        [TestMethod]
        public void Trend_Constructor_test()
        {
            int interval = 60;
            string symbol = "RTS-12.13_FT";
            TrendDirection direction = TrendDirection.Up;
            double speed = 30;
            Trend trend = new Trend(interval, symbol, direction, speed);

            Assert.AreEqual(interval, trend.Interval);
            Assert.AreEqual(symbol, trend.Symbol);
            Assert.AreEqual(direction, trend.Direction);
            Assert.AreEqual(speed, trend.Speed);
        }
    }
}
