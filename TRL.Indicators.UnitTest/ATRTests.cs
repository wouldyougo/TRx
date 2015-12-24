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
    public class ATRTests
    {
        [TestMethod]
        public void Indicators_Make_ATR_for_one_bar_test()
        {
            List<Bar> bars = new List<Bar>();
            bars.Add(new Bar(new DateTime(2013, 10, 1), 50, 55, 49, 51, 35));

            Assert.AreEqual(6, ATR.Value(bars));
        }

        [TestMethod]
        public void Indicators_Make_TrueRange_for_two_bars_test()
        {
            List<Bar> bars = new List<Bar>();
            bars.Add(new Bar(new DateTime(2013, 10, 1), 50, 55, 49, 51, 35));
            bars.Add(new Bar(new DateTime(2013, 10, 2), 51, 56, 48, 52, 38));

            Assert.AreEqual(7, ATR.Value(bars));
        }

        [TestMethod]
        public void Indicators_Make_ATR_for_collection()
        {
            List<Bar> bars = new List<Bar>();
            bars.Add(new Bar(new DateTime(2013, 10, 1), 50, 55, 49, 51, 35));
            bars.Add(new Bar(new DateTime(2013, 10, 2), 51, 56, 48, 52, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 3), 49, 53, 44, 50, 41));

            Assert.AreEqual(7.6667, ATR.Value(bars));
        }

        [TestMethod]
        public void Indicators_Make_ATR_value()
        {
            List<Bar> bars = new List<Bar>();
            bars.Add(new Bar(new DateTime(2013, 10, 11,  0, 0, 0), 148630, 149220, 148600, 149140, 35));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 11, 0, 0), 149170, 149310, 148380, 148800, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 12, 0, 0), 148800, 148810, 147470, 147900, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 13, 0, 0), 147910, 148370, 147800, 148250, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 14, 0, 0), 148240, 148340, 147920, 148130, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 15, 0, 0), 148110, 148290, 148000, 148260, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 16, 0, 0), 148260, 148260, 147940, 147980, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 17, 0, 0), 147990, 148110, 147670, 147680, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 18, 0, 0), 147680, 147940, 147110, 147920, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 19, 0, 0), 147920, 148030, 147550, 147810, 38));

            Assert.AreEqual(624, ATR.Value(bars));
        }

        [TestMethod]
        public void Indicators_Make_ATR_values()
        {
            List<Bar> bars = new List<Bar>();
            bars.Add(new Bar(new DateTime(2013, 10, 11, 0, 0, 0), 148630, 149220, 148600, 149140, 35));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 11, 0, 0), 149170, 149310, 148380, 148800, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 12, 0, 0), 148800, 148810, 147470, 147900, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 13, 0, 0), 147910, 148370, 147800, 148250, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 14, 0, 0), 148240, 148340, 147920, 148130, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 15, 0, 0), 148110, 148290, 148000, 148260, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 16, 0, 0), 148260, 148260, 147940, 147980, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 17, 0, 0), 147990, 148110, 147670, 147680, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 18, 0, 0), 147680, 147940, 147110, 147920, 38));
            bars.Add(new Bar(new DateTime(2013, 10, 11, 19, 0, 0), 147920, 148030, 147550, 147810, 38));

            IEnumerable<double> result = ATR.Values(bars, 5);

            Assert.AreEqual(0, result.ElementAt(0));
            Assert.AreEqual(0, result.ElementAt(1));
            Assert.AreEqual(0, result.ElementAt(2));
            Assert.AreEqual(0, result.ElementAt(3));
            Assert.AreEqual(776, result.ElementAt(4));
            Assert.AreEqual(710, result.ElementAt(5));
            Assert.AreEqual(588, result.ElementAt(6));
            Assert.AreEqual(408, result.ElementAt(7));
            Assert.AreEqual(460, result.ElementAt(8));
            Assert.AreEqual(472, result.ElementAt(9));
        }

    }
}
