using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
//using TRL.Common.Extensions.Models;
using TRL.Common.Extensions.Data;
using TRL.Common.Collections;
using TRL.Common.Extensions.Collections;

namespace TRL.Common.Extensions.Collections.Test
{
    [TestClass]
    public class BarCollectionExtensionsTests
    {
        private IDataContext tradingData;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
        }

        [TestMethod]
        public void get_newest_thirty_minutes_rts_bars()
        {
            add_minute_bars_to_marketData("RTS-6.13_FT", 60);
            add_minute_bars_to_marketData("Si-6.13_FT", 60);

            int count = 30;
            int interval = 60;
            IEnumerable<Bar> bars = this.tradingData.Get<IEnumerable<Bar>>().GetNewestBars("RTS-6.13_FT", interval, count);

            Assert.AreEqual(30, bars.Count());

            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual("RTS-6.13_FT", bars.ElementAt(i).Symbol);
                Assert.AreEqual(new DateTime(2013, 5, 10, 10, i + 30, 0), bars.ElementAt(i).DateTime);
            }
        }

        private void add_minute_bars_to_marketData(string symbol, int minuteCount)
        {
            if (minuteCount > 60)
                minuteCount = 60;

            for(int i = 0 ; i < minuteCount; i++)
                this.tradingData.Get<ICollection<Bar>>().Add(new Bar (symbol, 60, new DateTime(2013, 5, 10, 10, i, 0), 150000, 155000 + i, 149000, 153000, 55000 ));
        }

        [TestMethod]
        public void get_newest_bar()
        {
            add_minute_bars_to_marketData("RTS-6.13_FT", 60);
            add_minute_bars_to_marketData("Si-6.13_FT", 60);

            Bar bar = this.tradingData.Get<IEnumerable<Bar>>().GetNewestBar("RTS-6.13_FT", 60);

            Assert.AreEqual("RTS-6.13_FT", bar.Symbol);
            Assert.AreEqual(new DateTime(2013, 5, 10, 10, 59, 0), bar.DateTime);
            Assert.AreEqual(155059, bar.High);
        }

        public void AddNeutralBars(string symbol, ICollection<Bar> collection)
        {
            collection.Add(new Bar(symbol, 60, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
            collection.Add(new Bar(symbol, 60, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
            collection.Add(new Bar(symbol, 60, new DateTime(2014, 1, 10, 11, 2, 0), 12, 16, 11, 14, 100));
        }

        public void AddBreakToHighBars(string symbol, ICollection<Bar> collection)
        {
            collection.Add(new Bar(symbol, 60, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
            collection.Add(new Bar(symbol, 60, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
            collection.Add(new Bar(symbol, 60, new DateTime(2014, 1, 10, 11, 2, 0), 12, 19, 11, 16, 100));
        }

        public void AddBreakToLowBars(string symbol, ICollection<Bar> collection)
        {
            collection.Add(new Bar(symbol, 60, new DateTime(2014, 1, 10, 11, 0, 0), 12, 16, 10, 15, 100));
            collection.Add(new Bar(symbol, 60, new DateTime(2014, 1, 10, 11, 1, 0), 11, 15, 10, 14, 100));
            collection.Add(new Bar(symbol, 60, new DateTime(2014, 1, 10, 11, 2, 0), 12, 13, 8, 11, 100));
        }

        [TestMethod]
        public void LastBarHasHighestHigh_with_neutral_bars_returns_false_test()
        {
            AddNeutralBars("RTS", this.tradingData.Get<ICollection<Bar>>());

            Assert.IsFalse(this.tradingData.Get<IEnumerable<Bar>>().LastBarHasHighestHigh());
        }

        [TestMethod]
        public void LastBarHasHighestHigh_with_break_to_high_bars_returns_true_test()
        {
            AddBreakToHighBars("RTS", this.tradingData.Get<ICollection<Bar>>());

            Assert.IsTrue(this.tradingData.Get<IEnumerable<Bar>>().LastBarHasHighestHigh());
        }

        [TestMethod]
        public void LastBarHasHighestHigh_with_break_to_low_bars_returns_false_test()
        {
            AddBreakToLowBars("RTS", this.tradingData.Get<ICollection<Bar>>());

            Assert.IsFalse(this.tradingData.Get<IEnumerable<Bar>>().LastBarHasHighestHigh());
        }

        [TestMethod]
        public void LastBarHasLowestLow_with_neutral_bars_returns_false_test()
        {
            AddNeutralBars("RTS", this.tradingData.Get<ICollection<Bar>>());

            Assert.IsFalse(this.tradingData.Get<IEnumerable<Bar>>().LastBarHasLowestLow());
        }

        [TestMethod]
        public void LastBarHasLowestLow_with_break_to_high_bars_returns_false_test()
        {
            AddBreakToHighBars("RTS", this.tradingData.Get<ICollection<Bar>>());

            Assert.IsFalse(this.tradingData.Get<IEnumerable<Bar>>().LastBarHasLowestLow());
        }

        [TestMethod]
        public void LastBarHasLowestLow_with_break_to_low_bars_returns_true_test()
        {
            AddBreakToLowBars("RTS", this.tradingData.Get<ICollection<Bar>>());

            Assert.IsTrue(this.tradingData.Get<IEnumerable<Bar>>().LastBarHasLowestLow());
        }
    }
}
