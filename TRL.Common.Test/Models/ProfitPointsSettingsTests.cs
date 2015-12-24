using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class ProfitPointsSettingsTests
    {
        [TestMethod]
        public void TakeProfitSettings_constructor_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            double points = 100;
            bool trail = false;

            ProfitPointsSettings settings = new ProfitPointsSettings(strategyHeader, points, trail);

            Assert.AreEqual(strategyHeader.Id, settings.Id);
            Assert.AreEqual(strategyHeader.Id, settings.StrategyId);
            Assert.AreEqual(strategyHeader, settings.Strategy);
            Assert.AreEqual(points, settings.Points);
            Assert.AreEqual(trail, settings.Trail);
            Assert.IsInstanceOfType(settings, typeof(PointsSettings));
        }
    }
}
