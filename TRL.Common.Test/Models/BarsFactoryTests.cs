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
    public class BarsFactoryTests
    {
        private int count = 0;
        private ObservableCollection<Tick> ticks;

        [TestInitialize]
        public void Setup()
        {
            this.ticks = new ObservableCollection<Tick>();

            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 20, 20), Price = 150.43, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 20, 25), Price = 151.12, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 20, 28), Price = 155, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 20, 33), Price = 149, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 20, 38), Price = 149.55, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 21, 10), Price = 151, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 21, 20), Price = 150.15, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 21, 23), Price = 152, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 21, 45), Price = 152.38, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 21, 58), Price = 152.40, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 22, 20), Price = 152.49, Volume = 100, TradeAction = TradeAction.Buy });
            this.ticks.Add(new Tick{ Symbol = "SBER", DateTime = new DateTime(2012, 10, 23, 20, 22, 45), Price = 150, Volume = 100, TradeAction = TradeAction.Buy });
        }

        public void TicksChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.count++;
        }

        [TestMethod]
        public void OrderTicksByPrice()
        {
            IEnumerable<Tick> orderedTicks = BarsFactory.OrderByPrice(this.ticks);

            Assert.AreEqual(149, orderedTicks.ElementAt(0).Price);
            Assert.AreEqual(149.55, orderedTicks.ElementAt(1).Price);
            Assert.AreEqual(150, orderedTicks.ElementAt(2).Price);
            Assert.AreEqual(150.15, orderedTicks.ElementAt(3).Price);
            Assert.AreEqual(150.43, orderedTicks.ElementAt(4).Price);
            Assert.AreEqual(151, orderedTicks.ElementAt(5).Price);
            Assert.AreEqual(151.12, orderedTicks.ElementAt(6).Price);
            Assert.AreEqual(152, orderedTicks.ElementAt(7).Price);
            Assert.AreEqual(152.38, orderedTicks.ElementAt(8).Price);
            Assert.AreEqual(152.40, orderedTicks.ElementAt(9).Price);
            Assert.AreEqual(152.49, orderedTicks.ElementAt(10).Price);
            Assert.AreEqual(155, orderedTicks.ElementAt(11).Price);
        }

        [TestMethod]
        public void OrderTicksByDate()
        {
            IEnumerable<Tick> orderedTicks = BarsFactory.OrderByDateTime(this.ticks);

            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 20, 20), orderedTicks.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 20, 25), orderedTicks.ElementAt(1).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 20, 28), orderedTicks.ElementAt(2).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 20, 33), orderedTicks.ElementAt(3).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 20, 38), orderedTicks.ElementAt(4).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 21, 10), orderedTicks.ElementAt(5).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 21, 20), orderedTicks.ElementAt(6).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 21, 23), orderedTicks.ElementAt(7).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 21, 45), orderedTicks.ElementAt(8).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 21, 58), orderedTicks.ElementAt(9).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 22, 20), orderedTicks.ElementAt(10).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 22, 45), orderedTicks.ElementAt(11).DateTime);
        }

        [TestMethod]
        public void GetPart()
        {
            long interval = TimeSpan.TicksPerMinute;
            long from = new DateTime(2012, 10, 23, 20, 20, 0).Ticks;
            long to = from + interval;

            IEnumerable<Tick> ticks = BarsFactory.GetTicksInterval(this.ticks, from, to);

            Assert.IsTrue(ticks.Count() == 5);

            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 20, 20), ticks.ElementAt(0).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 20, 25), ticks.ElementAt(1).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 20, 28), ticks.ElementAt(2).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 20, 33), ticks.ElementAt(3).DateTime);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 20, 38), ticks.ElementAt(4).DateTime);

        }

        [TestMethod]
        public void MakeFirstBar()
        {
            long interval = TimeSpan.TicksPerMinute;
            long from = new DateTime(2012, 10, 23, 20, 20, 0).Ticks;
            long to = from + interval;

            IEnumerable<Tick> ticks = BarsFactory.GetTicksInterval(this.ticks, from, to);

            Bar bar = BarsFactory.MakeBar(ticks, new DateTime(2012, 10, 23, 20, 20, 38));

            Assert.AreEqual(155, bar.High);
            Assert.AreEqual(149, bar.Low);
            Assert.AreEqual(150.43, bar.Open);
            Assert.AreEqual(149.55, bar.Close);
            Assert.AreEqual(500, bar.Volume);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 20, 38), bar.DateTime);
        }

        [TestMethod]
        public void MakeSecondBar()
        {
            long interval = TimeSpan.TicksPerMinute;
            long from = new DateTime(2012, 10, 23, 20, 21, 0).Ticks;
            long to = from + interval;

            IEnumerable<Tick> ticks = BarsFactory.GetTicksInterval(this.ticks, from, to);

            Bar bar = BarsFactory.MakeBar(ticks, new DateTime(2012, 10, 23, 20, 21, 58));

            Assert.AreEqual(152.4, bar.High);
            Assert.AreEqual(150.15, bar.Low);
            Assert.AreEqual(151, bar.Open);
            Assert.AreEqual(152.40, bar.Close);
            Assert.AreEqual(500, bar.Volume);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 21, 58), bar.DateTime);
        }

        [TestMethod]
        public void MakeThirdBar()
        {
            long interval = TimeSpan.TicksPerMinute;
            long from = new DateTime(2012, 10, 23, 20, 22, 0).Ticks;
            long to = from + interval;

            IEnumerable<Tick> ticks = BarsFactory.GetTicksInterval(this.ticks, from, to);

            Bar bar = BarsFactory.MakeBar(ticks, new DateTime(2012, 10, 23, 20, 22, 45));

            Assert.AreEqual(152.49, bar.High);
            Assert.AreEqual(150, bar.Low);
            Assert.AreEqual(152.49, bar.Open);
            Assert.AreEqual(150, bar.Close);
            Assert.AreEqual(200, bar.Volume);
            Assert.AreEqual(new DateTime(2012, 10, 23, 20, 22, 45), bar.DateTime);
        }

        [TestMethod]
        public void BarsFactory_Make_ForHour()
        {
            IEnumerable<Bar> bars = BarsFactory.Make(String.Concat(ProjectRootFolderNameFactory.Make(), "\\TestData\\SPFB.RTS.txt"));

            Assert.AreEqual(574, bars.Count());
        }

        [TestMethod]
        public void BarsFactory_Load_All_Bars_From_File()
        {
            IEnumerable<Bar> bars = BarsFactory.Make(String.Concat(ProjectRootFolderNameFactory.Make(), "\\App_Data\\Import\\SPFB.RTS_120901_121001_Hour.txt"));

            Assert.AreEqual(294, bars.Count());
        }

        [TestMethod]
        public void MakeBarFromTicks()
        {
            Bar bar = BarsFactory.MakeBar(MakeTicks(), new DateTime(2012, 12, 01, 10, 00, 15));

            Assert.AreEqual(142100, bar.High);
            Assert.AreEqual(138808, bar.Low);
            Assert.AreEqual(140000, bar.Open);
            Assert.AreEqual(141000, bar.Close);
            Assert.AreEqual(17000, bar.Volume);
            Assert.AreEqual(new DateTime(2012, 12, 01, 10, 00, 15), bar.DateTime);

        }

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

    }
}
