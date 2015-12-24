using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Test.Data;
using TRL.Common.Handlers;
using TRL.Common.TimeHelpers;
//using TRL.Common.Test;
using TRL.Configuration;
using TRL.Common.Collections;
using TRL.Logging;
using TRL.Common.Test;

namespace TRL.Common.Handlers.Test
{
    [TestClass]
    public class UpdateBarsOnTickTests
    {

        private IDataContext tradingData;

        [TestInitialize]
        public void Handlers_Setup()
        {
            this.tradingData = new TradingDataContext();
        }

        [TestMethod]
        public void Handlers_make_bar_for_one_strategy_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(2, "Strategy 2", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ObservableHashSet<StrategyHeader>>().Add(strategyHeader);

            BarSettings barSettings = new BarSettings(strategyHeader, strategyHeader.Symbol, 60, 0);
            this.tradingData.Get<ObservableHashSet<BarSettings>>().Add(barSettings);

            DateTime start = new DateTime(2013, 7, 10, 10, 0, 0, 0);

            FakeTimeTracker ftt = new FakeTimeTracker(start, start);

            UpdateBarsOnTick handler = new UpdateBarsOnTick(barSettings, ftt, this.tradingData, new NullLogger());

            Assert.AreEqual(0, this.tradingData.Get<ObservableCollection<Bar>>().Count);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(barSettings.Symbol, start.AddMilliseconds(-500), TradeAction.Buy, 150000, 25));

            for (int i = 0; i < barSettings.Interval * 2; i++)
                this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(barSettings.Symbol, start.AddMilliseconds(i * 500), TradeAction.Buy, 150000, 25));

            ftt.IncrementStopDate(barSettings.Interval);

