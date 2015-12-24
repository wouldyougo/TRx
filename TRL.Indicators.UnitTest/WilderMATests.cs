using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Indicators;

namespace TRL.Indicators.Test
{
    [TestClass]
    public class WilderMATests
    {
        [TestMethod]
        public void Indicators_WilderMA_test()
        {
            List<Bar> bars = new List<Bar>();
            bars.Add(new Bar(new DateTime(2013, 10, 24, 11, 0, 0), 32066, 32066, 31922, 31931, 100));
            bars.Add(new Bar(new DateTime(2013, 10, 24, 12, 0, 0), 31931, 31970, 31922, 31965, 100));
            bars.Add(new Bar(new DateTime(2013, 10, 24, 13, 0, 0), 31966, 31985, 31958, 31971, 100));
            bars.Add(new Bar(new DateTime(2013, 10, 24, 14, 0, 0), 31972, 31999, 31969, 31979, 100));
            bars.Add(new Bar(new DateTime(2013, 10, 24, 15, 0, 0), 31980, 31982, 31957, 31967, 100));
            bars.Add(new Bar(new DateTime(2013, 10, 24, 16, 0, 0), 31970, 31977, 31961, 31970, 100));
            bars.Add(new Bar(new DateTime(2013, 10, 24, 17, 0, 0), 31970, 32000, 31946, 31990, 100));

            IEnumerable<double> source = bars.Select(b => b.Close);
            
            IEnumerable<double> result = WilderMA.Values(source, 5);

            Assert.AreEqual(7, result.Count());

            Assert.AreEqual(0, result.ElementAt(0));
            Assert.AreEqual(0, result.ElementAt(1));
            Assert.AreEqual(0, result.ElementAt(2));
            Assert.AreEqual(0, result.ElementAt(3));
            Assert.AreEqual(31962.6, result.ElementAt(4));
            Assert.AreEqual(31964.08, Math.Round(result.ElementAt(5), 2));
            Assert.AreEqual(31969.264, Math.Round(result.ElementAt(6), 3));
        }
    }
}
