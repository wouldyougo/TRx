using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common;
using TRL.Emulation;
using TRL.Logging;

namespace TRx.Handlers.Test
{
    [TestClass]
    public class HandlerDeviationTest
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;

        private StrategyHeader strategyHeader;
        private BarSettings barSettings;
        private double Period = 10;
        private IndicatorOnBarMaDeviation handler;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();

            this.strategyHeader = new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.barSettings = new BarSettings(this.strategyHeader, this.strategyHeader.Symbol, 60, 3);
            this.tradingData.Get<ICollection<BarSettings>>().Add(this.barSettings);

            handler =
                new IndicatorOnBarMaDeviation(this.strategyHeader,
                    this.tradingData,
                    this.Period,
                    new NullLogger());

            Assert.AreEqual(0, this.signalQueue.Count);
            Assert.IsNotNull(handler);
        }

        [TestMethod]
        public void IndicatorOnBarMaDeviation_value_count_test()
        {
            AddBreakToHighBars("RTS", this.tradingData.Get<ObservableCollection<Bar>>());

            Assert.AreEqual(3, this.handler.Ma.Count);
            Assert.AreEqual(3, this.handler.De.Count);
            Assert.AreEqual(3, this.handler.ValueMa.Count);
            Assert.AreEqual(3, this.handler.ValueDe.Count);
            Assert.AreEqual(15, Math.Round(this.handler.Ma[2]));
            Assert.AreEqual(1, Math.Round(this.handler.De[2]));
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

        //[TestMethod]
        //public void BreakOutOnBar_make_signal_to_sell_on_break_to_low_test()
        //{
        //    AddBreakToLowBars("RTS", this.tradingData.Get<ObservableCollection<Bar>>());

        //    //Assert.AreEqual(1, this.signalQueue.Count);

        //    //Signal signal = this.signalQueue.Dequeue();
        //    //Assert.AreEqual(this.strategyHeader.Id, signal.StrategyId);
        //    //Assert.AreEqual(TradeAction.Sell, signal.TradeAction);
        //    //Assert.AreEqual(OrderType.Market, signal.OrderType);
        //    //Assert.AreEqual(8, signal.Price);
        //}

        private void AddNeutralBars(string symbol, ObservableCollection<Bar> collection)
        {
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 2, 0), 12, 16, 11, 14, 100));
        }

        private void AddInsufficientBars(string symbol, ObservableCollection<Bar> collection)
        {
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
            collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
        }
    }
}
