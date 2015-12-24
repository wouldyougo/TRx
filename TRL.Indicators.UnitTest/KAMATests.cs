using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TRL.Common.Models;
using TRL.Indicators;

namespace TRL.Indicators.Test
{
    [TestClass]
    public class KAMATests
    {
        [TestMethod]
        public void Indicators_KAMA_Make_returns_proper_amount_of_results()
        {
            List<Bar> bars = new List<Bar>();

            bars.Add(new Bar(DateTime.Now, 140000, 144000, 139000, 143000, 100));
            bars.Add(new Bar(DateTime.Now, 140100, 144100, 139100, 143100, 100));
            bars.Add(new Bar(DateTime.Now, 140200, 144200, 139200, 143200, 100));
            bars.Add(new Bar(DateTime.Now, 140300, 144300, 139300, 143300, 100));
            bars.Add(new Bar(DateTime.Now, 140400, 144400, 139400, 143400, 100));
            bars.Add(new Bar(DateTime.Now, 140500, 144500, 139500, 143500, 100));

            IEnumerable<double> result = KAMA.Make(bars, 5, 2, 30);

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void Indicators_KAMA_Make_returns_empty_result_if_period_exceeds_amount_of_source_bars()
        {
            List<Bar> bars = new List<Bar>();

            bars.Add(new Bar(DateTime.Now, 140000, 144000, 139000, 143000, 100));
            bars.Add(new Bar(DateTime.Now, 140100, 144100, 139100, 143100, 100));
            bars.Add(new Bar(DateTime.Now, 140200, 144200, 139200, 143200, 100));
            bars.Add(new Bar(DateTime.Now, 140300, 144300, 139300, 143300, 100));
            bars.Add(new Bar(DateTime.Now, 140400, 144400, 139400, 143400, 100));
            bars.Add(new Bar(DateTime.Now, 140500, 144500, 139500, 143500, 100));

            int period = 7;

            Assert.IsTrue(period > bars.Count);

            IEnumerable<double> result = KAMA.Make(bars, period, 2, 30);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Indicators_KAMA_Make_returns_empty_result_if_period_equals_to_amount_of_source_bars()
        {
            List<Bar> bars = new List<Bar>();

            bars.Add(new Bar(DateTime.Now, 140000, 144000, 139000, 143000, 100));
            bars.Add(new Bar(DateTime.Now, 140100, 144100, 139100, 143100, 100));
            bars.Add(new Bar(DateTime.Now, 140200, 144200, 139200, 143200, 100));
            bars.Add(new Bar(DateTime.Now, 140300, 144300, 139300, 143300, 100));
            bars.Add(new Bar(DateTime.Now, 140400, 144400, 139400, 143400, 100));
            bars.Add(new Bar(DateTime.Now, 140500, 144500, 139500, 143500, 100));

            int period = 6;

            Assert.AreEqual(period, bars.Count);

            IEnumerable<double> result = KAMA.Make(bars, period, 2, 30);

            Assert.AreEqual(0, result.Count());
        }
    }
}
