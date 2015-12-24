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
    public class StopLossOrderSettingsFactoryTests
    {
        [TestMethod]
        public void Configuration_StopLossOrderSettingsFactory_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-12.13_FT", 1);
            string prefix = "RTSX";
            
            IGenericFactory<StopLossOrderSettings> factory =
                new StopLossOrderSettingsFactory(strategyHeader, prefix);

            StopLossOrderSettings sloSettings = factory.Make();

            Assert.AreEqual(strategyHeader.Id, sloSettings.Id);
            Assert.AreEqual(strategyHeader.Id, sloSettings.StrategyId);
            Assert.AreEqual(strategyHeader, sloSettings.Strategy);
            Assert.AreEqual(3600, sloSettings.TimeToLive);
        }

        [TestMethod]
        public void Configuration_StopLossOrderSettingsFactory_returns_null_for_nonexistent_prefix()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-12.13_FT", 1);
            string prefix = "RTSA";

            IGenericFactory<StopLossOrderSettings> factory =
                new StopLossOrderSettingsFactory(strategyHeader, prefix);

            Assert.IsNull(factory.Make());
        }
    }
}
