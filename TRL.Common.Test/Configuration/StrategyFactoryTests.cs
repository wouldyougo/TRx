using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Configuration;

namespace TRL.Common.Test.Configuration
{
    [TestClass]
    public class StrategyFactoryTests
    {
        [TestMethod]
        public void Configuration_StrategyFactory_tests()
        {
            string prefix = "RTSX";

            IGenericFactory<StrategyHeader> factory = new StrategyFactory(prefix);

            StrategyHeader strategyHeader = factory.Make();

            Assert.AreEqual(1, strategyHeader.Id);
            Assert.AreEqual("Break out RTS", strategyHeader.Description);
            Assert.AreEqual("BP12345-RF-01", strategyHeader.Portfolio);
            Assert.AreEqual("RTS-9.13_FT", strategyHeader.Symbol);
            Assert.AreEqual(15, strategyHeader.Amount);
        }

        [TestMethod]
        public void Configuration_StrategyFactory_makes_null_for_nonexistent_prefix_tests()
        {
            string prefix = "RTSD";

            IGenericFactory<StrategyHeader> factory = new StrategyFactory(prefix);

            Assert.IsNull(factory.Make());
        }
    }
}
