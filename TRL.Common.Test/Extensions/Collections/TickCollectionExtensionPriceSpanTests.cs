using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
//using TRL.Common.Extensions.Models;
//using TRL.Common.Extensions.Data;
using TRL.Common.Extensions.Collections;

namespace TRL.Common.Extensions.Collections.Test
{
    [TestClass]
    public class TickCollectionExtensionPriceSpanTests
    {
        private List<Tick> ticks;

        [TestInitialize]
        public void Setup()
        {
            this.ticks = new List<Tick>();

            this.ticks.Add(new Tick("SBRF-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 0), TradeAction.Buy, 10800, 90));
            this.ticks.Add(new Tick("RTS-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 0), TradeAction.Buy, 150000, 100));
            this.ticks.Add(new Tick("RTS-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 10), TradeAction.Buy, 151000, 100));
            this.ticks.Add(new Tick("RTS-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 20), TradeAction.Buy, 150000, 100));
            this.ticks.Add(new Tick("RTS-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 30), TradeAction.Buy, 151000, 100));
            this.ticks.Add(new Tick("RTS-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 40), TradeAction.Buy, 152000, 100));
            this.ticks.Add(new Tick("RTS-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 50), TradeAction.Buy, 154000, 100));
        }

        [TestMethod]
        public void PriceSpan_returns_zero_test()
        {
            TimeSpan ts = new TimeSpan(0, 0, 4);

            IEnumerable<Tick> result = this.ticks.Last("RTS-12.13_FT", ts);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(0, result.PriceSpan());
        }

        [TestMethod]
        public void PriceSpan_for_three_ticks_collection_test()
        {
            TimeSpan ts = new TimeSpan(0, 0, 21);

            IEnumerable<Tick> result = this.ticks.Last("RTS-12.13_FT", ts);

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(3000, result.PriceSpan());
        }

        [TestMethod]
        public void PriceSpan_for_six_ticks_collection_test()
        {
            TimeSpan ts = new TimeSpan(0, 2, 0);

            IEnumerable<Tick> result = this.ticks.Last("RTS-12.13_FT", ts);

            Assert.AreEqual(6, result.Count());
            Assert.AreEqual(4000, result.PriceSpan());
        }
    }
}
