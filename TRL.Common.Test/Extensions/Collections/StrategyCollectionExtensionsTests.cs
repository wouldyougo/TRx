using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Configuration;
using TRL.Common.Models;
//using TRL.Common.Extensions.Models;
//using TRL.Common.Extensions.Data;
using TRL.Common.Extensions.Collections;

namespace TRL.Common.Extensions.Collections.Test
{
    [TestClass]
    public class StrategyCollectionExtensionsTests
    {
        private IDataContext tradingData;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            
            AddStrategies();
        }

        private void AddStrategies()
        {
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(new StrategyHeader(1, "First", "BP12345-RF-01", "RTS-9.13_FT", 10));
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(new StrategyHeader(2, "Second", "BP12345-RF-02", "RTS-9.13_FT", 10));
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(new StrategyHeader(3, "Third", "BP12345-RF-01", "RTS-9.13_FT", 10));
            Assert.AreEqual(3, this.tradingData.Get<ICollection<StrategyHeader>>().Count);
        }

        [TestMethod]
        public void StrategyCollection_GetAmount_test()
        {
            Assert.AreEqual(20, this.tradingData.Get<ICollection<StrategyHeader>>().GetAmount("BP12345-RF-01", "RTS-9.13_FT"));
            Assert.AreEqual(10, this.tradingData.Get<ICollection<StrategyHeader>>().GetAmount("BP12345-RF-02", "RTS-9.13_FT"));
        }
    }
}
