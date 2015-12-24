using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Collections;
using TRL.Common.Models;
using System.Collections.Generic;
//using TRL.Common.Extensions;
using TRL.Emulation;
using TRL.Common;
using TRL.Logging;

namespace TRx.Trader.BackTest.Test
{
    [TestClass]
    public class StopLossOnBarTests
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;

        private StrategyHeader strategyHeader;
        private BarSettings barSettings;
        private double points;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();

            this.strategyHeader = new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS-3.14_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.barSettings = new BarSettings(this.strategyHeader, this.strategyHeader.Symbol, 60, 3);
            this.tradingData.Get<ICollection<BarSettings>>().Add(this.barSettings);

            this.points = 100;

            Assert.IsFalse(this.tradingData.PositionExists(this.strategyHeader));
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        private void InitHandler()
        {
            StopLossOnBar handler =
                new StopLossOnBar(this.strategyHeader,
                    this.points,
                    this.tradingData,
                    this.signalQueue,
                    new NullLogger());
        }

        [TestMethod]
        public void StopLossOnBar_make_signal_to_close_long_position_test()
        {
            InitHandler();

            Signal openSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar(this.barSettings.Symbol,
                this.barSettings.Interval,
                DateTime.Now,
                140000,
                141000,
                139850,
                139890,
                35000));

            Assert.AreEqual(1, this.signalQueue.Count);
            Signal signal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, signal.StrategyId);
            Assert.AreEqual(139890, signal.Price);
            Assert.AreEqual(TradeAction.Sell, signal.TradeAction);
            Assert.AreEqual(OrderType.Market, signal.OrderType);
            Assert.AreEqual(this.strategyHeader.Amount, signal.Amount);
        }

        [TestMethod]
        public void StopLossOnBar_make_signal_to_close_short_position_test()
        {
            InitHandler();

            Signal openSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 141000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar(this.barSettings.Symbol,
                this.barSettings.Interval,
                DateTime.Now,
                141000,
                141500,
                140800,
                141110,
                35000));

            Assert.AreEqual(1, this.signalQueue.Count);
            Signal signal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, signal.StrategyId);
            Assert.AreEqual(141110, signal.Price);
            Assert.AreEqual(TradeAction.Buy, signal.TradeAction);
            Assert.AreEqual(OrderType.Market, signal.OrderType);
            Assert.AreEqual(this.strategyHeader.Amount, signal.Amount);
        }

        [TestMethod]
        public void StopLossOnBar_ignore_bar_when_close_price_greater_than_long_position_stop_test()
        {
            InitHandler();

            Signal openSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar(this.barSettings.Symbol,
                this.barSettings.Interval,
                DateTime.Now,
                140000,
                141000,
                139850,
                139910,
                35000));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void StopLossOnBar_ignore_bar_when_close_price_smaller_than_short_position_stop_test()
        {
            InitHandler();

            Signal openSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar(this.barSettings.Symbol,
                this.barSettings.Interval,
                DateTime.Now,
                140000,
                140090,
                138300,
                140090,
                35000));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void StopLossOnBar_ignore_bar_with_unmatched_symbol_test()
        {
            InitHandler();

            Signal openSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("Si-3.14_FT",
                this.barSettings.Interval,
                DateTime.Now,
                140000,
                141000,
                139850,
                139890,
                35000));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void StopLossOnBar_ignore_bar_with_unmatched_interval_test()
        {
            InitHandler();

            Signal openSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar(this.barSettings.Symbol,
                300,
                DateTime.Now,
                140000,
                141000,
                139850,
                139890,
                35000));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void StopLossOnBar_do_nothing_when_no_position_exists_test()
        {
            InitHandler();

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar(this.barSettings.Symbol,
                300,
                DateTime.Now,
                140000,
                141000,
                139850,
                139890,
                35000));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void StopLossOnBar_do_nothing_when_unfilled_strategy_orders_exists_test()
        {
            InitHandler();

            Signal openSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 141000, 0, 0);
            this.tradingData.AddSignalAndItsOrder(closeSignal);

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar(this.barSettings.Symbol,
                this.barSettings.Interval,
                DateTime.Now,
                140000,
                141000,
                139850,
                139890,
                35000));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void StopLossOnBar_do_nothing_when_no_BarSettings_exists_for_strategy_test()
        {

            Signal openSignal =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            this.tradingData.Get<ICollection<BarSettings>>().Clear();

            InitHandler();

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar(this.barSettings.Symbol,
                this.barSettings.Interval,
                DateTime.Now,
                140000,
                141000,
                139850,
                139890,
                35000));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

    }
}
