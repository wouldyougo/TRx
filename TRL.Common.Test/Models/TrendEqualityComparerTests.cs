using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class TrendEqualityComparerTests
    {
        [TestMethod]
        public void TrendEqualityComparer_trends_are_equals_test()
        {
            Trend t1 = new Trend(60, "RTS-12.13_FT", TrendDirection.Up, 10);
            Trend t2 = new Trend(60, "RTS-12.13_FT", TrendDirection.Down, 30);
            
            TrendEqualityComparer tc = new TrendEqualityComparer();

            Assert.IsTrue(tc.Equals(t1, t2));
        }

        [TestMethod]
        public void TrendEqualityComparer_trends_are_not_equals_test()
        {
            Trend t1 = new Trend(30, "RTS-12.13_FT", TrendDirection.Up, 10);
            Trend t2 = new Trend(60, "RTS-12.13_FT", TrendDirection.Down, 30);

            TrendEqualityComparer tc = new TrendEqualityComparer();

            Assert.IsFalse(tc.Equals(t1, t2));

            Trend t3 = new Trend(60, "RTS-12.13_FT", TrendDirection.Flat, 20);
            Trend t4 = new Trend(60, "Si-12.13_FT", TrendDirection.Flat, 20);

            Assert.IsFalse(tc.Equals(t3, t4));
        }
    }
}
