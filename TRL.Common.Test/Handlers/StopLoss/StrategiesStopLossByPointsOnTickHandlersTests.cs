using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common.Handlers;
using TRL.Common.Collections;
using TRL.Handlers.StopLoss;
using TRL.Logging;

namespace TRL.Common.Handlers.Test.StopLoss
{
    [TestClass]
    public class StrategiesStopLossByPointsOnTickHandlersTests
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private int strategiesCounter, stopPointsSettingsCounter, stopLossOrderSettingsCounter;

        private StrategiesStopLossByPointsOnTickHandlers handlers;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();
            this.strategiesCounter = 5;
            this.stopPointsSettingsCounter = 4;
            this.stopLossOrderSettingsCounter = 3;

            MakeAndAddStrategiesToTradingDataContext(this.strategiesCounter);
            Assert.AreEqual(this.strategiesCounter, this.tradingData.Get<IEnumerable<StrategyHeader>>().Count());

            MakeAndAddStopPointsSettingsToTradingDataContext(this.stopPointsSettingsCounter);
            Assert.AreEqual(this.stopPointsSettingsCounter, this.tradingData.Get<IEnumerable<StopPointsSettings>>().Count());

            MakeAndAddStopLossOrderSettingsToTradingDataContext(this.stopLossOrderSettingsCounter);
            Assert.AreEqual(this.stopLossOrderSettingsCounter, this.tradingData.Get<IEnumerable<StopLossOrderSettings>>().Count());

            this.handlers = new StrategiesStopLossByPointsOnTickHandlers(this.tradingData, this.signalQueue, new NullLogger());
        }

        private void MakeAndAddStrategiesToTradingDataContext(int count)
        {
            for (int i = 1; i <= count; i++)
                this.tradingData.Get<ICollection<StrategyHeader>>().Add(new StrategyHeader(i, i.ToString(), "ST12345-RF-01", "RTS-9.14", i));
        }

        private void MakeAndAddStopPointsSettingsToTradingDataContext(int count)
        {
            foreach (StrategyHeader strategyHeader in this.tradingData.Get<IEnumerable<StrategyHeader>>())
            {
                this.tradingData.Get<ICollection<StopPointsSettings>>().Add(new StopPointsSettings(strategyHeader, count, false));

                count--;
                if (count == 0)
                    break;
            }
        }

        private void MakeAndAddStopLossOrderSettingsToTradingDataContext(int count)
        {
            foreach (StrategyHeader strategyHeader in this.tradingData.Get<IEnumerable<StrategyHeader>>())
            {
                this.tradingData.Get<ICollection<StopLossOrderSettings>>().Add(new StopLossOrderSettings(strategyHeader, count));

                count--;
                if (count == 0)
                    break;
            }
        }

        [TestMethod]
        public void StrategiesStopLossOnTickHandlers_is_HashSet_test()
        {
            Assert.IsInstanceOfType(this.handlers, typeof(HashSet<StrategyStopLossByPointsOnTick>));
        }

        [TestMethod]
        public void handlers_only_for_strategies_with_StopPointsSettings_and_StopLossOrderSettings_test()
        {
            Assert.AreEqual(this.stopLossOrderSettingsCounter, this.handlers.Count);
            Assert.IsTrue(this.handlers.Any(h => h.Id == 1));
            Assert.IsTrue(this.handlers.Any(h => h.Id == 2));
            Assert.IsTrue(this.handlers.Any(h => h.Id == 3));
        }
    }
}
