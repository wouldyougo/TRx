using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Csharp.Models;

namespace TRL.Csharp.Test.Models
{
    [TestClass]
    public class BarTests
    {
        [TestMethod]
        public void Bar_constructor_test()
        {
            DateTime date = DateTime.Now;
            int intervalSeconds = 60;
            string symbol = "RTS-9.14";
            double open = 125000;
            double high = 128000;
            double low = 123000;
            double close = 126000;
            double volume = 5000;

            Bar bar = new Bar(date, intervalSeconds, symbol, open, high, low, close, volume);

            Assert.AreEqual(date, bar.DateTime);
            Assert.AreEqual(intervalSeconds, bar.IntervalSeconds);
            Assert.AreEqual(symbol, bar.Symbol);
            Assert.AreEqual(open, bar.Open);
            Assert.AreEqual(high, bar.High);
            Assert.AreEqual(low, bar.Low);
            Assert.AreEqual(close, bar.Close);
            Assert.AreEqual(volume, bar.Volume);
        }
    }
}
