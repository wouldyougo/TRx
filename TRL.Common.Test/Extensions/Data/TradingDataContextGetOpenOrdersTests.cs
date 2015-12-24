using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.TimeHelpers;
using TRL.Common.Extensions.Data;
using TRL.Common.Collections;
using TRL.Emulation;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataContextGetOpenOrdersTests : TraderBaseInitializer
    {
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader = this.tradingData.Get<ICollection<StrategyHeader>>().Single(s => s.Id == 1);
            Assert.IsNotNull(this.strategyHeader);
        }

        [TestMethod]
        public void TradingDataContext_GetOpenOrders_returns_open_orders_for_strategy()
        {
            Assert.AreEqual(0, this.tradingData.GetOpenOrders(this.strategyHeader).Count());

            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.signalQueue.Enqueue(signal);

            Assert.AreEqual(1, this.tradingData.GetOpenOrders(this.strategyHeader).Count());
        }

        [TestMethod]
        public void TradingDataContext_GetOpenOrders_doesnt_returns_other_strategy_orders()
        {
            Assert.AreEqual(0, this.tradingData.GetOpenOrders(this.strategyHeader).Count());

            StrategyHeader strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 2);

            Signal signal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.signalQueue.Enqueue(signal);

            Assert.AreEqual(0, this.tradingData.GetOpenOrders(this.strategyHeader).Count());
        }

        [TestMethod]
        public void TradingDataContext_GetOpenOrders_doesnt_returns_strategy_close_orders()
        {
            Assert.AreEqual(0, this.tradingData.GetOpenOrders(this.strategyHeader).Count());

            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.signalQueue.Enqueue(signal);
            
            Order order = this.tradingData.Get<IEnumerable<Order>>().Single(o => o.SignalId == signal.Id);
            Assert.IsNotNull(order);

            Trade trade = new Trade(order, order.Portfolio, order.Symbol, 150010, this.strategyHeader.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(trade);

            Signal closeSignal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 151000, 0, 0);
            this.signalQueue.Enqueue(closeSignal);
            
            Assert.AreEqual(1, this.tradingData.GetOpenOrders(this.strategyHeader).Count());
            Assert.AreEqual(TradeAction.Buy, this.tradingData.GetOpenOrders(this.strategyHeader).First().TradeAction);
        }

    }
}
