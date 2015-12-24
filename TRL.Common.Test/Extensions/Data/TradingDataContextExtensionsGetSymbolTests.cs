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
    public class TradingDataContextExtensionsGetSymbolTests:TraderBaseInitializer
    {
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
        }

        [TestMethod]
        public void TradingDataContext_GetSymbol_returns_symbol_test()
        {
            Symbol symbol = this.tradingData.GetSymbol(this.strategyHeader.Symbol);

            Assert.IsNotNull(symbol);
            Assert.AreEqual(symbol.Name, this.strategyHeader.Symbol);
        }
    }
}
