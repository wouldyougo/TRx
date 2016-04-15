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
using TRL.Logging;

namespace TRL.Common.Handlers.Test.StopLoss
{
    [TestClass]
    public class StrategyStopLossByPointsOnTickMeasureFromSignalPriceTests
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private StrategyHeader strategyHeader;
        private StopPointsSettings spSettings;
        private StopLossOrderSettings slSettings;
        private Signal buySignal, sellSignal;

        private StrategyStopLossByPointsOnTick handler;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();

            this.strategyHeader = new StrategyHeader(1, "Description", "ST12345-RF-01", "RTS-9.14", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.spSettings = new StopPointsSettings(this.strategyHeader, 100, false);
            this.tradingData.Get<ICollection<StopPointsSettings>>().Add(this.spSettings);

            this.slSettings = new StopLossOrderSettings(this.strategyHeader, 180);
            this.tradingData.Get<ICollection<StopLossOrderSettings>>().Add(this.slSettings);

            this.handler =
                new StrategyStopLossByPointsOnTick(this.strategyHeader, this.tradingData, this.signalQueue, new NullLogger(), true);
            
            this.buySignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            this.sellSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_when_just_unfilled_open_long_order_exists_test()
        {
            this.tradingData.AddSignalAndItsOrder(this.buySignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 124900, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_close_long_when_tick_price_greater_than_stop_price_test()
        {
            this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 124910, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_long_when_open_order_just_partially_filled_test()
        {
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal, this.buySignal.Price, 5);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 124900, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_long_when_unfilled_stop_order_exists_test()
        {
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal);

            Signal closeSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Stop, 124900, 0, 0);
            this.tradingData.AddSignalAndItsOrder(closeSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 124900, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }
        [TestMethod]
        public void make_no_signal_to_close_long_when_unfilled_market_order_exists_test()
        {
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal);
            this.tradingData.AddSignalAndItsOrder(this.sellSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 124900, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_long_when_partially_filled_stop_order_exists_test()
        {
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Stop, 124900, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal, closeSignal.Price, 5);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 124900, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_signal_to_close_long_position_with_single_open_trade_test()
        {
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal, this.buySignal.Price - 10);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 124900, 11, TradeAction.Sell);
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
        public void make_signal_to_close_long_position_with_multiple_open_trades_test()
        {
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal, this.buySignal.Price - 20, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal, this.buySignal.Price - 20, 5);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 124900, 11, TradeAction.Sell);
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
        public void make_signal_to_close_long_position_with_multiple_open_trades_and_cancelled_open_order_test()
        {
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal, this.buySignal.Price - 10, 5);
            this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal, this.buySignal.Price - 10, 3);

            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));
            firstTrade.Order.Cancel(DateTime.Now, "cancel order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 124900, 11, TradeAction.Sell);
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
        public void make_signal_to_close_single_trade_long_when_unfilled_take_profit_order_exists_test()
        {
            Trade openTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal, this.buySignal.Price - 20);

            Signal tpSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Limit, 125000, 0, 125500);
            Order tpOrder = this.tradingData.AddSignalAndItsOrder(tpSignal);


            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 124900, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal slSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, slSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, slSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, slSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, slSignal.OrderType);
            Assert.AreEqual(tick.Price, slSignal.Price);
            Assert.AreEqual(openTrade.Amount, slSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_close_multiple_trades_long_when_unfilled_take_profit_order_exists_test()
        {
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal, this.buySignal.Price - 10, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal, this.buySignal.Price - 10, 5);

            Signal tpSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Limit, 125000, 0, 125500);
            Order tpOrder = this.tradingData.AddSignalAndItsOrder(tpSignal);


            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 124900, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal slSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, slSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, slSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, slSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, slSignal.OrderType);
            Assert.AreEqual(tick.Price, slSignal.Price);
            Assert.AreEqual(firstTrade.Amount + secondTrade.Amount, slSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_close_multiple_trades_long_with_partially_canceled_open_order_when_unfilled_take_profit_order_exists_test()
        {
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal, this.buySignal.Price - 20, 2);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.buySignal, this.buySignal.Price - 20, 3);
            firstTrade.Order.Cancel(DateTime.Now, "Cancel partially filled order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);

            Signal tpSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Limit, 125000, 0, 125500);
            Order tpOrder = this.tradingData.AddSignalAndItsOrder(tpSignal);


            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 124900, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal slSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, slSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, slSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, slSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, slSignal.OrderType);
            Assert.AreEqual(tick.Price, slSignal.Price);
            Assert.AreEqual(firstTrade.Amount + secondTrade.Amount, slSignal.Amount);
        }

        [TestMethod]
        public void make_no_signal_to_buy_when_no_short_position_exists_and_an_unfilled_open_order_exists_test()
        {
            this.tradingData.AddSignalAndItsOrder(this.sellSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 125100, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_short_when_open_order_just_partially_filled_test()
        {
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal, this.sellSignal.Price, 5);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 125100, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_short_when_unfilled_market_order_exists_test()
        {
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal);
            Order marketOrder = this.tradingData.AddSignalAndItsOrder(this.buySignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 125100, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_short_when_unfilled_stop_order_exists_test()
        {
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Stop, 125100, 0, 0);
            this.tradingData.AddSignalAndItsOrder(closeSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 125100, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_to_close_short_when_partially_filled_stop_order_exists_test()
        {
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Stop, 125100, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal, closeSignal.Price, 5);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 125100, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_signal_to_buy_on_stop_for_short_position_with_single_open_trade_test()
        {
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal, this.sellSignal.Price + 20);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 125100, 11, TradeAction.Sell);
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
        public void make_signal_to_buy_on_stop_for_short_position_with_multiple_open_trades_test()
        {
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal, this.sellSignal.Price + 20, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal, this.sellSignal.Price + 20, 5);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 125100, 11, TradeAction.Sell);
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
        public void make_signal_to_buy_on_stop_for_short_position_with_multiple_open_trades_and_cancelled_open_order_test()
        {
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal, this.sellSignal.Price + 20, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal, this.sellSignal.Price + 20, 3);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));
            firstTrade.Order.Cancel(DateTime.Now, "cancel order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 125100, 11, TradeAction.Sell);
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
        public void make_no_signal_to_buy_when_tick_price_smaller_than_stop_price_test()
        {
            this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal, this.sellSignal.Price + 20);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 125090, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_signal_to_close_single_trade_short_position_when_tporder_exists_test()
        {
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal, this.sellSignal.Price + 20);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Signal tpSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Limit, 124000, 0, 124000);
            Order tpOrder = this.tradingData.AddSignalAndItsOrder(tpSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 125100, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal slSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, slSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, slSignal.Strategy);
            Assert.AreEqual(TradeAction.Buy, slSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, slSignal.OrderType);
            Assert.AreEqual(tick.Price, slSignal.Price);
            Assert.AreEqual(amount, slSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_close_multiple_trade_short_position_when_tporder_exists_test()
        {
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal, this.sellSignal.Price + 20, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal, this.sellSignal.Price + 20, 5);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Signal tpSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Limit, 124000, 0, 124000);
            Order tpOrder = this.tradingData.AddSignalAndItsOrder(tpSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 125100, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal slSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, slSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, slSignal.Strategy);
            Assert.AreEqual(TradeAction.Buy, slSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, slSignal.OrderType);
            Assert.AreEqual(tick.Price, slSignal.Price);
            Assert.AreEqual(amount, slSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_close_multiple_trade_with_partially_filled_and_cancelled_open_order_short_position_when_tporder_exists_test()
        {
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal, this.sellSignal.Price + 20, 3);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(this.sellSignal, this.sellSignal.Price + 20, 4);
            firstTrade.Order.Cancel(DateTime.Now, "Cancel partially filled open order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Signal tpSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Limit, 124000, 0, 124000);
            Order tpOrder = this.tradingData.AddSignalAndItsOrder(tpSignal);

            Tick tick = new Tick(this.strategyHeader.Symbol, DateTime.Now, 125100, 11, TradeAction.Sell);
            this.tradingData.Get<ObservableCollection<Tick>>().Add(tick);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal slSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, slSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, slSignal.Strategy);
            Assert.AreEqual(TradeAction.Buy, slSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, slSignal.OrderType);
            Assert.AreEqual(tick.Price, slSignal.Price);
            Assert.AreEqual(amount, slSignal.Amount);
        }

    }
}
