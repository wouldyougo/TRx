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
    public class TakeProfitOrderSettingsFactoryTests
    {
        [TestMethod]
        public void Configuration_TakeProfitOrderSettingsFactory_make_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-12.13_FT", 1);
            string prefix = "RTSX";

            IGenericFactory<TakeProfitOrderSettings> factory =
                new TakeProfitOrderSettingsFactory(strategyHeader, prefix);

            TakeProfitOrderSettings tpoSettings = factory.Make();
            Assert.AreEqual(strategyHeader.Id, tpoSettings.Id);
            Assert.AreEqual(strategyHeader.Id, tpoSettings.StrategyId);
            Assert.AreEqual(strategyHeader, tpoSettings.Strategy);
            Assert.AreEqual(1900, tpoSettings.TimeToLive);
        }

        [TestMethod]
        public void Configuration_TakeProfitOrderSettingsFactory_returns_null_for_nonexistent_prefix()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-12.13_FT", 1);
            string prefix = "RTSA";

            IGenericFactory<TakeProfitOrderSettings> factory =
                new TakeProfitOrderSettingsFactory(strategyHeader, prefix);

            Assert.IsNull(factory.Make());
        }
    }
}
