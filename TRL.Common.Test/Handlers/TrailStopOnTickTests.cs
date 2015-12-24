using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Test.Mocks;
using TRL.Common.Extensions.Data;
using TRL.Emulation;
using TRL.Common.Collections;
using TRL.Common.Handlers;
using TRL.Logging;

namespace TRL.Common.Handlers.Test
{
    [TestClass]
    public class TrailStopOnTickTests
    {
        private IDataContext tradingData;
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Handlers_Setup()
        {
            this.tradingData = new TradingDataContext();

            this.strategyHeader = new StrategyHeader(1, "Description", "Portfolio", "Symbol", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            TrailStopOnTick handler =
                new TrailStopOnTick(this.strategyHeader,
                    this.tradingData,
                    new NullLogger());

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<MoveOrder>>().Count());
        }

        
        [TestMethod]
        public void Handlers_trail_long_position_stop_order_when_tick_price_is_greater_than_stop_test()
        {
            Signal oSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(oSignal);

            Signal sSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Stop, 150010, 149910, 0);
            this.tradingData.AddSignalAndItsOrder(sSignal);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 150020, 10));

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<MoveOrder>>().Count());
        }

        [TestMethod]
        public void Handlers_do_not_trail_long_position_stop_when_tick_price_is_lower_than_stop_test()
        {
            Signal oSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(oSignal);

            Signal sSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Stop, 150010, 149910, 0);
            this.tradingData.AddSignalAndItsOrder(sSignal);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 150020, 10));

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<MoveOrder>>().Count());
        }

        [TestMethod]
        public void Handlers_trail_short_position_stop_order_when_tick_price_move_down_test()
        {
            Signal oSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(oSignal);

            Signal sSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Stop, 150010, 150110, 0);
            this.tradingData.AddSignalAndItsOrder(sSignal);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 150000, 10));

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<MoveOrder>>().Count());
        }

    }
}
