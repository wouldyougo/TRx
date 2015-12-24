using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Extensions.Data;
using TRL.Emulation;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataContextGetProfitPointsSettingsTests : TraderBaseInitializer
    {
        private StrategyHeader strategy1, strategy2;

        [TestInitialize]
        public void Setup()
        {
            this.strategy1 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            this.strategy2 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 2);

            ProfitPointsSettings pps1 = new ProfitPointsSettings(this.strategy1, 100, false);
            this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(pps1);
        }

        [TestMethod]
        public void TradingDataContext_GetProfitPointsSettings_returns_null_test()
        {
            ProfitPointsSettings settings = this.tradingData.GetProfitPointsSettings(this.strategy2);
            Assert.IsNull(settings);
        }

        [TestMethod]
        public void TradingDataContext_GetProfitPointsSettings_returns_settings_test()
        {
            ProfitPointsSettings settings = this.tradingData.GetProfitPointsSettings(this.strategy1);

            Assert.IsNotNull(settings);
            Assert.AreEqual(100, settings.Points);
            Assert.IsFalse(settings.Trail);
        }
    }
}