            Assert.AreEqual(121, this.tradingData.Get<ObservableCollection<Tick>>().Count);
            Assert.AreEqual(new DateTime(2013, 7, 10, 10, 0, 59, 500), this.tradingData.Get<ObservableCollection<Tick>>().Last().DateTime);
            Assert.AreEqual(0, this.tradingData.Get<ObservableCollection<Bar>>().Count);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(barSettings.Symbol, new DateTime(2013, 7, 10, 10, 0, 59, 600), TradeAction.Buy, 151000, 25));
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(barSettings.Symbol, new DateTime(2013, 7, 10, 10, 0, 59, 700), TradeAction.Buy, 149000, 25));
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(barSettings.Symbol, new DateTime(2013, 7, 10, 10, 0, 59, 800), TradeAction.Buy, 150000, 25));
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(barSettings.Symbol, new DateTime(2013, 7, 10, 10, 0, 59, 900), TradeAction.Buy, 149500, 25));
            Assert.AreEqual(0, this.tradingData.Get<ObservableCollection<Bar>>().Count);

            // Обработчик генерирует новый бар, только когда время пришедшего тика располагается в диапазоне следующего бара.
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(barSettings.Symbol, new DateTime(2013, 7, 10, 10, 1, 0, 0), TradeAction.Buy, 150000, 25));
            Assert.AreEqual(1, this.tradingData.Get<ObservableCollection<Bar>>().Count);

            Bar bar = this.tradingData.Get<ObservableCollection<Bar>>().Last();
            Assert.AreEqual(start.AddSeconds(60), bar.DateTime);
            Assert.AreEqual(barSettings.Symbol, bar.Symbol);
            Assert.AreEqual(barSettings.Interval, bar.Interval);
            Assert.AreEqual(150000, bar.Open);
            Assert.AreEqual(151000, bar.High);
            Assert.AreEqual(149000, bar.Low);
            Assert.AreEqual(149500, bar.Close);
            Assert.AreEqual(3100, bar.Volume);
        }

        [TestMethod]
        public void Handlers_Make_First_Bar_For_Hour()
        {
            StrategyHeader s = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-12.12_FT", 10);
            BarSettings barSettings = new BarSettings(s, "RTS-12.12_FT", 3600, 19);

            DateTime start = new DateTime(2013, 5, 15, 10, 0, 0);

            FakeTimeTracker tt = new FakeTimeTracker(start, start);

            UpdateBarsOnTick updateBars = new UpdateBarsOnTick(barSettings, tt, this.tradingData, new NullLogger());

            Assert.AreEqual(0, this.tradingData.Get<ObservableCollection<Bar>>().Count);


            for (int i = 0; i < barSettings.Interval; i++)
            {
                this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(i), Symbol = "RTS-12.12_FT", Price = 150000 + i, TradeAction = TradeAction.Buy, Volume = 100 });

                if (i == 1000)
                {
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(1000), Symbol = "RTS-12.12_FT", Price = 149000, TradeAction = TradeAction.Buy, Volume = 100 });
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(2000), Symbol = "RTS-12.12_FT", Price = 154500, TradeAction = TradeAction.Buy, Volume = 100 });
                }

                tt.IncrementStopDate(1);
            }

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(-1), Symbol = "RTS-12.12_FT", Price = 148000, TradeAction = TradeAction.Buy, Volume = 100 });
            Assert.AreEqual(0, this.tradingData.Get<ObservableCollection<Bar>>().Count);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(3600), Symbol = "RTS-12.12_FT", Price = 155500, TradeAction = TradeAction.Buy, Volume = 100 });
            Assert.AreEqual(1, this.tradingData.Get<ObservableCollection<Bar>>().Count);


            Bar bar = this.tradingData.Get<ObservableCollection<Bar>>().First();

            Assert.AreEqual("RTS-12.12_FT", bar.Symbol);
            Assert.AreEqual(start.AddHours(1), bar.DateTime);
            Assert.AreEqual(149000, bar.Low);
            Assert.AreEqual(154500, bar.High);
            Assert.AreEqual(150000, bar.Open);
            Assert.AreEqual(153599, bar.Close);
            Assert.AreEqual(360200, bar.Volume);
        }

        [TestMethod]
        public void Handlers_Do_Nothing_If_Bar_With_Same_Date_Exists()
        {
            StrategyHeader s = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-12.12_FT", 10);
            BarSettings barSettings = new BarSettings(s, "RTS-12.12_FT", 3600, 19);

            DateTime start = new DateTime(2013, 5, 15, 10, 0, 0);

            FakeTimeTracker tt = new FakeTimeTracker(start, start);

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar { Symbol = barSettings.Symbol, DateTime = start.AddHours(1), Open = 100, High = 100, Low = 100, Close = 100, Volume = 100 });

            MakeBarsOnTick updateBars = new MakeBarsOnTick(barSettings, tt, this.tradingData, new NullLogger());

            Assert.AreEqual(1, this.tradingData.Get<ObservableCollection<Bar>>().Count);


            for (int i = 0; i < barSettings.Interval; i++)
            {
                this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(i), Symbol = "RTS-12.12_FT", Price = 150000 + i, TradeAction = TradeAction.Buy, Volume = 100 });

                if (i == 1000)
                {
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(1000), Symbol = "RTS-12.12_FT", Price = 149000, TradeAction = TradeAction.Buy, Volume = 100 });
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(2000), Symbol = "RTS-12.12_FT", Price = 154500, TradeAction = TradeAction.Buy, Volume = 100 });
                }

                tt.IncrementStopDate(1);
            }

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(-1), Symbol = "RTS-12.12_FT", Price = 148000, TradeAction = TradeAction.Buy, Volume = 100 });
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(3600), Symbol = "RTS-12.12_FT", Price = 155500, TradeAction = TradeAction.Buy, Volume = 100 });

            IEnumerable<Tick> ticks = this.tradingData.Get<ObservableCollection<Tick>>().Where(t => t.DateTime >= start && t.DateTime < start.AddSeconds(3600));
            Assert.AreEqual(3602, ticks.Count());

            Assert.AreEqual(1, this.tradingData.Get<ObservableCollection<Bar>>().Count);

            Bar bar = this.tradingData.Get<ObservableCollection<Bar>>().First();

            Assert.AreEqual("RTS-12.12_FT", bar.Symbol);
            Assert.AreEqual(start.AddHours(1), bar.DateTime);
            Assert.AreEqual(100, bar.Low);
            Assert.AreEqual(100, bar.High);
            Assert.AreEqual(100, bar.Open);
            Assert.AreEqual(100, bar.Close);
            Assert.AreEqual(100, bar.Volume);
        }

        [TestMethod]
        public void Handlers_Make_First_Morning_Bar_For_Hour()
        {
            StrategyHeader s = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-12.12_FT", 10);
            BarSettings barSettings = new BarSettings(s, "RTS-12.12_FT", 3600, 19);

            DateTime start = new DateTime(2013, 5, 14, 23, 50, 0);

            DateTime end = new DateTime(2013, 5, 16, 10, 0, 0);

            FakeTimeTracker tt = new FakeTimeTracker(start, end);

            MakeBarsOnTick updateBars = new MakeBarsOnTick(barSettings, tt, this.tradingData, new NullLogger());

            Assert.AreEqual(0, this.tradingData.Get<ObservableCollection<Bar>>().Count);


            for (int i = 0; i < barSettings.Interval; i++)
            {
                this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(i), Symbol = "RTS-12.12_FT", Price = 150000 + i, TradeAction = TradeAction.Buy, Volume = 100 });

                if (i == 1000)
                {
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(1000), Symbol = "RTS-12.12_FT", Price = 149000, TradeAction = TradeAction.Buy, Volume = 100 });
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(2000), Symbol = "RTS-12.12_FT", Price = 154500, TradeAction = TradeAction.Buy, Volume = 100 });
                }

                tt.IncrementStopDate(1);
            }

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(-1), Symbol = "RTS-12.12_FT", Price = 148000, TradeAction = TradeAction.Buy, Volume = 100 });
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(3600), Symbol = "RTS-12.12_FT", Price = 155500, TradeAction = TradeAction.Buy, Volume = 100 });

            Assert.AreEqual(1, this.tradingData.Get<ObservableCollection<Bar>>().Count);

            Bar bar = this.tradingData.Get<ObservableCollection<Bar>>().Last();

            Assert.AreEqual("RTS-12.12_FT", bar.Symbol);
            Assert.AreEqual(end.AddHours(1), bar.DateTime);
            Assert.AreEqual(149000, bar.Low);
            Assert.AreEqual(154500, bar.High);
            Assert.AreEqual(150000, bar.Open);
            Assert.AreEqual(153599, bar.Close);
            Assert.AreEqual(360200, bar.Volume);
        }

        [TestMethod]
        public void Handlers_Make_Next_Bar_Instead_Of_Existing_Oldest()
        {
            StrategyHeader s = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-12.12_FT", 10);
            BarSettings barSettings = new BarSettings(s, "RTS-12.12_FT", 3600, 19);

            DateTime start = new DateTime(2013, 5, 15, 0, 0, 0);

            DateTime end = new DateTime(2013, 5, 16, 11, 0, 0);

            FakeTimeTracker tt = new FakeTimeTracker(start, end);

            MakeBarsOnTick updateBars = new MakeBarsOnTick(barSettings, tt, this.tradingData, new NullLogger());

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar { Symbol = "RTS-12.12_FT", DateTime = start, Open = 150000, High = 160000, Low = 140000, Close = 145000, Volume = 100 });
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar { Symbol = "RTS-12.12_FT", DateTime = end, Open = 150000, High = 160000, Low = 140000, Close = 145000, Volume = 100 });

            Assert.AreEqual(2, this.tradingData.Get<ObservableCollection<Bar>>().Count);


            for (int i = 0; i < barSettings.Interval; i++)
            {
                this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(i), Symbol = "RTS-12.12_FT", Price = 150000 + i, TradeAction = TradeAction.Buy, Volume = 100 });

                if (i == 1000)
                {
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(1000), Symbol = "RTS-12.12_FT", Price = 149000, TradeAction = TradeAction.Buy, Volume = 100 });
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(2000), Symbol = "RTS-12.12_FT", Price = 154500, TradeAction = TradeAction.Buy, Volume = 100 });
                }

                tt.IncrementStopDate(1);
            }

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(-1), Symbol = "RTS-12.12_FT", Price = 148000, TradeAction = TradeAction.Buy, Volume = 100 });
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(3600), Symbol = "RTS-12.12_FT", Price = 155500, TradeAction = TradeAction.Buy, Volume = 100 });

            Assert.AreEqual(3, this.tradingData.Get<ObservableCollection<Bar>>().Count);

            Bar first = this.tradingData.Get<ObservableCollection<Bar>>().ElementAt(1);
            Assert.AreEqual(end, first.DateTime);

            Bar bar = this.tradingData.Get<ObservableCollection<Bar>>().Last();

            Assert.AreEqual("RTS-12.12_FT", bar.Symbol);
            Assert.AreEqual(end.AddHours(1), bar.DateTime);
            Assert.AreEqual(149000, bar.Low);
            Assert.AreEqual(154500, bar.High);
            Assert.AreEqual(150000, bar.Open);
            Assert.AreEqual(153599, bar.Close);
            Assert.AreEqual(360200, bar.Volume);
        }

        [TestMethod]
        public void Handlers_Make_Fifteen_Minutes_Bars()
        {
            DateTime start = new DateTime(2013, 5, 15, 0, 0, 0);

            DateTime end = new DateTime(2013, 5, 16, 11, 0, 0);

            FakeTimeTracker tt = new FakeTimeTracker(start, end);
            StrategyHeader s = new StrategyHeader(1, "Strategy 2", "BP12345-RF-01", "RTS-12.12_FT", 10);
            BarSettings settings = new BarSettings(s, "RTS-12.12_FT", 900, 10);

            MakeBarsOnTick updateBars = new MakeBarsOnTick(settings, tt, this.tradingData, new NullLogger());

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar { Symbol = "RTS-12.12_FT", DateTime = start, Open = 150000, High = 160000, Low = 140000, Close = 145000, Volume = 100 });
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar { Symbol = "RTS-12.12_FT", DateTime = end, Open = 150000, High = 160000, Low = 140000, Close = 145000, Volume = 100 });

            Assert.AreEqual(2, this.tradingData.Get<ObservableCollection<Bar>>().Count);


            for (int i = 0; i < settings.Interval * 4; i++)
            {
                this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(i), Symbol = "RTS-12.12_FT", Price = 150000 + i, TradeAction = TradeAction.Buy, Volume = 100 });

                if (i == 1000)
                {
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(1000), Symbol = "RTS-12.12_FT", Price = 149000, TradeAction = TradeAction.Buy, Volume = 100 });
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(2000), Symbol = "RTS-12.12_FT", Price = 154500, TradeAction = TradeAction.Buy, Volume = 100 });
                }

                tt.IncrementStopDate(1);
            }

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(-1), Symbol = "RTS-12.12_FT", Price = 148000, TradeAction = TradeAction.Buy, Volume = 100 });
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = end.AddSeconds(3600), Symbol = "RTS-12.12_FT", Price = 155500, TradeAction = TradeAction.Buy, Volume = 100 });

            Assert.AreEqual(6, this.tradingData.Get<ObservableCollection<Bar>>().Count);

            Bar bar = this.tradingData.Get<ObservableCollection<Bar>>().Last();

            Assert.AreEqual("RTS-12.12_FT", this.tradingData.Get<ObservableCollection<Bar>>().ElementAt(5).Symbol);
            Assert.AreEqual(end.AddHours(1), this.tradingData.Get<ObservableCollection<Bar>>().ElementAt(5).DateTime);
            Assert.AreEqual(152700, this.tradingData.Get<ObservableCollection<Bar>>().ElementAt(5).Low);
            Assert.AreEqual(153599, this.tradingData.Get<ObservableCollection<Bar>>().ElementAt(5).High);
            Assert.AreEqual(152700, this.tradingData.Get<ObservableCollection<Bar>>().ElementAt(5).Open);
            Assert.AreEqual(153599, this.tradingData.Get<ObservableCollection<Bar>>().ElementAt(5).Close);
            Assert.AreEqual(90000, this.tradingData.Get<ObservableCollection<Bar>>().ElementAt(5).Volume);
        }

        [TestMethod]
        public void Handlers_Do_Nothing_When_No_Ticks_With_TradeSettings_Symbol()
        {
            StrategyHeader s = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-12.12_FT", 10);
            BarSettings barSettings = new BarSettings(s, "RTS-12.12_FT", 3600, 19);

            DateTime start = new DateTime(2013, 5, 15, 10, 0, 0);

            FakeTimeTracker tt = new FakeTimeTracker(start, start);

            MakeBarsOnTick updateBars = new MakeBarsOnTick(barSettings, tt, this.tradingData, new NullLogger());

            Assert.AreEqual(0, this.tradingData.Get<ObservableCollection<Bar>>().Count);


            for (int i = 0; i < barSettings.Interval; i++)
            {
                this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(i), Symbol = "RTS-6.13_FT", Price = 150000 + i, TradeAction = TradeAction.Buy, Volume = 100 });

                if (i == 1000)
                {
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(1000), Symbol = "RTS-6.13_FT", Price = 149000, TradeAction = TradeAction.Buy, Volume = 100 });
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(2000), Symbol = "RTS-6.13_FT", Price = 154500, TradeAction = TradeAction.Buy, Volume = 100 });
                }

                tt.IncrementStopDate(1);
            }

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(-1), Symbol = "RTS-6.13_FT", Price = 148000, TradeAction = TradeAction.Buy, Volume = 100 });
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(3600), Symbol = "RTS-6.13_FT", Price = 155500, TradeAction = TradeAction.Buy, Volume = 100 });

            Assert.AreEqual(0, this.tradingData.Get<ObservableCollection<Bar>>().Count);

        }

        [TestMethod]
        public void Handlers_Do_Nothing_When_MarketData_Already_Contains_Bar_For_This_Date()
        {
            StrategyHeader s = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-12.12_FT", 10);
            BarSettings barSettings = new BarSettings(s, "RTS-12.12_FT", 3600, 19);

            DateTime start = new DateTime(2013, 5, 15, 10, 0, 0);
            DateTime stop = start.AddSeconds(3600);

            FakeTimeTracker tt = new FakeTimeTracker(start, start);

            MakeBarsOnTick updateBars = new MakeBarsOnTick(barSettings, tt, this.tradingData, new NullLogger());

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar { Symbol = "RTS-12.12_FT", DateTime = stop, Open = 150000, High = 160000, Low = 140000, Close = 145000, Volume = 100 });

            Assert.AreEqual(1, this.tradingData.Get<ObservableCollection<Bar>>().Count);


            for (int i = 0; i < barSettings.Interval; i++)
            {
                this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(i), Symbol = "RTS-12.12_FT", Price = 150000 + i, TradeAction = TradeAction.Buy, Volume = 100 });

                if (i == 1000)
                {
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(1000), Symbol = "RTS-12.12_FT", Price = 149000, TradeAction = TradeAction.Buy, Volume = 100 });
                    this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(2000), Symbol = "RTS-12.12_FT", Price = 154500, TradeAction = TradeAction.Buy, Volume = 100 });
                }

                tt.IncrementStopDate(1);
            }

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(-1), Symbol = "RTS-12.12_FT", Price = 148000, TradeAction = TradeAction.Buy, Volume = 100 });
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = start.AddSeconds(3600), Symbol = "RTS-12.12_FT", Price = 155500, TradeAction = TradeAction.Buy, Volume = 100 });

            Assert.AreEqual(1, this.tradingData.Get<ObservableCollection<Bar>>().Count);

            Bar bar = this.tradingData.Get<ObservableCollection<Bar>>().First();

            Assert.AreEqual("RTS-12.12_FT", bar.Symbol);
            Assert.AreEqual(start.AddHours(1), bar.DateTime);
            Assert.AreEqual(140000, bar.Low);
            Assert.AreEqual(160000, bar.High);
            Assert.AreEqual(150000, bar.Open);
            Assert.AreEqual(145000, bar.Close);
            Assert.AreEqual(100, bar.Volume);

        }

    }
}
