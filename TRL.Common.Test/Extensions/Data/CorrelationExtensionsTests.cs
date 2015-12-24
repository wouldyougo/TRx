using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Extensions;
using TRL.Common.Extensions.Data;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class CorrelationExtensionsTests
    {
        private List<double> srcPattern;

        [TestInitialize]
        public void Setup()
        {
            this.srcPattern = new List<double>();
            this.srcPattern.Add(10);
            this.srcPattern.Add(12);
            this.srcPattern.Add(14);
            this.srcPattern.Add(16);
            this.srcPattern.Add(18);
        }

        [TestMethod]
        public void EuclideanDistance_equals_one_test()
        {
            Assert.AreEqual(1, this.srcPattern.CalculateEuclideanDistanceWith(this.srcPattern));
        }

        [TestMethod]
        public void EuclideanDistance_equals_non_one_test()
        {
            List<double> match = new List<double>();
            match.Add(12);
            match.Add(14);
            match.Add(16);
            match.Add(18);
            match.Add(20);

            Assert.AreEqual(0.182744, Math.Round(this.srcPattern.CalculateEuclideanDistanceWith(match), 6));
        }

        [TestMethod]
        public void PearsonCorrelation_equals_one_test()
        {
            Assert.AreEqual(1, this.srcPattern.CalculatePearsonCorrelationWith(this.srcPattern));
        }

        [TestMethod]
        public void PearsonCorrelation_equals_one_too_test()
        {
            List<double> match = new List<double>();
            match.Add(12);
            match.Add(14);
            match.Add(16);
            match.Add(18);
            match.Add(20);

            Assert.AreEqual(1, this.srcPattern.CalculatePearsonCorrelationWith(match));
        }

        [TestMethod]
        public void PearsonCorrelation_one_more_test()
        {
            List<double> match = new List<double>();
            match.Add(1);
            match.Add(2);
            match.Add(3);
            match.Add(6);
            match.Add(8);

            Assert.AreEqual(0.976187, Math.Round(this.srcPattern.CalculatePearsonCorrelationWith(match), 6));
        }

        [TestMethod]
        public void PearsonCorrelation_negative_value_test()
        {
            List<double> match = new List<double>();
            match.Add(10);
            match.Add(8);
            match.Add(8);
            match.Add(7);
            match.Add(7);

            Assert.AreEqual(-0.9037, Math.Round(this.srcPattern.CalculatePearsonCorrelationWith(match), 4));
        }

        [TestMethod]
        public void PearsonCorrelation_returns_zero_if_any_of_pattern_contains_no_data_test()
        {
            List<double> src = new List<double>();

            List<double> dst = new List<double>();
            dst.Add(10);

            Assert.AreEqual(0, src.CalculatePearsonCorrelationWith(dst));
            Assert.AreEqual(0, dst.CalculatePearsonCorrelationWith(src));
        }

        [TestMethod]
        public void PearsonCorrelation_returns_zero_if_any_patterns_contains_unequal_count_of_values_test()
        {
            List<double> match = new List<double>();
            match.Add(10);
            match.Add(8);
            match.Add(8);
            match.Add(7);

            Assert.AreEqual(0, this.srcPattern.CalculatePearsonCorrelationWith(match));
        }

        //[TestMethod]
        //public void PearsonCorrelation_in_place_with_big_array()
        //{
        //    List<double> match = new List<double>(new double[100]);

        //    List<double> pattern = new List<double>(new double[100]);

        //    for (int i = 0; i < 500000; i++)
        //        pattern.CalculatePearsonCorrelationWith(match);

        //    Assert.AreEqual(1, this.srcPattern.CalculatePearsonCorrelationWith(match));
        //}

        [TestMethod]
        public void PearsonCorrelation_in_place_equals_one_test()
        {
            Assert.AreEqual(1, this.srcPattern.CalculatePearsonCorrelation(0, this.srcPattern));
        }

        [TestMethod]
        public void PearsonCorrelation_in_place_equals_one_too_test()
        {
            List<double> match = new List<double>();
            match.Add(12);
            match.Add(14);
            match.Add(16);
            match.Add(18);
            match.Add(20);

            Assert.AreEqual(1, this.srcPattern.CalculatePearsonCorrelation(0, match));
        }

        [TestMethod]
        public void PearsonCorrelation_in_place_one_more_test()
        {
            List<double> match = new List<double>();
            match.Add(1);
            match.Add(2);
            match.Add(3);
            match.Add(6);
            match.Add(8);

            Assert.AreEqual(0.976187, Math.Round(this.srcPattern.CalculatePearsonCorrelation(0, match), 6));
        }

        [TestMethod]
        public void PearsonCorrelation_in_place_negative_value_test()
        {
            List<double> match = new List<double>();
            match.Add(10);
            match.Add(8);
            match.Add(8);
            match.Add(7);
            match.Add(7);

            Assert.AreEqual(-0.9037, Math.Round(this.srcPattern.CalculatePearsonCorrelation(0, match), 4));
        }
    }
}
