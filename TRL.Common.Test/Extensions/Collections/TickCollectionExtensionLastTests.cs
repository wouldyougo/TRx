using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
//using TRL.Common.Extensions.Data;
//using TRL.Common.Extensions.Models;
using TRL.Common.Extensions.Collections;

namespace TRL.Common.Extensions.Collections.Test
{
    [TestClass]
    public class TickCollectionExtensionLastTests
    {
        private List<Tick> ticks;

        [TestInitialize]
        public void Setup()
        {
            this.ticks = new List<Tick>();

            this.ticks.Add(new Tick("SBRF-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 0), 10800, 90));
            this.ticks.Add(new Tick("RTS-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 0) , 150000, 100));
            this.ticks.Add(new Tick("RTS-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 10), 150000, 100));
            this.ticks.Add(new Tick("RTS-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 20), 150000, 100));
            this.ticks.Add(new Tick("RTS-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 30), 150000, 100));
            this.ticks.Add(new Tick("RTS-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 40), 150000, 100));
            this.ticks.Add(new Tick("RTS-12.13_FT", new DateTime(2013, 1, 1, 10, 0, 50), 150000, 100));
        }

        [TestMethod]
        public void Last_returns_null_test()
        {
            TimeSpan ts = new TimeSpan(0, 0, 4);

            IEnumerable<Tick> result = this.ticks.Last("Si-12.13_FT", ts);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Last_returns_one_tick_collection_tests()
        {
            TimeSpan ts = new TimeSpan(0, 0, 5);

            IEnumerable<Tick> result = this.ticks.Last("RTS-12.13_FT", ts);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(new DateTime(2013, 1, 1, 10, 0, 50), result.First().DateTime);
        }

        [TestMethod]
        public void Last_returns_three_ticks_collection_test()
        {
            TimeSpan ts = new TimeSpan(0, 0, 21);

            IEnumerable<Tick> result = this.ticks.Last("RTS-12.13_FT", ts);

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(new DateTime(2013, 1, 1, 10, 0, 30), result.First().DateTime);
            Assert.AreEqual(new DateTime(2013, 1, 1, 10, 0, 50), result.Last().DateTime);
        }

        [TestMethod]
        public void Last_returns_six_ticks_collection_test()
        {
            TimeSpan ts = new TimeSpan(0, 2, 0);

            IEnumerable<Tick> result = this.ticks.Last("RTS-12.13_FT", ts);

            Assert.AreEqual(6, result.Count());
            Assert.AreEqual(new DateTime(2013, 1, 1, 10, 0, 0), result.First().DateTime);
            Assert.AreEqual(new DateTime(2013, 1, 1, 10, 0, 50), result.Last().DateTime);
        }

        [TestMethod]
        public void Last_returns_three_last_ticks()
        {
            int count = 3;

            IEnumerable<Tick> result = this.ticks.Last("RTS-12.13_FT", count);

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(new DateTime(2013, 1, 1, 10, 0, 30), result.First().DateTime);
            Assert.AreEqual(new DateTime(2013, 1, 1, 10, 0, 50), result.Last().DateTime);
        }
    }
}
