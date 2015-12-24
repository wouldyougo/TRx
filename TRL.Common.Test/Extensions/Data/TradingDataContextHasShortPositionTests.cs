using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Models;
using TRL.Emulation;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataContextHasShortPositionTests
    {
        private IDataContext tradingData;
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup() 
        {
            this.tradingData = new TradingDataContext();
            
            this.strategyHeader = new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS-3.14_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);
        }

        [TestMethod]
        public void TradingDataContext_has_no_short_position_when_no_any_position_exists_test()
        {
            Assert.IsFalse(this.tradingData.HasShortPosition(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContext_has_no_short_position_when_there_is_no_trades_for_strategy_test()
        {
            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.AddSignalAndItsOrder(signal);

            Assert.IsFalse(this.tradingData.HasShortPosition(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContext_has_short_position_test()
        {
            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(signal);

            Assert.IsTrue(this.tradingData.HasShortPosition(this.strategyHeader));
        }
    }
}
