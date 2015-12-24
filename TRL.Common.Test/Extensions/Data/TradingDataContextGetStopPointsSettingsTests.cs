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
    public class TradingDataContextGetStopPointsSettingsTests:TraderBaseInitializer
    {
        private StrategyHeader strategy1, strategy2;

        [TestInitialize]
        public void Setup()
        {
            this.strategy1 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            this.strategy2 = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 2);

            StopPointsSettings sps1 = new StopPointsSettings(this.strategy1, 100, false);
            this.tradingData.Get<ICollection<StopPointsSettings>>().Add(sps1);
        }

        [TestMethod]
        public void TradingDataContext_GetStopPointsSettings_returns_null_test()
        {
            StopPointsSettings settings = this.tradingData.GetStopPointsSettings(this.strategy2);
            Assert.IsNull(settings);
        }

        [TestMethod]
        public void TradingDataContext_GetStopPointsSettings_returns_settings_test()
        {
            StopPointsSettings settings = this.tradingData.GetStopPointsSettings(this.strategy1);

            Assert.IsNotNull(settings);
            Assert.AreEqual(100, settings.Points);
            Assert.IsFalse(settings.Trail);
        }
    }
}
