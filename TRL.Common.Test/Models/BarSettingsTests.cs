using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class BarSettingsTests
    {
        [TestMethod]
        public void BarSettings_Constructor_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Break out strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);

            BarSettings barSettings = new BarSettings(strategyHeader, "RTS-9.13_FT", 60, 15);

            Assert.IsTrue(barSettings.Id > 0);
            Assert.IsTrue(barSettings is IIdentified);
            Assert.AreEqual("RTS-9.13_FT", barSettings.Symbol);
            Assert.AreEqual(60, barSettings.Interval);
            Assert.IsInstanceOfType(barSettings.Interval, typeof(int));
            Assert.AreEqual(15, barSettings.Period);
            Assert.IsInstanceOfType(barSettings.Period, typeof(int));
            Assert.AreEqual(strategyHeader.Id, barSettings.StrategyId);
            Assert.AreSame(strategyHeader, barSettings.Strategy);
        }
    }
}
