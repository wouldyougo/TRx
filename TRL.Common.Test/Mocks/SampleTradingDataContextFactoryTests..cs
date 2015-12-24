using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Emulation;

namespace TRL.Common.Test.Mocks
{
    [TestClass]
    public class SampleTradingDataContextFactoryTests
    {
        private IDataContext tdContext;

        [TestInitialize]
        public void Setup()
        {
            this.tdContext = SampleTradingDataContextFactory.Make();
        }

        [TestMethod]
        public void TradingContext_contains_three_strategies_test()
        {
            Assert.AreEqual(5, this.tdContext.Get<IEnumerable<StrategyHeader>>().Count());
            Assert.IsTrue(this.tdContext.Get<IEnumerable<StrategyHeader>>().Any(s => s.Symbol.Equals("RTS-12.13_FT")));
            Assert.IsTrue(this.tdContext.Get<IEnumerable<StrategyHeader>>().Any(s => s.Symbol.Equals("Si-12.13_FT")));
            Assert.IsTrue(this.tdContext.Get<IEnumerable<StrategyHeader>>().Any(s => s.Symbol.Equals("Eu-12.13_FT")));
            Assert.IsTrue(this.tdContext.Get<IEnumerable<StrategyHeader>>().Any(s => s.Symbol.Equals("SBRF-12.13_FT")));
            Assert.IsTrue(this.tdContext.Get<IEnumerable<StrategyHeader>>().Any(s => s.Symbol.Equals("SBPR-12.13_FT")));
        }

        [TestMethod]
        public void TradingContext_contains_SymbolSettings_for_strategies_test()
        {
            Assert.AreEqual(5, this.tdContext.Get<IEnumerable<Symbol>>().Count());

            foreach (StrategyHeader strategyHeader in this.tdContext.Get<IEnumerable<StrategyHeader>>())
                Assert.IsTrue(this.tdContext.Get<IEnumerable<Symbol>>().Any(s => s.Name.Equals(strategyHeader.Symbol)));
        }
    }
}
