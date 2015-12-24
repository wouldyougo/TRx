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
    public class CrossLinesTests
    {
        [TestMethod]
        public void Extensions_Second_line_crosses_first_line_under_test()
        {
            double[] first = { 3.0, 4.0, 5.0, 5.0, 7.0 };
            double[] second = { 2.0, 2.0, 4.0, 6.0, 8.0 };

            Assert.IsTrue(second.CrossUnder(first));
            Assert.IsFalse(first.CrossUnder(second));
        }

        [TestMethod]
        public void Extensions_First_line_crosses_second_line_over_test()
        {
            double[] first = { 3.0, 4.0, 5.0, 5.0, 7.0 };
            double[] second = { 2.0, 2.0, 4.0, 6.0, 8.0 };

            Assert.IsTrue(first.CrossOver(second));
            Assert.IsFalse(second.CrossOver(first));
        }

        [TestMethod]
        public void Extensions_Empty_line_doesnt_cross_over_or_cross_under_any_test()
        {
            List<double> first = new List<double>();
            double[] second = { 2.0, 2.0, 4.0, 6.0, 8.0 };

            Assert.IsFalse(first.CrossOver(second));
            Assert.IsFalse(second.CrossOver(first));
        }

        [TestMethod]
        public void Extensions_both_empty_lines_cant_cross_one_another_test()
        {
            List<double> first = new List<double>();
            List<double> second = new List<double>();

            Assert.IsFalse(first.CrossOver(second));
            Assert.IsFalse(second.CrossOver(first));
        }

        [TestMethod]
        public void Extensions_Check_non_crossed_lines_of_unequals_length_test()
        {
            List<double> first = new List<double>(new[] { 2.0, 4.0, 5.0, 5.0, 6.0, 7.0 });
            double[] second = { 1.0, 3.0, 4.0, 4.0 };

            Assert.IsFalse(first.CrossOver(second));
            Assert.IsFalse(first.CrossUnder(second));

            Assert.IsFalse(second.CrossOver(first));
            Assert.IsFalse(second.CrossUnder(first));
        }
    }
}
