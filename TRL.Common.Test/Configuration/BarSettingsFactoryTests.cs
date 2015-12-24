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
    public class BarSettingsFactoryTests
    {
        [TestMethod]
        public void Configuration_BarSettingsFactory_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-12.13_FT", 1);
            string prefix = "RTSX";
            
            IGenericFactory<BarSettings> factory =
                new BarSettingsFactory(strategyHeader, prefix);

            BarSettings bSettings = factory.Make();

            Assert.IsTrue(bSettings.Id > 0);
            Assert.AreEqual(strategyHeader.Id, bSettings.StrategyId);
            Assert.AreEqual(strategyHeader, bSettings.Strategy);
            Assert.AreEqual("Si-12.13_FT", bSettings.Symbol);
            Assert.AreEqual(3600, bSettings.Interval);
            Assert.AreEqual(35, bSettings.Period);
        }

        [TestMethod]
        public void Configuration_BarSettingsFactory_returns_null_for_nonexistent_prefix()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-12.13_FT", 1);
            string prefix = "RTSA";

            IGenericFactory<BarSettings> factory =
                new BarSettingsFactory(strategyHeader, prefix);

            Assert.IsNull(factory.Make());
        }
    }
}
