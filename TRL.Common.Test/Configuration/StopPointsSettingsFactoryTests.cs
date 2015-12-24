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
    public class StopPointsSettingsFactoryTests
    {
        [TestMethod]
        public void Configuration_StopPointsSettingsFactory_makes_settings_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS-12.13_FT", 1);

            string prefix = "RTSX";

            IGenericFactory<StopPointsSettings> spSettingsFactory = new StopPointsSettingsFactory(strategyHeader, prefix);

            StopPointsSettings spSettings = spSettingsFactory.Make();

            Assert.AreEqual(strategyHeader.Id, spSettings.Id);
            Assert.AreEqual(strategyHeader.Id, spSettings.StrategyId);
            Assert.AreEqual(strategyHeader, spSettings.Strategy);
            Assert.AreEqual(500, spSettings.Points);
            Assert.AreEqual(false, spSettings.Trail);
        }

        [TestMethod]
        public void Configuration_StopPointsSettingsFactory_makes_null_for_nonexistent_prefix()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS-12.13_FT", 1);

            string prefix = "RTSA";

            IGenericFactory<StopPointsSettings> spSettingsFactory = new StopPointsSettingsFactory(strategyHeader, prefix);

            Assert.IsNull(spSettingsFactory.Make());
        }
    }
}
