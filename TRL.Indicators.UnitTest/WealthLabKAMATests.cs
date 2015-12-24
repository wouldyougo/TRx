using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Indicators;

namespace TRL.Indicators.Test
{
    [TestClass]
    public class WealthLabKAMATests
    {
        private List<double> src;

        [TestInitialize]
        public void Indicators_Setup()
        {
            this.src = new List<double>();
            this.src.Add(148620);
            this.src.Add(148280);
            this.src.Add(148360);
            this.src.Add(148770);
            this.src.Add(148850);
            this.src.Add(148230);
            this.src.Add(148950);
            this.src.Add(149350);
            this.src.Add(149310);
            this.src.Add(149560);
            this.src.Add(149140);
        }

        [TestMethod]
        public void Indicators_WealthLabKAMA_indicator_tests()
        {
            IEnumerable<double> result = WealthLabKAMA.Values(this.src, 6);

            Assert.AreEqual(11, result.Count());

            Assert.AreEqual(0, result.ElementAt(0));
            Assert.AreEqual(0, result.ElementAt(1));
            Assert.AreEqual(0, result.ElementAt(2));
            Assert.AreEqual(0, result.ElementAt(3));
            Assert.AreEqual(0, result.ElementAt(4));
            Assert.AreEqual(0, result.ElementAt(5));
            Assert.AreEqual(0, result.ElementAt(6));
            Assert.AreEqual(148997.1789, Math.Round(result.ElementAt(7), 4));
            Assert.AreEqual(149028.5183, Math.Round(result.ElementAt(8), 4));
            Assert.AreEqual(149073.2054, Math.Round(result.ElementAt(9), 4));
            Assert.AreEqual(149074.437, Math.Round(result.ElementAt(10), 4));
        }

    }
}
