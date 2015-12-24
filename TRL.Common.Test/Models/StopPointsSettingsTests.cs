using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class StopPointsSettingsTests
    {
        [TestMethod]
        public void StopLossSettings_constructor_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StopPointsSettings stopLossSettings = new StopPointsSettings(strategyHeader, 300, true);

            Assert.IsTrue(stopLossSettings is IIdentified);
            Assert.IsInstanceOfType(stopLossSettings, typeof(PointsSettings));
            Assert.AreEqual(strategyHeader.Id, stopLossSettings.Id);
            Assert.AreEqual(strategyHeader, stopLossSettings.Strategy);
            Assert.AreEqual(strategyHeader.Id, stopLossSettings.StrategyId);
            Assert.AreEqual(300, stopLossSettings.Points);
            Assert.IsInstanceOfType(stopLossSettings.Points, typeof(double));
            Assert.IsTrue(stopLossSettings.Trail);
        }
    }
}
