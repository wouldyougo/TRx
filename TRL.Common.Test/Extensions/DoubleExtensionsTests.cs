using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using TRL.Common.Extensions.Data;
using TRL.Common.Extensions;

namespace TRL.Common.Test.Extensions
{
    [TestClass]
    public class DoubleExtensionsTests
    {

        [TestMethod]
        public void Extensions_round_value_to_step_one()
        {
            double step = 1;

            double src = 32113;

            Assert.AreEqual(32113, src.RoundUp(step));
            Assert.AreEqual(32113, src.RoundDown(step));
        }

        [TestMethod]
        public void Extensions_round_fractional_to_step_one()
        {
            double step = 1;

            double src = 33155.396;

            Assert.AreEqual(33156, src.RoundUp(step));
            Assert.AreEqual(33155, src.RoundDown(step));
        }

        [TestMethod]
        public void Extensions_round_value_to_step_ten()
        {
            double step = 10;

            double src = 128815;

            Assert.AreEqual(128820, src.RoundUp(step));
            Assert.AreEqual(128810, src.RoundDown(step));
        }

        [TestMethod]
        public void Extensions_round_fractional_to_step_ten()
        {
            double step = 10;

            double src = 128815.22;

            Assert.AreEqual(128820, src.RoundUp(step));
            Assert.AreEqual(128810, src.RoundDown(step));
        }

        [TestMethod]
        public void Extensions_negative_value_HasOppositeSignWith_positive_value_test()
        {
            double value = -20.065;
            Assert.IsTrue(value.HasOppositeSignWith(1));
        }

        [TestMethod]
        public void Extensions_positive_value_HasOppositeSignWith_negative_value_test()
        {
            double value = 20.065;
            Assert.IsTrue(value.HasOppositeSignWith(-1));
        }

        [TestMethod]
        public void Extensions_positive_value_doesnt_HasOppositeSignWith_positive_value_test()
        {
            double value = 20.065;
            Assert.IsFalse(value.HasOppositeSignWith(1));
        }

        [TestMethod]
        public void Extensions_negative_value_doesnt_HasOppositeSignWith_negative_value_test()
        {
            double value = -20.065;
            Assert.IsFalse(value.HasOppositeSignWith(-1));
        }
    }
}
