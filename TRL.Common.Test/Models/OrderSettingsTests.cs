using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class OrderSettingsTests
    {
        [TestMethod]
        public void OrderSettings_constructor_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            int timeToLiveSeconds = 60;

            OrderSettings orderSettings = new OrderSettings(strategyHeader, timeToLiveSeconds);

            Assert.IsTrue(orderSettings.Id > 0);
            Assert.AreEqual(strategyHeader, orderSettings.Strategy);
            Assert.AreEqual(strategyHeader.Id, orderSettings.StrategyId);
            Assert.AreEqual(timeToLiveSeconds, orderSettings.TimeToLive);
        }
    }
}
