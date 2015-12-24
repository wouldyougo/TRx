using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Extensions.Data;
using TRL.Emulation;

namespace TRL.Common.Test
{
    [TestClass]
    public class TraderBaseInitializerTests:TraderBaseInitializer
    {
        [TestMethod]
        public void Handlers_TraderBaseInitializer_EmulateTradeFor_buy_Signal_test()
        {
            StrategyHeader strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            Signal signal = new Signal(strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.signalQueue.Enqueue(signal);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(0, this.tradingData.GetAmount(strategyHeader));

            EmulateTradeFor(signal);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(1, this.tradingData.GetAmount(strategyHeader));
        }

        [TestMethod]
        public void Handlers_TraderBaseInitializer_Any_EmulateTradeFor_makes_OrderDeliviryConfirmation_test()
        {
            StrategyHeader strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            Signal signal = new Signal(strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);

            EmulateTradeFor(signal);

            Order order = this.tradingData.Get<IEnumerable<Order>>().Last();
            Assert.AreEqual(signal.Id, order.SignalId);
            Assert.IsTrue(order.IsDelivered);
        }

        [TestMethod]
        public void Handlers_TraderBaseInitializer_EmulateTradeFor_nonexistent_buy_Signal_test()
        {
            StrategyHeader strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            Signal signal = new Signal(strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(0, this.tradingData.GetAmount(strategyHeader));

            EmulateTradeFor(signal);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(1, this.tradingData.GetAmount(strategyHeader));
        }

        [TestMethod]
        public void Handlers_TraderBaseInitializer_EmulateTradeFor_buy_Signal_with_price_test()
        {
            StrategyHeader strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            Signal signal = new Signal(strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(0, this.tradingData.GetAmount(strategyHeader));

            double price = 150010;
            EmulateTradeFor(signal, price);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(1, this.tradingData.GetAmount(strategyHeader));

            Trade trade = this.tradingData.Get<IEnumerable<Trade>>().Last();
            Assert.AreEqual(price, trade.Price);
        }

        [TestMethod]
        public void Handlers_TraderBaseInitializer_EmulateTradeFor_buy_Signal_with_price_and_amount_test()
        {
            StrategyHeader strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 2);
            Signal signal = new Signal(strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 32000, 0, 0);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(0, this.tradingData.GetAmount(strategyHeader));

            double price = 32001;
            double amount = 1;
            EmulateTradeFor(signal, price, amount);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(1, this.tradingData.GetAmount(strategyHeader));

            Trade trade = this.tradingData.Get<IEnumerable<Trade>>().Last();
            Assert.AreEqual(price, trade.Price);
            Assert.AreEqual(amount, trade.Amount);
        }

        [TestMethod]
        public void Handlers_TraderBaseInitializer_EmulateTradeFor_sell_Signal_test()
        {
            StrategyHeader strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            Signal signal = new Signal(strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            this.signalQueue.Enqueue(signal);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(0, this.tradingData.GetAmount(strategyHeader));

            EmulateTradeFor(signal);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(-1, this.tradingData.GetAmount(strategyHeader));
        }

        [TestMethod]
        public void Handlers_TraderBaseInitializer_EmulateTradeFor_sell_Signal_with_price_test()
        {
            StrategyHeader strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            Signal signal = new Signal(strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            this.signalQueue.Enqueue(signal);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(0, this.tradingData.GetAmount(strategyHeader));

            double price = 150000;

            EmulateTradeFor(signal, price);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(-1, this.tradingData.GetAmount(strategyHeader));

            Trade trade = this.tradingData.Get<IEnumerable<Trade>>().Last();
            Assert.AreEqual(price, trade.Price);
        }

        [TestMethod]
        public void Handlers_TraderBaseInitializer_EmulateTradeFor_sell_Signal_with_price_and_amount_test()
        {
            StrategyHeader strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 2);
            Signal signal = new Signal(strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 32000, 0, 0);
            this.signalQueue.Enqueue(signal);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(0, this.tradingData.GetAmount(strategyHeader));

            double price = 32001;
            double amount = 1;

            EmulateTradeFor(signal, price, amount);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(-1, this.tradingData.GetAmount(strategyHeader));

            Trade trade = this.tradingData.Get<IEnumerable<Trade>>().Last();
            Assert.AreEqual(price, trade.Price);
            Assert.AreEqual(-amount, trade.Amount);
        }

        [TestMethod]
        public void Handlers_TraderBaseInitializer_EmulateTradeFor_nonexistent_sell_Signal_test()
        {
            StrategyHeader strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);
            Signal signal = new Signal(strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(0, this.tradingData.GetAmount(strategyHeader));

            EmulateTradeFor(signal);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(-1, this.tradingData.GetAmount(strategyHeader));
        }
    }
}
