using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class PointsSettingsTests
    {
        [TestMethod]
        public void ClosePointsSettings_constructor_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-9.13_FT", 8);

            PointsSettings settings = new PointsSettings(strategyHeader, 350, true);

            Assert.AreEqual(strategyHeader.Id, settings.Id);
            Assert.AreSame(strategyHeader, settings.Strategy);
            Assert.AreEqual(strategyHeader.Id, settings.StrategyId);
            Assert.AreEqual(350, settings.Points);
            Assert.IsTrue(settings.Trail);
        }
    }
}
