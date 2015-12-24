using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class StrategyVolumeChangeStepTests
    {
        [TestMethod]
        public void StrategyVolumeChangeStep_constructor_test()
        {
            double stepValue = 10;

            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "SBRF-3.14_FT", 100);

            StrategyVolumeChangeStep step = new StrategyVolumeChangeStep(strategyHeader, stepValue);

            Assert.AreEqual(stepValue, step.Amount);
            Assert.AreEqual(strategyHeader.Id, step.Id);
            Assert.AreEqual(strategyHeader.Id, step.StrategyId);
            Assert.AreEqual(strategyHeader, step.Strategy);
        }
    }
}
