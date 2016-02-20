using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Common.Models;
using System.Collections.Generic;
using System.Linq;
using TRL.Common;
using TRL.Emulation;
using TRL.Logging;
using TRx.Indicators;

namespace TRx.Handlers.Test
{
    public class DataSourceTest : DataOutput1<double>
    {
        public DataSourceTest(IList<double> source) : base(source) {}
    }

    [TestClass]
    public class HandlerIndicatorMaDexTest
    {
        //private IDataContext tradingData;
        //private ObservableQueue<Signal> signalQueue;

        //private StrategyHeader strategyHeader;
        //private BarSettings barSettings;
        private DataInput<double> dataInput;
        private DataSourceTest dataSource;
        private double Period = 5;
        private IndicatorMaDe handler;
        private IndicatorMaDex maxs;
        private int series;
        List<double> source;
        IList<double> period;


        [TestInitialize]
        public void Setup()
        {
            ///List<double>(10) { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90 }
            source = new List<double>(10);
            dataSource = new DataSourceTest(source);
            dataInput = new DataInput<double>(dataSource);

            handler = new IndicatorMaDe(this.Period, this.dataInput, new NullLogger());

            //Assert.AreEqual(0, this.signalQueue.Count);
            Assert.IsNotNull(handler);

            Assert.AreEqual(0, handler.Input.Value.Count);
            for (int i = 0; i < 10; i++)
            {
                source.Add(10 * i);
            }

            ///IndicatorMACDS
            series = 7;
            ///List<double>(7) { 0, 10, 20, 30, 40, 50, 60 }
            period = new List<double>(series);
            for (int i = 0; i < series; i++)
            {
                period.Add(10 * i);
            }

            maxs = new IndicatorMaDex(period, this.dataInput, new NullLogger());
            // матрица пересечений                0      1      2      3      4      5     6       
            //macds.CrossTo[2] = new List<bool> { false, false, false, true,  true,  true };
            //macds.CrossTo[3] = new List<bool> { false, false, false, false, true,  true };
            //macds.CrossTo[4] = new List<bool> { false, false, false, false, false, true, true };

            // матрица пересечений через список кортежей
            maxs.CrossTo.Add(new Tuple<int, int>(2, 3));
            maxs.CrossTo.Add(new Tuple<int, int>(2, 4));
            maxs.CrossTo.Add(new Tuple<int, int>(2, 5));

            maxs.CrossTo.Add(new Tuple<int, int>(3, 4));
            maxs.CrossTo.Add(new Tuple<int, int>(3, 5));

            maxs.CrossTo.Add(new Tuple<int, int>(4, 5));
            maxs.CrossTo.Add(new Tuple<int, int>(4, 6));
        }

        [TestMethod]
        public void IndicatorMaDe_init_test()
        {
            ///проверяем Input
            Assert.IsNotNull(handler);
            Assert.IsNotNull(handler.Input);
            
            ///List<double>(10) { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90 }
            Assert.AreEqual(10, handler.Input.Value.Count);

            ///проверяем Output
            Assert.IsNotNull(handler[0]);
            Assert.IsNotNull(handler[1]);
        }

        [TestMethod]
        public void IndicatorMaDe_do_test()
        {
            var ema = Indicator.EMA(source, 3);
            handler.Do(0);
            Assert.AreEqual(1, handler[0].Count);
            Assert.AreEqual(handler[0], handler.Ma);
            Assert.AreEqual(handler[1], handler.De);

            //Assert.AreEqual(ema.Last(), handler.Source.Last());
            //for (int i = 0; i < ema.Count; i++)
            //{
            //    Assert.AreEqual(ema[i], handler.Source[i]);
            //}
        }

        [TestMethod]
        public void IndicatorMaDex_init_test()
        {
            Assert.AreEqual(series, maxs.MaDe.Count);
            //проверяем входы
            Assert.AreEqual(this.dataInput, maxs.MaDe[0].Input);
            for (int i = 1; i < series; i++)
            {
                Assert.AreEqual(maxs.MaDe[i - 1], maxs.MaDe[i].Input.DataOutput);
                Assert.AreEqual(period[i], maxs.MaDe[i].Period);
            }
            
            // матрица пересечений через список кортежей
            //maxs.CrossTo.Add(new Tuple<int, int>(2, 3));
            //...
            //maxs.CrossTo.Add(new Tuple<int, int>(4, 6));

            //проверяем список для пересечений
            Assert.AreEqual(maxs.CrossTo.First().Item1, 2);
            Assert.AreEqual(maxs.CrossTo.First().Item2, 3);
            Assert.AreEqual(maxs.CrossTo.Last().Item1, 4);
            Assert.AreEqual(maxs.CrossTo.Last().Item2, 6);
        }
        [TestMethod]
        public void IndicatorMaDex_do_test()
        {
            Assert.AreEqual(maxs.CrossUp[2, 3].Count, 0);
            Assert.AreEqual(maxs.CrossDn[2, 3].Count, 0);

            //проверяем работу
            long id = 10;
            maxs.Do(id);
            foreach (var mx in maxs.MaDe)
            {
                Assert.AreEqual(mx.Ma.Last(), 90);                
            }
            // пересечения посчитаны
            Assert.AreEqual(maxs.CrossUp[2, 3].Count, 1);
            Assert.AreEqual(maxs.CrossDn[2, 3].Count, 1);
            // пересечения посчитаны, но как точно надо проверять другим тестом
        }


        //[TestInitialize]
        //public void Setup()
        //{
        //    this.tradingData = new TradingDataContext();
        //    this.signalQueue = new ObservableQueue<Signal>();

        //    this.strategyHeader = new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS", 10);
        //    this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

        //    this.barSettings = new BarSettings(this.strategyHeader, this.strategyHeader.Symbol, 60, 3);
        //    this.tradingData.Get<ICollection<BarSettings>>().Add(this.barSettings);

        //    handler =
        //        new IndicatorOnBarMaDeviation(this.strategyHeader,
        //            this.tradingData,
        //            this.Period,
        //            new NullLogger());

        //    Assert.AreEqual(0, this.signalQueue.Count);
        //    Assert.IsNotNull(handler);
        //}

        //[TestMethod]
        //public void IndicatorOnBarMaDeviation_value_count_test()
        //{
        //    AddBreakToHighBars("RTS", this.tradingData.Get<ObservableCollection<Bar>>());

        //    Assert.AreEqual(3, this.handler.Ma.Count);
        //    Assert.AreEqual(3, this.handler.De.Count);
        //    Assert.AreEqual(3, this.handler.ValueMa.Count);
        //    Assert.AreEqual(3, this.handler.ValueDe.Count);
        //    Assert.AreEqual(15, Math.Round(this.handler.Ma[2]));
        //    Assert.AreEqual(1, Math.Round(this.handler.De[2]));
        //}

        //private void AddBreakToHighBars(string symbol, ObservableCollection<Bar> collection)
        //{
        //    collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
        //    collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
        //    collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 2, 0), 12, 19, 11, 16, 100));
        //}

        //private void AddBreakToLowBars(string symbol, ObservableCollection<Bar> collection)
        //{
        //    collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
        //    collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
        //    collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 2, 0), 12, 13, 8, 11, 100));
        //}

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

        //private void AddNeutralBars(string symbol, ObservableCollection<Bar> collection)
        //{
        //    collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
        //    collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
        //    collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 2, 0), 12, 16, 11, 14, 100));
        //}

        //private void AddInsufficientBars(string symbol, ObservableCollection<Bar> collection)
        //{
        //    collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
        //    collection.Add(new Bar(symbol, this.barSettings.Interval, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
        //}
    }
}
