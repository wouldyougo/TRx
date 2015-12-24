using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common;
using TRL.Emulation;
using TRL.Logging;

namespace TRx.Trader.Scalper.Test
{
    [TestClass]
    public class BreakOutOnBarTests
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;

        private StrategyHeader strategyHeader;
        private BarSettings barSettings;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();

            this.strategyHeader = new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.barSettings = new BarSettings(this.strategyHeader, this.strategyHeader.Symbol, 60, 3);
            this.tradingData.Get<ICollection<BarSettings>>().Add(this.barSettings);

            BreakOutOnBar handler =
                new BreakOutOnBar(this.strategyHeader,
                    this.tradingData,
                    this.signalQueue,
                    new NullLogger());

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void BreakOutOnBar_make_signal_to_buy_on_break_to_high_test()
        {
            AddBreakToHighBars("RTS", this.tradingData.Get<ObservableCollection<Bar>>());

            Assert.AreEqual(1, this.signalQueue.Count);

            Signal signal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, signal.StrategyId);
            Assert.AreEqual(TradeAction.Buy, signal.TradeAction);
            Assert.AreEqual(OrderType.Market, signal.OrderType);
            Assert.AreEqual(19, signal.Price);
        }

        private void AddBreakToHighBars(string symbol, ObservableCollection<Bar> collection)
        {
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 2, 0), 12, 19, 11, 16, 100));
        }

        private void AddBreakToLowBars(string symbol, ObservableCollection<Bar> collection)
        {
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 2, 0), 12, 13, 8, 11, 100));
        }

        [TestMethod]
        public void BreakOutOnBar_make_signal_to_sell_on_break_to_low_test()
        {
            AddBreakToLowBars("RTS", this.tradingData.Get<ObservableCollection<Bar>>());

            Assert.AreEqual(1, this.signalQueue.Count);

            Signal signal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, signal.StrategyId);
            Assert.AreEqual(TradeAction.Sell, signal.TradeAction);
            Assert.AreEqual(OrderType.Market, signal.OrderType);
            Assert.AreEqual(8, signal.Price);
        }

        private void AddNeutralBars(string symbol, ObservableCollection<Bar> collection)
        {
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 2, 0), 12, 16, 11, 14, 100));
        }

        [TestMethod]
        public void BreakOutOnBar_ignore_neutral_bars_test()
        {
            AddNeutralBars("RTS", this.tradingData.Get<ObservableCollection<Bar>>());

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        private void AddInsufficientBars(string symbol, ObservableCollection<Bar> collection)
        {
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
        }

        [TestMethod]
        public void BreakOutOnBar_ignore_insufficient_quantity_of_bars_test()
        {
            AddInsufficientBars("RTS", this.tradingData.Get<ObservableCollection<Bar>>());

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void BreakOutOnBar_ignore_other_symbol_than_strategy_bars_test()
        {
            AddBreakToHighBars("Si", this.tradingData.Get<ObservableCollection<Bar>>());

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void BreakOutOnBar_do_nothing_if_position_exists_test()
        {
            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 10, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(signal);

            AddBreakToHighBars("RTS", this.tradingData.Get<ObservableCollection<Bar>>());

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void BreakOutOnBar_do_nothing_if_unfilled_strategy_order_exists_test()
        {
            Signal signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 10, 0, 0);
            this.tradingData.AddSignalAndItsOrder(signal);

            AddBreakToHighBars("RTS", this.tradingData.Get<ObservableCollection<Bar>>());

            Assert.AreEqual(0, this.signalQueue.Count);
        }

    }
}
