using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class StrategyPositionTests
    {
        private StrategyHeader strategyHeader;

        private StrategyPosition strategyPosition;

        [TestInitialize]
        public void SetUp()
        {
            this.strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-3.14_FT", 10);

            this.strategyPosition = new StrategyPosition(strategyHeader);
        }

        [TestMethod]
        public void StrategyPosition_default_constructor_test()
        {
            Assert.IsInstanceOfType(strategyPosition, typeof(IIdentified));
            Assert.AreEqual(strategyHeader.Id, strategyPosition.Id);
            Assert.AreEqual(strategyHeader, strategyPosition.Strategy);
            Assert.AreEqual(strategyHeader.Id, strategyPosition.StrategyId);
            Assert.AreEqual(0, strategyPosition.Trades.Count());
            Assert.AreEqual(0, strategyPosition.Amount);
            Assert.AreEqual(0, strategyPosition.Sum);
        }
    }
}
