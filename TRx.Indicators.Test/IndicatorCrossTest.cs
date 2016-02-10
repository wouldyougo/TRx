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
    public class IndicatorCrossTests
    {
        [TestMethod]
        public void IndicatorCross_Second_line_crosses_first_line_under_test()
        {
            double[] first = { 3.0, 4.0, 5.0, 5.0, 7.0 };
            double[] second = { 2.0, 2.0, 4.0, 6.0, 8.0 };
            var b1 = Indicator.CrossUnder(first, second, 3, 3);
            var b2 = Indicator.CrossUnder(second, first, 3, 3);
            Assert.IsTrue(b1);
            Assert.IsFalse(b2);
        }

        [TestMethod]
        public void IndicatorCross_First_line_crosses_second_line_over_test()
        {
            double[] first = { 3.0, 4.0, 5.0, 5.0, 7.0 };
            double[] second = { 2.0, 2.0, 4.0, 6.0, 8.0 };

            Assert.IsTrue(Indicator.CrossOver(second, first, 3, 3));
            Assert.IsFalse(Indicator.CrossOver(first, second, 3, 3));
        }

        [TestMethod]
        public void IndicatorCross_Empty_line_doesnt_cross_over_or_cross_under_any_test()
        {
            List<double> first = new List<double>();
            double[] second = { 2.0, 2.0, 4.0, 6.0, 8.0 };

            Assert.IsFalse(Indicator.CrossOver(first, second, 3, 3));
            Assert.IsFalse(Indicator.CrossOver(second, first, 3, 3));
        }

        [TestMethod]
        public void IndicatorCross_both_empty_lines_cant_cross_one_another_test()
        {
            List<double> first = new List<double>();
            List<double> second = new List<double>();

            Assert.IsFalse(Indicator.CrossOver(first, second, 3, 3));
            Assert.IsFalse(Indicator.CrossOver(second, first, 3, 3));
        }

        [TestMethod]
        public void IndicatorCross_Check_crossed_lines_of_unequals_length_test()
        {
            List<double> first = new List<double>(
                        new[] { 2.0, 4.0, 5.0, 3.0, 6.0, 7.0 });
            double[] second = { 1.0, 3.0, 4.0, 4.0 };
            //                  0    1    2    3    4   
            Assert.IsFalse(Indicator.CrossOver(first, second, 3, 3));
            Assert.IsTrue(Indicator.CrossUnder(first, second, 3, 3));

            Assert.IsTrue(Indicator.CrossOver(first, second, 4, 3));
            Assert.IsFalse(Indicator.CrossUnder(first, second, 4, 3));

            Assert.IsTrue(Indicator.CrossOver(second, first, 3, 3));
            Assert.IsFalse(Indicator.CrossUnder(second, first, 3, 3));

            Assert.IsFalse(Indicator.CrossOver(second, first, 4, 3));
            Assert.IsFalse(Indicator.CrossUnder(second, first, 4, 3));
        }

        [TestMethod]
        public void IndicatorCross_line_cross_over_value_test()
        {
            double first = 5.0;
            double[] second = { 2.0, 2.0, 4.0, 6.0, 8.0 };

            Assert.IsFalse(Indicator.CrossUnder(first, second, 1));
            Assert.IsFalse(Indicator.CrossOver(first, second, 1));

            Assert.IsFalse(Indicator.CrossUnder(first, second, 2));
            Assert.IsFalse(Indicator.CrossOver(first, second, 2));

            Assert.IsTrue(Indicator.CrossUnder(first, second, 3));
            Assert.IsFalse(Indicator.CrossOver(first, second, 3));

            Assert.IsFalse(Indicator.CrossUnder(first, second, 4));
            Assert.IsFalse(Indicator.CrossOver(first, second, 4));
        }
        [TestMethod]
        public void IndicatorCross_line_cross_under_value_test()
        {
            double first = 5.0;
            double[] second = { 2.0, 2.0, 6.0, 4.0, 8.0 };

            Assert.IsFalse(Indicator.CrossUnder(first, second, 1));
            Assert.IsFalse(Indicator.CrossOver(first, second, 1));

            Assert.IsTrue(Indicator.CrossUnder(first, second, 2));
            Assert.IsFalse(Indicator.CrossOver(first, second, 2));

            Assert.IsFalse(Indicator.CrossUnder(first, second, 3));
            Assert.IsTrue(Indicator.CrossOver(first, second, 3));

            Assert.IsTrue(Indicator.CrossUnder(first, second, 4));
            Assert.IsFalse(Indicator.CrossOver(first, second, 4));
        }
    }
}
