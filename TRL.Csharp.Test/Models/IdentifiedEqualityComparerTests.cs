using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Csharp.Models;
using System.Collections.Generic;

namespace TRL.Csharp.Test.Models
{
    [TestClass]
    public class IdentifiedEqualityComparerTests
    {
        [TestMethod]
        public void compare_two_different_trades_test()
        {
            Trade first = new Trade(1, "ST12345-RF-01", "RTS-9.14", new DateTime(2014, 8, 26, 10, 0, 0), 125000, 10);
            Trade second = new Trade(2, "ST12345-RF-01", "RTS-9.14", new DateTime(2014, 8, 26, 10, 0, 0), 125000, 10);

            IEqualityComparer<Trade> comparer = new IdentifiedEqualityComparer();
            Assert.IsFalse(comparer.Equals(first, second));
        }

        [TestMethod]
        public void compare_two_equals_trades_test()
        {
            Trade first = new Trade(1, "ST12345-RF-01", "RTS-9.14", new DateTime(2014, 8, 26, 10, 0, 0), 125000, 10);
            Trade second = new Trade(1, "ST12345-RF-01", "RTS-9.14", new DateTime(2014, 8, 26, 10, 0, 0), 125000, 10);

            IEqualityComparer<Trade> comparer = new IdentifiedEqualityComparer();
            Assert.IsTrue(comparer.Equals(first, second));
        }

    }
}
