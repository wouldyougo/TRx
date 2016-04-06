using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Common.Test;

namespace TRL.Common.Models.Test
{
    [TestClass]
    public class BarBuilderTests
    {
        private int count = 0;
        private ObservableCollection<Tick> ticks;
        //private ObservableCollection<Bar> bars;

        private BarBuilderRangeBar barBuilder;
        private BarSettings barSettings;
        private Common.Enums.DataModelType barType;

        [TestInitialize]
        public void Setup()
        {
            this.ticks = new ObservableCollection<Tick>();

            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 20, 20), Price = 500, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 20, 25), Price = 530, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 20, 28), Price = 510, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 20, 33), Price = 550, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 20, 38), Price = 490, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 21, 10), Price = 510, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 21, 20), Price = 500, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 21, 23), Price = 520, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 21, 45), Price = 520, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 21, 58), Price = 520, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 22, 20), Price = 520, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 22, 45), Price = 500, Volume = 100, TradeAction = TradeAction.Buy });

            barBuilder = CreateBarBuilderRangeBar(out barSettings, out barType);
        }

        public void TicksChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.count++;
        }

        /// <summary>
        /// Создаем екземпляр построителя баров
        /// </summary>
        /// <param name="barSettings"></param>
        /// <param name="barType"></param>
        /// <returns></returns>
        private static BarBuilderRangeBar CreateBarBuilderRangeBar(out BarSettings barSettings, out Common.Enums.DataModelType barType)
        {
            string symbol = "SBER";
            StrategyHeader strategyHeader = new StrategyHeader(1, "Break out strategyHeader", "BP12345-RF-01", symbol, 10);
            int interval = 50;
            barType = Common.Enums.DataModelType.RangeBar;

            barSettings = new BarSettings(strategyHeader, symbol, interval, 0, barType);

            return new BarBuilderRangeBar(barSettings);
        }

        [TestMethod]
        public void BarBuilder_СоздатьЭкземплар()
        {
            BarBuilderRangeBar barBuilder = null;
            Assert.IsNull(barBuilder);
            BarSettings barSettings;

            Common.Enums.DataModelType barType;
            barBuilder = CreateBarBuilderRangeBar(out barSettings, out barType);

            Assert.IsNotNull(barBuilder);
            Assert.AreEqual(barBuilder.BarSettings.BarType, barType);
        }


        [TestMethod]
        public void BarBuilder_СоздатьНовый_Bar()
        {
            var tick = ticks[0];
            Bar bar = barBuilder.CreateBar(tick, tick.DateTime);
            Assert.IsNotNull(bar);
            Assert.AreEqual(barSettings.Symbol, bar.Symbol);
            Assert.AreEqual(barSettings.Interval, bar.Interval);

            Assert.AreEqual(bar.Open, tick.Price);
            Assert.AreEqual(bar.High, tick.Price);
            Assert.AreEqual(bar.Low, tick.Price);
            Assert.AreEqual(bar.Close, tick.Price);
            Assert.AreEqual(bar.Volume, tick.Volume);
            Assert.AreEqual(bar.DateTime, tick.DateTime);
        }

        [TestMethod]
        public void BarBuilder_Обновить_Bar()
        {
            var tick1 = ticks[0];
            Bar bar = barBuilder.CreateBar(tick1, tick1.DateTime);

            Assert.AreEqual(bar.DateTime, tick1.DateTime);
            Assert.AreEqual(bar.Open, tick1.Price);
            Assert.AreEqual(bar.High, tick1.Price);
            Assert.AreEqual(bar.Low, tick1.Price);
            Assert.AreEqual(bar.Close, tick1.Price);
            Assert.AreEqual(bar.Volume, tick1.Volume);
            Assert.AreEqual(bar.VolumePrice, tick1.Volume * tick1.Price);
            Assert.AreEqual(barBuilder.CheckRangeReach(bar), false);

            Tick tick2 = ticks[1];
            bar = barBuilder.UpdateBar(bar, tick2);

            Assert.AreEqual(bar.DateTime, tick2.DateTime);
            Assert.AreEqual(bar.Open, tick1.Price);
            Assert.AreEqual(bar.High, tick2.Price);
            Assert.AreEqual(bar.Low, tick1.Price);
            Assert.AreEqual(bar.Close, tick2.Price);
            Assert.AreEqual(bar.Volume, tick1.Volume + tick2.Volume);
            Assert.AreEqual(bar.VolumePrice, tick1.Volume * tick1.Price + tick2.Volume * tick2.Price);
            Assert.AreEqual(barBuilder.CheckRangeReach(bar), false);

            Tick tick3 = ticks[2];
            bar = barBuilder.UpdateBar(bar, tick3);

            Assert.AreEqual(bar.DateTime, tick3.DateTime);
            Assert.AreEqual(bar.Open, tick1.Price);
            Assert.AreEqual(bar.High, tick2.Price);
            Assert.AreEqual(bar.Low, tick1.Price);
            Assert.AreEqual(bar.Close, tick3.Price);
            Assert.AreEqual(bar.Volume, tick1.Volume + tick2.Volume + tick3.Volume);
            Assert.AreEqual(bar.VolumePrice, tick1.Volume * tick1.Price + 
                                             tick2.Volume * tick2.Price + 
                                             tick3.Volume * tick3.Price);
            Assert.AreEqual(barBuilder.CheckRangeReach(bar), false);

            Tick tick4 = ticks[3];
            bar = barBuilder.UpdateBar(bar, tick4);

            Assert.AreEqual(bar.DateTime, tick4.DateTime);
            Assert.AreEqual(bar.Open, tick1.Price);
            Assert.AreEqual(bar.High, tick4.Price);
            Assert.AreEqual(bar.Low, tick1.Price);
            Assert.AreEqual(bar.Close, tick4.Price);
            Assert.AreEqual(bar.Volume, tick1.Volume + tick2.Volume + tick3.Volume + tick4.Volume);
            Assert.AreEqual(bar.VolumePrice, tick1.Volume * tick1.Price + 
                                             tick2.Volume * tick2.Price +
                                             tick3.Volume * tick3.Price + 
                                             tick4.Volume * tick4.Price );
            Assert.AreEqual(barBuilder.CheckRangeReach(bar), true);
        }

        //[TestMethod]
        //public void MakeFirstBar()
        //{
        //    long interval = TimeSpan.TicksPerMinute;
        //    long from = new DateTime(2012, 10, 23, 20, 20, 0).Ticks;
        //    long to = from + interval;

        //    IEnumerable<Tick> ticks = BarsFactory.GetTicksInterval(this.ticks, from, to);

        //    Bar bar = BarsFactory.CreateBar(ticks, new DateTime(2012, 10, 23, 20, 20, 38));

        //    Assert.AreEqual(155, bar.High);
        //    Assert.AreEqual(149, bar.Low);
        //    Assert.AreEqual(150.43, bar.Open);
        //    Assert.AreEqual(149.55, bar.Close);
        //    Assert.AreEqual(500, bar.Volume);
        //    Assert.AreEqual(new DateTime(2012, 10, 23, 20, 20, 38), bar.DateTime);
        //}

        //[TestMethod]
        //public void MakeSecondBar()
        //{
        //    long interval = TimeSpan.TicksPerMinute;
        //    long from = new DateTime(2012, 10, 23, 20, 21, 0).Ticks;
        //    long to = from + interval;

        //    IEnumerable<Tick> ticks = BarsFactory.GetTicksInterval(this.ticks, from, to);

        //    Bar bar = BarsFactory.CreateBar(ticks, new DateTime(2012, 10, 23, 20, 21, 58));

        //    Assert.AreEqual(152.4, bar.High);
        //    Assert.AreEqual(150.15, bar.Low);
        //    Assert.AreEqual(151, bar.Open);
        //    Assert.AreEqual(152.40, bar.Close);
        //    Assert.AreEqual(500, bar.Volume);
        //    Assert.AreEqual(new DateTime(2012, 10, 23, 20, 21, 58), bar.DateTime);
        //}

        //[TestMethod]
        //public void MakeThirdBar()
        //{
        //    long interval = TimeSpan.TicksPerMinute;
        //    long from = new DateTime(2012, 10, 23, 20, 22, 0).Ticks;
        //    long to = from + interval;

        //    IEnumerable<Tick> ticks = BarsFactory.GetTicksInterval(this.ticks, from, to);

        //    Bar bar = BarsFactory.CreateBar(ticks, new DateTime(2012, 10, 23, 20, 22, 45));

        //    Assert.AreEqual(152.49, bar.High);
        //    Assert.AreEqual(150, bar.Low);
        //    Assert.AreEqual(152.49, bar.Open);
        //    Assert.AreEqual(150, bar.Close);
        //    Assert.AreEqual(200, bar.Volume);
        //    Assert.AreEqual(new DateTime(2012, 10, 23, 20, 22, 45), bar.DateTime);
        //}


        //[TestMethod]
        //public void MakeBarFromTicks()
        //{
        //    Bar bar = BarsFactory.CreateBar(MakeTicks(), new DateTime(2012, 12, 01, 10, 00, 15));

        //    Assert.AreEqual(142100, bar.High);
        //    Assert.AreEqual(138808, bar.Low);
        //    Assert.AreEqual(140000, bar.Open);
        //    Assert.AreEqual(141000, bar.Close);
        //    Assert.AreEqual(17000, bar.Volume);
        //    Assert.AreEqual(new DateTime(2012, 12, 01, 10, 00, 15), bar.DateTime);

        //}

        private IEnumerable<Tick> MakeTicks()
        {
            List<Tick> result = new List<Tick>();

            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 00), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 01), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 02), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 03), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 04), Price = 142100, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 05), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 06), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 07), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 08), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 09), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 10), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 11), Price = 138808, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 12), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 13), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 14), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 15), Price = 140000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });
            result.Add(new Tick { Symbol = "RTS-12.12_FT", DateTime = new DateTime(2012, 12, 01, 10, 00, 15), Price = 141000, Volume = 1000, TradeAction = Common.Models.TradeAction.Buy });

            return result;
        }

        //[TestInitialize]
        //public void Setup()
        //{
        //    string symbol = "RTS-6.15_FT";
        //    int interval = 100000;
        //    Common.Enums.DataModelType dataModelType = Common.Enums.DataModelType.RangeBar;
        //    barType = new TradeBarType(symbol, interval, dataModelType);

        //    barBuilder = new BarBuilder(barType);

        //    tick = this.Tick();
        //}
        //public Tick Tick(decimal value = 150000, decimal volume = 3)
        //{
        //    DateTime tickDate = DateTime.Now;
        //    Tick tick = new Tick("RTS-6.15_FT", tickDate, TradeAction.Buy, value, volume);
        //    return tick;
        //}

        [TestMethod]
        public void BarBuilderTimeBar_СоздатьЭкземплар()
        {
            BarBuilderTimeBar barBuilder = null;
            Assert.IsNull(barBuilder);
            BarSettings barSettings;

            Common.Enums.DataModelType barType;
            barBuilder = CreateBarBuilderTimeBar(out barSettings, out barType);

            Assert.IsNotNull(barBuilder);
            Assert.AreEqual(barBuilder.BarSettings.BarType, barType);
            Assert.IsNotNull(barBuilder.BarList);
            Assert.AreEqual(barBuilder.TimeSpanInterval.TotalSeconds, barBuilder.BarSettings.Interval);
            Assert.AreEqual(barBuilder.DateTimeStart.Date, DateTime.Now.Date);
            Assert.AreEqual(barBuilder.DateTimeStart.TimeOfDay, barBuilder.BarSettings.DateTimeStart.TimeOfDay);
            Assert.AreNotEqual(barBuilder.BarList.Count, 0);
            ///15 чаосов * 60 мин = 900 мин
            Assert.AreEqual(barBuilder.BarList.Count, 900);
            Assert.AreEqual(barBuilder.BarList.First().DateTimeOpen, (DateTime.Now.Date).AddHours(9));
            Assert.AreEqual(barBuilder.BarList.First().DateTime,     (DateTime.Now.Date).AddHours(9) + barBuilder.TimeSpanInterval);
            Assert.AreEqual(barBuilder.BarList.Last().DateTimeOpen, (DateTime.Now.Date).AddHours(24) - barBuilder.TimeSpanInterval); 
            Assert.AreEqual(barBuilder.BarList.Last().DateTime, (DateTime.Now.Date).AddHours(24));

            Assert.AreEqual(barBuilder.BarList.Last().Interval, barBuilder.BarSettings.Interval);
            Assert.AreEqual(barBuilder.BarList.Last().Symbol, barBuilder.BarSettings.Symbol);
        }
        /// <summary>
        /// Создаем екземпляр построителя баров
        /// </summary>
        /// <param name="barSettings"></param>
        /// <param name="barType"></param>
        /// <returns></returns>
        private static BarBuilderTimeBar CreateBarBuilderTimeBar(out BarSettings barSettings, out Common.Enums.DataModelType barType)
        {
            string symbol = "SBER";
            StrategyHeader strategyHeader = new StrategyHeader(1, "Break out strategyHeader", "BP12345-RF-01", symbol, 10);
            int interval = 60;
            barType = Common.Enums.DataModelType.TimeBar;

            barSettings = new BarSettings(strategyHeader, symbol, interval, 0, barType);
            barSettings.DateTimeStart = (new DateTime()).AddHours(9);

            return new BarBuilderTimeBar(barSettings);
        }
    }
}
