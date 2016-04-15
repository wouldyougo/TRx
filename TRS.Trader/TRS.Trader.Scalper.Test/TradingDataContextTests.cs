using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common.Collections;

namespace TRx.Trader.Scalper.Test
{
    public class Customer
    {
    }

    [TestClass]
    public class TradingDataContextTests
    {
        private IDataContext tradingData;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
        }

        [TestMethod]
        public void How_to_add_strategy_to_data_context_test()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<StrategyHeader>>().Count());

            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-3.14_FT", 10);
            
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<StrategyHeader>>().Count());
        }

        [TestMethod]
        public void Request_customers_collection_returns_null_test()
        {
            Assert.IsNull(this.tradingData.Get<IEnumerable<Customer>>());
        }

        [TestMethod]
        public void Link_handler_to_Tick_collection_test()
        {
            TickCounterHandler handler = new TickCounterHandler(this.tradingData);

            Assert.AreEqual(0, handler.TickCounter);

            Tick tick = new Tick("RTS-3.14_FT", DateTime.Now, 145000, 10);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);

            Assert.AreEqual(1, handler.TickCounter);

            Tick oneMoreTick = new Tick("Si-3.14_FT", DateTime.Now, 33500, 10, TradeAction.Sell);
            this.tradingData.Get<ICollection<Tick>>().Add(oneMoreTick);

            Assert.AreEqual(2, this.tradingData.Get<IEnumerable<Tick>>().Count());
            Assert.AreEqual(1, handler.TickCounter);
        }
    }
}
