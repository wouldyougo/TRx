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
    public class OrderSettingsFactoryTests
    {
        [TestMethod]
        public void Configuration_OrderSettingsFactory_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-12.13_FT", 1);
            string prefix = "RTSX";
            
            IGenericFactory<OrderSettings> factory =
                new OrderSettingsFactory(strategyHeader, prefix);

            OrderSettings sloSettings = factory.Make();

            Assert.AreEqual(strategyHeader.Id, sloSettings.Id);
            Assert.AreEqual(strategyHeader.Id, sloSettings.StrategyId);
            Assert.AreEqual(strategyHeader, sloSettings.Strategy);
            Assert.AreEqual(86400, sloSettings.TimeToLive);
        }

        [TestMethod]
        public void Configuration_OrderSettingsFactory_returns_null_for_nonexistent_prefix()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-12.13_FT", 1);
            string prefix = "RTSA";

            IGenericFactory<OrderSettings> factory =
                new OrderSettingsFactory(strategyHeader, prefix);

            Assert.IsNull(factory.Make());
        }
    }
}
