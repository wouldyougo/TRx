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
    public class ProfitPointsSettingsFactoryTests
    {
        [TestMethod]
        public void Configuration_ProfitPointsSettingsFactory_makes_settings_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS-12.13_FT", 1);

            string prefix = "RTSX";

            IGenericFactory<ProfitPointsSettings> factory = new ProfitPointsSettingsFactory(strategyHeader, prefix);

            ProfitPointsSettings ppSettings = factory.Make();

            Assert.AreEqual(strategyHeader.Id, ppSettings.Id);
            Assert.AreEqual(strategyHeader.Id, ppSettings.StrategyId);
            Assert.AreEqual(strategyHeader, ppSettings.Strategy);
            Assert.AreEqual(1000, ppSettings.Points);
            Assert.AreEqual(false, ppSettings.Trail);
        }

        [TestMethod]
        public void Configuration_ProfitPointsSettingsFactory_makes_null_for_nonexistent_prefix()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS-12.13_FT", 1);

            string prefix = "RTSA";

            IGenericFactory<ProfitPointsSettings> factory = new ProfitPointsSettingsFactory(strategyHeader, prefix);

            Assert.IsNull(factory.Make());
        }
    }
}
