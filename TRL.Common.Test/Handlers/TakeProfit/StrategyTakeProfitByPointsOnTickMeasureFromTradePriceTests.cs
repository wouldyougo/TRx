using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common.Collections;
using System.Linq;
using TRL.Common.Handlers;
using TRL.Emulation;
using TRL.Common.Extensions.Data;
using TRL.Handlers.StopLoss;
using TRL.Handlers.TakeProfit;
using TRL.Logging;

namespace TRL.Common.Handlers.Test.TakeProfit
{
    [TestClass]
    public class StrategyTakeProfitByPointsOnTickMeasureFromTradePriceTests
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private StrategyHeader strategyHeader;
        private ProfitPointsSettings ppSettings;
        private TakeProfitOrderSettings tpoSettings;

        private StrategyTakeProfitByPointsOnTick handler;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();

            this.strategyHeader = new StrategyHeader(1, "Description", "ST12345-RF-01", "RTS-9.14", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.ppSettings = new ProfitPointsSettings(this.strategyHeader, 100, false);
            this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(this.ppSettings);

            this.tpoSettings = new TakeProfitOrderSettings(this.strategyHeader, 180);
            this.tradingData.Get<ICollection<TakeProfitOrderSettings>>().Add(this.tpoSettings);

            this.handler =
                new StrategyTakeProfitByPointsOnTick(this.strategyHeader, this.tradingData, this.signalQueue, new NullLogger());
            
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void StrategyTakeProfitByPointsOnTick_is_Identified_test()
        {
            Assert.IsInstanceOfType(this.handler, typeof(IIdentified));
            Assert.AreEqual(this.strategyHeader.Id, this.handler.Id);
        }

        [TestMethod]
        public void do_nothing_when_no_position_exists_test()
        {
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Buy, 120000, 31));
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_sell_when_no_long_position_exists_and_open_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            this.tradingData.AddSignalAndItsOrder(openSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 125100, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_sell_when_tick_price_smaller_than_profit_price_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 125090, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_long_when_open_order_just_partially_filled_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 125100, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_long_when_unfilled_profit_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125100, 0, 0);
            this.tradingData.AddSignalAndItsOrder(closeSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 125100, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_long_when_partially_filled_profit_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125100, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal, closeSignal.Price, 5);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 125100, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_signal_to_close_long_when_partially_filled_stop_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal stopSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Stop, 124900, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(stopSignal, stopSignal.Price, 5);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 125100, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, closeSignal.OrderType);
            Assert.AreEqual(tick.Price, closeSignal.Price);
            Assert.AreEqual(amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_sell_for_long_position_with_single_open_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 125100, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, closeSignal.OrderType);
            Assert.AreEqual(tick.Price, closeSignal.Price);
            Assert.AreEqual(amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_sell_for_long_position_with_multiple_open_trades_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 125100, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, closeSignal.OrderType);
            Assert.AreEqual(tick.Price, closeSignal.Price);
            Assert.AreEqual(amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_sell_for_long_position_with_multiple_open_trades_and_cancelled_open_order_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 3);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));
            firstTrade.Order.Cancel(DateTime.Now, "cancel order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 125100, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, closeSignal.OrderType);
            Assert.AreEqual(tick.Price, closeSignal.Price);
            Assert.AreEqual(amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_no_signal_to_buy_when_no_short_position_exists_and_unfilled_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            this.tradingData.AddSignalAndItsOrder(openSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 124900, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_short_when_open_order_just_partially_filled_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 124900, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_short_when_unfilled_market_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 124900, 0, 0);
            this.tradingData.AddSignalAndItsOrder(closeSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 124900, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_short_when_unfilled_limit_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Limit, 124900, 0, 124900);
            this.tradingData.AddSignalAndItsOrder(closeSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 124900, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_short_when_partially_filled_market_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 124900, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal, closeSignal.Price, 5);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 124900, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_short_when_partially_filled_limit_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Limit, 124900, 0, 124900);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal, closeSignal.Price, 5);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 124900, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_signal_to_buy_for_short_position_with_single_open_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 124900, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Buy, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, closeSignal.OrderType);
            Assert.AreEqual(tick.Price, closeSignal.Price);
            Assert.AreEqual(amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_buy_for_short_position_with_multiple_open_trades_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 124900, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Buy, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, closeSignal.OrderType);
            Assert.AreEqual(tick.Price, closeSignal.Price);
            Assert.AreEqual(amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_buy_for_short_position_with_multiple_open_trades_and_cancelled_open_order_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 3);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));
            firstTrade.Order.Cancel(DateTime.Now, "cancel order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 124900, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Buy, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, closeSignal.OrderType);
            Assert.AreEqual(tick.Price, closeSignal.Price);
            Assert.AreEqual(amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_no_signal_to_buy_when_tick_price_greater_than_profit_price_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, TradeAction.Sell, 124990, 11);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

    }
}
