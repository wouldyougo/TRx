using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using TRL.Common.IndicatorCross.Data;
//using TRL.Common.IndicatorCross;

namespace TRx.Indicators.Test
{
    [TestClass]
    public class IndicatorDifferenceTests
    {
        [TestMethod]
        public void IndicatorDifference0_test()
        {
            double[] first = { 3.0, 4.0, 5.0, 6.0, 7.0 };
            double[] second = { 2.0, 3.0, 4.0, 5.0, 7.0 };
            var b1 = Indicator.Difference(first, 0);
            Assert.AreEqual(b1.Count, 5);
            Assert.AreEqual(b1.Sum(), 0);

            var b2 = Indicator.Difference(first, 1);
            Assert.AreEqual(b2.Count, 5);
            Assert.AreEqual(b2.Sum(), 4);
            Assert.AreEqual(b2[0], 0);
        }
    }
}
