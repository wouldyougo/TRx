using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class SMASettingsTests
    {
        [TestMethod]
        public void SMASettings_constructor_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            SMASettings settings = new SMASettings(strategyHeader, 5, 10);

            Assert.IsTrue(settings.Id > 0);
            Assert.AreEqual(strategyHeader, settings.Strategy);
            Assert.AreEqual(strategyHeader.Id, settings.StrategyId);
            Assert.AreEqual(5, settings.PeriodFast);
            Assert.AreEqual(10, settings.PeriodSlow);
        }
    }
}
