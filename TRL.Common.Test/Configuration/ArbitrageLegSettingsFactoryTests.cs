using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Configuration;

namespace TRL.Common.Test.Configuration
{
    [TestClass]
    public class ArbitrageLegSettingsFactoryTests
    {
        [TestMethod]
        public void Configuration_ArbitrageLegFactory_test()
        {
            string prefix = "SSTR_LeftLeg";
            IGenericFactory<IEnumerable<StrategyHeader>> factory =
                new ArbitrageLegSettingsFactory(prefix);

            IEnumerable<StrategyHeader> leftLeg = factory.Make();

            Assert.AreEqual(3, leftLeg.Count());
            Assert.AreEqual(81, leftLeg.ElementAt(0).Id);
            Assert.AreEqual(82, leftLeg.ElementAt(1).Id);
            Assert.AreEqual(83, leftLeg.ElementAt(2).Id);
            Assert.AreEqual("BP12345-RF-01", leftLeg.ElementAt(0).Portfolio);
            Assert.AreEqual("BP12345-RF-01", leftLeg.ElementAt(1).Portfolio);
            Assert.AreEqual("BP12345-RF-01", leftLeg.ElementAt(2).Portfolio);
            Assert.AreEqual("RTS-12.13_FT", leftLeg.ElementAt(0).Symbol);
            Assert.AreEqual("MIX-12.13_FT", leftLeg.ElementAt(1).Symbol);
            Assert.AreEqual("GOLD-12.13_FT", leftLeg.ElementAt(2).Symbol);
            Assert.AreEqual("Arbitrage spread left leg", leftLeg.ElementAt(0).Description);
            Assert.AreEqual("Arbitrage spread left leg", leftLeg.ElementAt(1).Description);
            Assert.AreEqual("Arbitrage spread left leg", leftLeg.ElementAt(2).Description);
            Assert.AreEqual(5, leftLeg.ElementAt(0).Amount);
            Assert.AreEqual(3, leftLeg.ElementAt(1).Amount);
            Assert.AreEqual(8, leftLeg.ElementAt(2).Amount);
        }

        [TestMethod]
        public void Configuration_ArbitrageLegFactory_three_portfolios_test()
        {
            string prefix = "SSTR_RightLeg";
            IGenericFactory<IEnumerable<StrategyHeader>> factory =
                new ArbitrageLegSettingsFactory(prefix);

            IEnumerable<StrategyHeader> leftLeg = factory.Make();

            Assert.AreEqual(3, leftLeg.Count());
            Assert.AreEqual(91, leftLeg.ElementAt(0).Id);
            Assert.AreEqual(92, leftLeg.ElementAt(1).Id);
            Assert.AreEqual(93, leftLeg.ElementAt(2).Id);
            Assert.AreEqual("BP12345-RF-01", leftLeg.ElementAt(0).Portfolio);
            Assert.AreEqual("BP12345-RF-02", leftLeg.ElementAt(1).Portfolio);
            Assert.AreEqual("BP12345-RF-03", leftLeg.ElementAt(2).Portfolio);
            Assert.AreEqual("Si-12.13_FT", leftLeg.ElementAt(0).Symbol);
            Assert.AreEqual("Eu-12.13_FT", leftLeg.ElementAt(1).Symbol);
            Assert.AreEqual("VTBR-12.13_FT", leftLeg.ElementAt(2).Symbol);
            Assert.AreEqual("Arbitrage spread right leg", leftLeg.ElementAt(0).Description);
            Assert.AreEqual("Arbitrage spread right leg", leftLeg.ElementAt(1).Description);
            Assert.AreEqual("Arbitrage spread right leg", leftLeg.ElementAt(2).Description);
            Assert.AreEqual(25, leftLeg.ElementAt(0).Amount);
            Assert.AreEqual(20, leftLeg.ElementAt(1).Amount);
            Assert.AreEqual(50, leftLeg.ElementAt(2).Amount);
        }

        [TestMethod]
        public void Configuration_ArbitrageLegFactory_returns_empty_collection_for_nonexistent_prefix_test()
        {
            string prefix = "SSTB_RightLeg";
            IGenericFactory<IEnumerable<StrategyHeader>> factory =
                new ArbitrageLegSettingsFactory(prefix);

            IEnumerable<StrategyHeader> leftLeg = factory.Make();

            Assert.AreEqual(0, leftLeg.Count());
        }
    }
}
