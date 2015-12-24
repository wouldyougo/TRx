using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Indicators;

namespace TRL.Indicators.Test
{
    [TestClass]
    public class EmaTests
    {        

        [TestMethod]
        public void Indicators_Calculate_Ema_For_Ten()
        {
            double[] src = { 138700.0, 139350.0, 139970.0, 140160.0, 139845.0, 139705.0, 139895.0, 139765.0, 140495.0, 140830.0, 140745.0, 140710.0, 140830.0, 140730.0, 141435.0 };

            IEnumerable<double> result = Ema.Make(src, 10);

            Assert.AreEqual(6, result.Count());
            Assert.AreEqual(139871.5, result.ElementAt(0));
            Assert.AreEqual(140030.3182, result.ElementAt(1));
            Assert.AreEqual(140153.8967, result.ElementAt(2));
            Assert.AreEqual(140276.8246, result.ElementAt(3));
            Assert.AreEqual(140359.2201, result.ElementAt(4));
            Assert.AreEqual(140554.8164, result.ElementAt(5));

        }

        [TestMethod]
        public void Indicators_Returns_Empty_Ema_For_Empty_Source_Array()
        {
            double[] src = new double[0];

            IEnumerable<double> result = Ema.Make(src, 10);

            Assert.AreEqual(0, result.Count());

        }

        [TestMethod]
        public void Indicators_Returns_Empty_Ema_For_TooShort_Source_Array()
        {
            double[] src = { 138700.0, 139350.0, 139970.0, 140160.0, 139845.0, 139705.0, 139895.0, 139765.0, 140495.0 };

            IEnumerable<double> result = Ema.Make(src, 10);

            Assert.AreEqual(0, result.Count());

        }

        [TestMethod]
        public void Indicators_Make_One_More_Ema()
        {
            double[] src = { 447.3, 456.8, 451.0, 452.5, 453.4, 455.5, 456.0, 454.7, 453.5, 456.5, 459.5, 465.2, 460.8, 460.8 };

            IEnumerable<double> result = Ema.Make(src, 10);

            Assert.AreEqual(5, result.Count());
            Assert.AreEqual(453.72, result.ElementAt(0));
            Assert.AreEqual(454.7709, result.ElementAt(1));
            Assert.AreEqual(456.6671, result.ElementAt(2));
            Assert.AreEqual(457.4185, result.ElementAt(3));
            Assert.AreEqual(458.0333, result.ElementAt(4));
        }

        [TestMethod]
        public void Indicators_And_Make_One_More_Ema()
        {
            double[] src = { 137900.0, 138460.0, 139080.0, 139725.0, 139755.0, 139455.0, 139565.0, 139500.0, 139670.0, 140420.0, 140625.0, 140650.0, 140655.0, 140550.0, 140315.0, 141000.0, 141020.0, 140955.0, 140635.0, 140610.0, 140915.0, 140555.0 };

            IEnumerable<double> result = Ema.Make(src, 14);

            Assert.AreEqual(9, result.Count());
            Assert.AreEqual(139715.0, result.ElementAt(0));
            Assert.AreEqual(139795.0, result.ElementAt(1));
            Assert.AreEqual(139955.6667, result.ElementAt(2));
            Assert.AreEqual(140097.5778, result.ElementAt(3));
            Assert.AreEqual(140211.9008, result.ElementAt(4));
            Assert.AreEqual(140268.314, result.ElementAt(5));
            Assert.AreEqual(140313.8721, result.ElementAt(6));
            Assert.AreEqual(140394.0225, result.ElementAt(7));
            Assert.AreEqual(140415.4862, result.ElementAt(8));
        }

        [TestMethod]
        public void Indicators_Make_Ema_for_five_numbers()
        {
            IEnumerable<double> src = new[] { 5.0, 6.0, 7.0, 8.0, 9.0 };

            Assert.AreEqual(7, Ema.Make(src, 5).ElementAt(0));
        }

    }
}
