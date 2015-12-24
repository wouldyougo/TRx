using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using System.Globalization;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class CorrelationTests
    {
        private StrategyHeader strategyHeader;
        private double value;
        private DateTime date;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-3.14_FT", 10);
            this.value = 0.98;
            this.date = DateTime.Now;
        }

        [TestMethod]
        public void Correlation_constructor_test()
        {
            Correlation correlation = new Correlation(strategyHeader, date, value);
            Assert.AreEqual(strategyHeader, correlation.Strategy);
            Assert.AreEqual(strategyHeader.Id, correlation.StrategyId);
            Assert.AreEqual(date, correlation.DateTime);
            Assert.AreEqual(value, correlation.Value);
        }

        [TestMethod]
        public void Correlation_ToString_test()
        {
            Correlation correlation = new Correlation(strategyHeader, date, value);

            CultureInfo ci = CultureInfo.InvariantCulture;

            string expect = String.Format("Correlation: {0}, {1}, {2}", correlation.StrategyId, 
                correlation.DateTime.ToString(ci), 
                correlation.Value.ToString("0.0000", ci));

            Assert.AreEqual(expect, correlation.ToString());
        }
    }
}
