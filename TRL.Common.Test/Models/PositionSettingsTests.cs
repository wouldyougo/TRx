using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class PositionSettingsTests
    {
        [TestMethod]
        public void PositionSettings_constructor_tests()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy sample", "BP12345-RF-01", "RTS-12.13_FT", 1);

            PositionSettings ps = new PositionSettings(strategyHeader, PositionType.Long);

            Assert.AreEqual(strategyHeader.Id, ps.Id);
            Assert.AreEqual(strategyHeader.Id, ps.StrategyId);
            Assert.AreEqual(strategyHeader, ps.Strategy);
            Assert.AreEqual(PositionType.Long, ps.PositionType);
        }
    }
}
