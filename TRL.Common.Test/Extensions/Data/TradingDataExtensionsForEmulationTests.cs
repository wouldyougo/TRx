using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Emulation;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataExtensionsForEmulationTests
    {
        private StrategyHeader strategyHeader;
        private IDataContext tradingData;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader = new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS-3.14_FT", 1);

            this.tradingData = new TradingDataContext();

            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);
        }

        [TestMethod]
        public void AddSignalAndItsOrder_test()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());

            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);

            Order order = this.tradingData.AddSignalAndItsOrder(signal);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());

            Assert.AreEqual(signal.Id, order.SignalId);
            Assert.AreEqual(signal.Amount, order.Amount);
        }

        [TestMethod]
        public void Do_not_AddSignalAndItsOrder_when_no_signal_strategy_in_dataContext_test()
        {
            StrategyHeader anotherStrategy = new StrategyHeader(2, "Description two", "BP12345-RF-01", "RTS-3.14_FT", 10);

            Signal signal = new Signal(anotherStrategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);

            this.tradingData.AddSignalAndItsOrder(signal);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
        }

        [TestMethod]
        public void AddSignalAndItsOrderAndTrade_buy_with_price_and_quantity_test()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());

            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);

            double amount = 1;
            double price = 150010;

            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(signal, price, amount);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();
            Assert.AreEqual(signal.Id, order.SignalId);
            Assert.AreEqual(amount, order.FilledAmount);

            Assert.AreEqual(order.Id, trade.OrderId);
            Assert.AreEqual(price, trade.Price);
            Assert.AreEqual(amount, trade.Amount);
        }

        [TestMethod]
        public void AddSignalAndItsOrderAndTrade_sell_with_price_and_quantity_test()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());

            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);

            double amount = 1;
            double price = 150010;

            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(signal, price, amount);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();
            Assert.AreEqual(signal.Id, order.SignalId);
            Assert.AreEqual(amount, order.FilledAmount);
            Assert.IsTrue(order.IsFilled);

            Assert.AreEqual(order.Id, trade.OrderId);
            Assert.AreEqual(price, trade.Price);
            Assert.AreEqual(amount, -trade.Amount);
        }

        [TestMethod]
        public void Do_not_AddSignalAndItsOrderAndTrade_when_no_signal_strategy_in_dataContext_test()
        {
            StrategyHeader anotherStrategy = new StrategyHeader(2, "Description two", "BP12345-RF-01", "RTS-3.14_FT", 10);

            Signal signal = new Signal(anotherStrategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);

            this.tradingData.AddSignalAndItsOrderAndTrade(signal, 150050, 1);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
        }

        [TestMethod]
        public void AddSignalAndItsOrderAndTrade_change_order_FilledAmount_after_method_call_test()
        {
            StrategyHeader secondStrategy = new StrategyHeader(2, "Second strategyHeader", "BP12345-RF-01", "RTS-3.14_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(secondStrategy);

            Signal signal = new Signal(secondStrategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            Trade fTrade = this.tradingData.AddSignalAndItsOrderAndTrade(signal, 150000, 3);
            Assert.AreEqual(3, fTrade.Order.FilledAmount);

            Trade sTrade = this.tradingData.AddSignalAndItsOrderAndTrade(signal, 150000, 3);
            Assert.AreEqual(6, sTrade.Order.FilledAmount);
        }

        [TestMethod]
        public void AddSignalAndItsOrderAndTrade_add_signal_and_order_just_once_test()
        {
            StrategyHeader secondStrategy = new StrategyHeader(2, "Second strategyHeader", "BP12345-RF-01", "RTS-3.14_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(secondStrategy);

            Signal signal = new Signal(secondStrategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            Trade fTrade = this.tradingData.AddSignalAndItsOrderAndTrade(signal, 150000, 3);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());

            Trade sTrade = this.tradingData.AddSignalAndItsOrderAndTrade(signal, 150000, 3);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(2, this.tradingData.Get<IEnumerable<Trade>>().Count());
        }
    }
}
