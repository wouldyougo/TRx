using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using TRL.Common.Models;
using System.Collections.Generic;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class ArbitrageSettingsTests
    {
        private List<StrategyHeader> leftLeg;
        private List<StrategyHeader> rightLeg;

        private SpreadSettings spreadSettings;

        private ArbitrageSettings arbitrageSettings;

        [TestInitialize]
        public void SetUp()
        {
            leftLeg = new List<StrategyHeader>();
            rightLeg = new List<StrategyHeader>();

            this.leftLeg.Add(new StrategyHeader(1, "Left leg strategyHeader", "BP12345-RF-01", "RTS-12.13_FT", 10));
            this.rightLeg.Add(new StrategyHeader(2, "Right leg strategyHeader", "BP12345-RF-01", "Si-12.13_FT", 50));

            spreadSettings = new SpreadSettings(1.10, 1.11, 0.9);

            arbitrageSettings = new ArbitrageSettings(1, leftLeg, rightLeg, spreadSettings);
        }

        [TestMethod]
        public void ArbitrageSettings_constructor_test()
        {
            Assert.AreEqual(leftLeg, arbitrageSettings.LeftLeg);
            Assert.AreEqual(rightLeg, arbitrageSettings.RightLeg);
            Assert.AreEqual(1, arbitrageSettings.Id);
            Assert.AreEqual(spreadSettings, arbitrageSettings.SpreadSettings);
        }

        [TestMethod]
        public void ArbitrageSettings_has_symbol_with_exising_symbol()
        {
            Assert.IsTrue(arbitrageSettings.HasSymbol("RTS-12.13_FT"));
            Assert.IsTrue(arbitrageSettings.HasSymbol("Si-12.13_FT"));
        }

        [TestMethod]
        public void ArbitrageSettings_has_symbol_with_non_exising_symbol()
        {
            Assert.IsFalse(arbitrageSettings.HasSymbol("SBRF-12.13_FT"));
            Assert.IsFalse(arbitrageSettings.HasSymbol("GMNK-12.13_FT"));
        }

        [TestMethod]
        public void ArbitrageSettings_default_constructor_test()
        {
            ArbitrageSettings arbitrageSettings = new ArbitrageSettings();

            Assert.IsInstanceOfType(arbitrageSettings, typeof(IIdentified));
            Assert.IsTrue(arbitrageSettings.Id > 0);
            Assert.IsNotNull(arbitrageSettings.LeftLeg);
            Assert.IsNotNull(arbitrageSettings.RightLeg);
            Assert.AreEqual(0, arbitrageSettings.LeftLeg.Count());
            Assert.AreEqual(0, arbitrageSettings.RightLeg.Count());
            Assert.IsNotNull(arbitrageSettings.SpreadSettings);
            Assert.AreEqual(0, arbitrageSettings.SpreadSettings.FairPrice);
            Assert.AreEqual(0, arbitrageSettings.SpreadSettings.BuyBeforePrice);
            Assert.AreEqual(0, arbitrageSettings.SpreadSettings.SellAfterPrice);
        }
    }
}
