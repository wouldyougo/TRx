using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class StopLossOrderSettingsTests
    {
        [TestMethod]
        public void StopLossOrderSettings_constructor_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-9.13_FT", 10);

            StopLossOrderSettings sl = new StopLossOrderSettings(strategyHeader, 3600);

            Assert.IsInstanceOfType(sl, typeof(OrderSettings));
            Assert.AreEqual(strategyHeader.Id, sl.Id);
            Assert.AreEqual(strategyHeader.Id, sl.StrategyId);
            Assert.AreEqual(strategyHeader, sl.Strategy);
            Assert.AreEqual(3600, sl.TimeToLive);
        }
    }
}
