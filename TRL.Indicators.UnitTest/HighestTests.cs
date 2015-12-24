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
    public class HighestTests
    {

        [TestInitialize]
        public void Indicators_Setup()
        {
        }

        private IEnumerable<Bar> MakeBars()
        {
            List<Bar> bars = new List<Bar>();

            bars.Add(Bar.Parse("20120903,100000,138285.00000,139150.00000,137900.00000,138700.00000,108700"));
            bars.Add(Bar.Parse("20120903,110000,138695.00000,139430.00000,138460.00000,139350.00000,116471"));
            bars.Add(Bar.Parse("20120903,120000,139340.00000,140155.00000,139080.00000,139970.00000,129776"));
            bars.Add(Bar.Parse("20120903,130000,139985.00000,140330.00000,139725.00000,140160.00000,70623"));
            bars.Add(Bar.Parse("20120903,140000,140160.00000,140300.00000,139755.00000,139845.00000,45450"));
            bars.Add(Bar.Parse("20120903,150000,139865.00000,140000.00000,139455.00000,139705.00000,52745"));
            bars.Add(Bar.Parse("20120903,160000,139685.00000,139960.00000,139565.00000,139895.00000,39282"));
            bars.Add(Bar.Parse("20120903,170000,139880.00000,139995.00000,139500.00000,139765.00000,48546"));
            bars.Add(Bar.Parse("20120903,180000,139760.00000,140750.00000,139670.00000,140495.00000,98837"));
            bars.Add(Bar.Parse("20120903,190000,140500.00000,141150.00000,140420.00000,140830.00000,36736"));

            return bars;
        }


        [TestMethod]
        public void Indicators_Highest_HighCollection_For_Period_Three()
        {
            IEnumerable<double> highest = Highest.HighCollection(MakeBars(), 3);

            Assert.AreEqual(10, highest.Count());
            Assert.AreEqual(141150, highest.ElementAt(9));
            Assert.AreEqual(141150, highest.ElementAt(8));
            Assert.AreEqual(141150, highest.ElementAt(7));
            Assert.AreEqual(140300, highest.ElementAt(6));
            Assert.AreEqual(140300, highest.ElementAt(5));
            Assert.AreEqual(140300, highest.ElementAt(4));
            Assert.AreEqual(140330, highest.ElementAt(3));
            Assert.AreEqual(140330, highest.ElementAt(2));
            Assert.AreEqual(140330, highest.ElementAt(1));
            Assert.AreEqual(139150, highest.ElementAt(0));

        }

        [TestMethod]
        public void Indicators_Highest_HighCollection_For_Period_Two()
        {
            IEnumerable<double> highest = Highest.HighCollection(MakeBars(), 2);

            Assert.AreEqual(10, highest.Count());
            Assert.AreEqual(141150, highest.ElementAt(9));
            Assert.AreEqual(141150, highest.ElementAt(8));
            Assert.AreEqual(139995, highest.ElementAt(7));
            Assert.AreEqual(139995, highest.ElementAt(6));
            Assert.AreEqual(140300, highest.ElementAt(5));
            Assert.AreEqual(140300, highest.ElementAt(4));
            Assert.AreEqual(140330, highest.ElementAt(3));
            Assert.AreEqual(140330, highest.ElementAt(2));
            Assert.AreEqual(139430, highest.ElementAt(1));
            Assert.AreEqual(139430, highest.ElementAt(0));

        }

        [TestMethod]
        public void Indicators_Highest_HighCollection_For_Period_Four()
        {
            IEnumerable<double> highest = Highest.HighCollection(MakeBars(), 4);

            Assert.AreEqual(10, highest.Count());
            Assert.AreEqual(141150, highest.ElementAt(9));
            Assert.AreEqual(141150, highest.ElementAt(8));
            Assert.AreEqual(141150, highest.ElementAt(7));
            Assert.AreEqual(141150, highest.ElementAt(6));
            Assert.AreEqual(140330, highest.ElementAt(5));
            Assert.AreEqual(140330, highest.ElementAt(4));
            Assert.AreEqual(140330, highest.ElementAt(3));
            Assert.AreEqual(140330, highest.ElementAt(2));
            Assert.AreEqual(139430, highest.ElementAt(1));
            Assert.AreEqual(139430, highest.ElementAt(0));

        }

        [TestMethod]
        public void Indicators_Highest_HighCollection_For_Period_Five()
        {
            IEnumerable<double> highest = Highest.HighCollection(MakeBars(), 5);

            Assert.AreEqual(10, highest.Count());
            Assert.AreEqual(141150, highest.ElementAt(9));
            Assert.AreEqual(141150, highest.ElementAt(8));
            Assert.AreEqual(141150, highest.ElementAt(7));
            Assert.AreEqual(141150, highest.ElementAt(6));
            Assert.AreEqual(141150, highest.ElementAt(5));
            Assert.AreEqual(140330, highest.ElementAt(4));
            Assert.AreEqual(140330, highest.ElementAt(3));
            Assert.AreEqual(140330, highest.ElementAt(2));
            Assert.AreEqual(140330, highest.ElementAt(1));
            Assert.AreEqual(140330, highest.ElementAt(0));
        }

        [TestMethod]
        public void Indicators_Highest_HighCollection_For_Period_Six()
        {
            IEnumerable<double> highest = Highest.HighCollection(MakeBars(), 6);

            Assert.AreEqual(10, highest.Count());
            Assert.AreEqual(141150, highest.ElementAt(9));
            Assert.AreEqual(141150, highest.ElementAt(8));
            Assert.AreEqual(141150, highest.ElementAt(7));
            Assert.AreEqual(141150, highest.ElementAt(6));
            Assert.AreEqual(141150, highest.ElementAt(5));
            Assert.AreEqual(141150, highest.ElementAt(4));
            Assert.AreEqual(140330, highest.ElementAt(3));
            Assert.AreEqual(140330, highest.ElementAt(2));
            Assert.AreEqual(140330, highest.ElementAt(1));
            Assert.AreEqual(140330, highest.ElementAt(0));
        }

        [TestMethod]
        public void Indicators_Highest_HighCollection_For_Period_Seven()
        {
            IEnumerable<double> highest = Highest.HighCollection(MakeBars(), 7);

            Assert.AreEqual(10, highest.Count());
            Assert.AreEqual(141150, highest.ElementAt(9));
            Assert.AreEqual(141150, highest.ElementAt(8));
            Assert.AreEqual(141150, highest.ElementAt(7));
            Assert.AreEqual(141150, highest.ElementAt(6));
            Assert.AreEqual(141150, highest.ElementAt(5));
            Assert.AreEqual(141150, highest.ElementAt(4));
            Assert.AreEqual(141150, highest.ElementAt(3));
            Assert.AreEqual(140155, highest.ElementAt(2));
            Assert.AreEqual(140155, highest.ElementAt(1));
            Assert.AreEqual(140155, highest.ElementAt(0));
        }

        [TestMethod]
        public void Indicators_Highest_HighCollection_For_Period_Eight()
        {
            IEnumerable<double> highest = Highest.HighCollection(MakeBars(), 8);

            Assert.AreEqual(10, highest.Count());
            Assert.AreEqual(141150, highest.ElementAt(9));
            Assert.AreEqual(141150, highest.ElementAt(8));
            Assert.AreEqual(141150, highest.ElementAt(7));
            Assert.AreEqual(141150, highest.ElementAt(6));
            Assert.AreEqual(141150, highest.ElementAt(5));
            Assert.AreEqual(141150, highest.ElementAt(4));
            Assert.AreEqual(141150, highest.ElementAt(3));
            Assert.AreEqual(141150, highest.ElementAt(2));
            Assert.AreEqual(139430, highest.ElementAt(1));
            Assert.AreEqual(139430, highest.ElementAt(0));
        }

        [TestMethod]
        public void Indicators_Highest_HighCollection_For_Period_Nine()
        {
            IEnumerable<double> highest = Highest.HighCollection(MakeBars(), 9);

            Assert.AreEqual(10, highest.Count());
            Assert.AreEqual(141150, highest.ElementAt(9));
            Assert.AreEqual(141150, highest.ElementAt(8));
            Assert.AreEqual(141150, highest.ElementAt(7));
            Assert.AreEqual(141150, highest.ElementAt(6));
            Assert.AreEqual(141150, highest.ElementAt(5));
            Assert.AreEqual(141150, highest.ElementAt(4));
            Assert.AreEqual(141150, highest.ElementAt(3));
            Assert.AreEqual(141150, highest.ElementAt(2));
            Assert.AreEqual(141150, highest.ElementAt(1));
            Assert.AreEqual(139150, highest.ElementAt(0));
        }

        [TestMethod]
        public void Indicators_Highest_HighCollection_For_Period_Ten()
        {
            IEnumerable<double> highest = Highest.HighCollection(MakeBars(), 10);

            Assert.AreEqual(10, highest.Count());
            Assert.AreEqual(141150, highest.ElementAt(9));
            Assert.AreEqual(141150, highest.ElementAt(8));
            Assert.AreEqual(141150, highest.ElementAt(7));
            Assert.AreEqual(141150, highest.ElementAt(6));
            Assert.AreEqual(141150, highest.ElementAt(5));
            Assert.AreEqual(141150, highest.ElementAt(4));
            Assert.AreEqual(141150, highest.ElementAt(3));
            Assert.AreEqual(141150, highest.ElementAt(2));
            Assert.AreEqual(141150, highest.ElementAt(1));
            Assert.AreEqual(141150, highest.ElementAt(0));
        }
    }
}
