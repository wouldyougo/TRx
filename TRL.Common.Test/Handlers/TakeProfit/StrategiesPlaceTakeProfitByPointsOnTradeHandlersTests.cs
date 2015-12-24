using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common.Handlers;
using TRL.Common.Collections;
using TRL.Handlers.TakeProfit;
using TRL.Logging;

namespace TRL.Common.Handlers.Test.TakeProfit
{
    [TestClass]
    public class StrategiesPlaceTakeProfitByPointsOnTradeHandlersTests
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private int strategiesCounter, profitPointsSettingsCounter, takeProfitOrderSettingsCounter;

        private StrategiesPlaceTakeProfitByPointsOnTradeHandlers handlers;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();
            this.strategiesCounter = 5;
            this.profitPointsSettingsCounter = 4;
            this.takeProfitOrderSettingsCounter = 3;

            MakeAndAddStrategiesToTradingDataContext(this.strategiesCounter);
            Assert.AreEqual(this.strategiesCounter, this.tradingData.Get<IEnumerable<StrategyHeader>>().Count());

            MakeAndAddProfitPointsSettingsToTradingDataContext(this.profitPointsSettingsCounter);
            Assert.AreEqual(this.profitPointsSettingsCounter, this.tradingData.Get<IEnumerable<ProfitPointsSettings>>().Count());

            MakeAndAddTakeProfitOrderSettingsToTradingDataContext(this.takeProfitOrderSettingsCounter);
            Assert.AreEqual(this.takeProfitOrderSettingsCounter, this.tradingData.Get<IEnumerable<TakeProfitOrderSettings>>().Count());

            this.handlers = 
                new StrategiesPlaceTakeProfitByPointsOnTradeHandlers(this.tradingData, this.signalQueue, new NullLogger());
        }

        private void MakeAndAddStrategiesToTradingDataContext(int count)
        {
            for (int i = 1; i <= count; i++)
                this.tradingData.Get<ICollection<StrategyHeader>>().Add(new StrategyHeader(i, i.ToString(), "ST12345-RF-01", "RTS-9.14", i));
        }

        private void MakeAndAddProfitPointsSettingsToTradingDataContext(int count)
        {
            foreach (StrategyHeader strategyHeader in this.tradingData.Get<IEnumerable<StrategyHeader>>())
            {
                this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(new ProfitPointsSettings(strategyHeader, count, false));

                count--;
                if (count == 0)
                    break;
            }
        }

        private void MakeAndAddTakeProfitOrderSettingsToTradingDataContext(int count)
        {
            foreach (StrategyHeader strategyHeader in this.tradingData.Get<IEnumerable<StrategyHeader>>())
            {
                this.tradingData.Get<ICollection<TakeProfitOrderSettings>>().Add(new TakeProfitOrderSettings(strategyHeader, count));

                count--;
                if (count == 0)
                    break;
            }
        }

        [TestMethod]
        public void StrategiesTakeProfitByPointsOnTickHandlersis_HashSet_test()
        {
            Assert.IsInstanceOfType(this.handlers, typeof(HashSet<PlaceStrategyTakeProfitByPointsOnTrade>));
        }

        [TestMethod]
        public void handlers_only_for_strategies_with_ProfitPointsSettings_and_TakeProfitOrderSettings_test()
        {
            Assert.AreEqual(this.takeProfitOrderSettingsCounter, this.handlers.Count);
            Assert.IsTrue(this.handlers.Any(h => h.Id == 1));
            Assert.IsTrue(this.handlers.Any(h => h.Id == 2));
            Assert.IsTrue(this.handlers.Any(h => h.Id == 3));
        }
    }
}
