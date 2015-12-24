using System;
using System.Linq;
using TRL.Common.Extensions.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Handlers.Spreads;
using TRL.Logging;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Handlers.Test.Spreads
{
    [TestClass]
    public class CalculateSpreadOnOrderBookChangeTests
    {
        private StrategyHeader leftStrategy, rightStrategy;
        private List<StrategyHeader> leftLeg, rigthLeg;
        private SpreadSettings spreadSettings;
        private ArbitrageSettings arbitrageSettings;

        private IDataContext tradingData;
        private OrderBookContext orderBook;

        [TestInitialize]
        public void Setup()
        {
            this.leftLeg = new List<StrategyHeader>();
            this.rigthLeg = new List<StrategyHeader>();

            this.leftStrategy = new StrategyHeader(1, "Left leg", "BP12345-RF-01", "AB", 10);
            this.rightStrategy = new StrategyHeader(2, "Right leg", "BP1235-RF-01", "BA", 10);
            this.leftLeg.Add(this.leftStrategy);
            this.rigthLeg.Add(this.rightStrategy);

            this.spreadSettings = new SpreadSettings();
            this.arbitrageSettings = new ArbitrageSettings(1, this.leftLeg, this.rigthLeg, this.spreadSettings);

            this.orderBook = new OrderBookContext();
            this.tradingData = new TradingDataContext();

            CalculateSpreadOnOrderBookChange handler =
                new CalculateSpreadOnOrderBookChange(this.arbitrageSettings, this.orderBook, this.tradingData, new NullLogger());
        }

        [TestMethod]
        public void CalculateSpreadOnOrderBookChange_calculate_spread_test()
        {
            Assert.IsFalse(this.tradingData.Get<IEnumerable<SpreadValue>>().Any(s => s.Id == this.arbitrageSettings.Id));

            this.orderBook.Update(0, this.leftStrategy.Symbol, 5, 10, 6, 10);

            Assert.IsFalse(this.tradingData.Get<IEnumerable<SpreadValue>>().Any(s => s.Id == this.arbitrageSettings.Id));

            this.orderBook.Update(0, this.rightStrategy.Symbol, 4, 10, 5, 10);

            Assert.IsTrue(this.tradingData.Get<IEnumerable<SpreadValue>>().Any(s => s.Id == this.arbitrageSettings.Id));

            SpreadValue spreadValue = this.tradingData.Get<IEnumerable<SpreadValue>>().Last();
            Assert.AreEqual(this.arbitrageSettings.Id, spreadValue.Id);
            Assert.AreEqual(1.5, spreadValue.BuyBeforePrice);
            Assert.AreEqual(1, spreadValue.SellAfterPrice);            
        }

        [TestMethod]
        public void CalculateSpreadOnOrderBookChange_do_not_add_new_SpreadValue_if_previous_was_the_same_test()
        {
            this.tradingData.Get<ICollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, BrokerDateTime.Make(DateTime.Now), 1, 1.5));

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<SpreadValue>>().Count(s => s.Id == this.arbitrageSettings.Id));

            this.orderBook.Update(0, this.leftStrategy.Symbol, 5, 10, 6, 10);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<SpreadValue>>().Count(s => s.Id == this.arbitrageSettings.Id));

            this.orderBook.Update(0, this.rightStrategy.Symbol, 4, 10, 5, 10);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<SpreadValue>>().Count(s => s.Id == this.arbitrageSettings.Id));
        }

        [TestMethod]
        public void CalculateSpreadOnOrderBookChange_ignore_another_strategy_SpreadValue_test()
        {
            this.tradingData.Get<ICollection<SpreadValue>>().Add(new SpreadValue(355, BrokerDateTime.Make(DateTime.Now), 1, 1.5));

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<SpreadValue>>().Count(s => s.Id == this.arbitrageSettings.Id));

            this.orderBook.Update(0, this.leftStrategy.Symbol, 5, 10, 6, 10);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<SpreadValue>>().Count(s => s.Id == this.arbitrageSettings.Id));

            this.orderBook.Update(0, this.rightStrategy.Symbol, 4, 10, 5, 10);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<SpreadValue>>().Count(s => s.Id == this.arbitrageSettings.Id));
        }
    }
}
