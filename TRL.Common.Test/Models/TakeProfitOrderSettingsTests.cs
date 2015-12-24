using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class TakeProfitOrderSettingsTests
    {
        [TestMethod]
        public void TakeProfitOrderSettings_constructor_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-9.13_FT", 10);

            TakeProfitOrderSettings tp = new TakeProfitOrderSettings(strategyHeader, 3600);

            Assert.IsInstanceOfType(tp, typeof(OrderSettings));
            Assert.AreEqual(strategyHeader.Id, tp.Id);
            Assert.AreEqual(strategyHeader.Id, tp.StrategyId);
            Assert.AreEqual(strategyHeader, tp.Strategy);
            Assert.AreEqual(3600, tp.TimeToLive);
        }
    }
}
