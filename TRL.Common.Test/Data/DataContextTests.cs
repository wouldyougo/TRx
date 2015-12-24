using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using System.Reflection;
using TRL.Common.Data;
using TRL.Common.Collections;

namespace TRL.Common.Test.Data
{
    public class SampleDataContext : RawBaseDataContext
    {
        public int Counter { get; private set; }
        public ObservableCollection<Bar> Bars { get; private set; }
        public ObservableCollection<Tick> Ticks { get; private set; }

        public SampleDataContext()
        {
            this.Counter = 5;
            this.Bars = new ObservableCollection<Bar>();
            this.Ticks = new ObservableCollection<Tick>();
        }
    }

    [TestClass]
    public class DataContextTests
    {

        [TestMethod]
        public void DataContext_sample_test()
        {
            IDataContext dt = new SampleDataContext();
            
            int counter = dt.Get<int>();
            Assert.AreEqual(5, counter);

            IEnumerable<Bar> bars = dt.Get<IEnumerable<Bar>>();
            Assert.IsNotNull(bars);
            Assert.AreEqual(0, bars.Count());

            IEnumerable<Tick> ticks = dt.Get<List<Tick>>();
            Assert.IsNotNull(ticks);
            Assert.AreEqual(0, ticks.Count());

        }

        [TestMethod]
        public void DataContext_returns_defaults_for_nonexistent_properties()
        {
            IDataContext dt = new SampleDataContext();

            double price = dt.Get<double>();
            Assert.AreEqual(0, price);

            IEnumerable<BidAsk> bids = dt.Get<List<BidAsk>>();
            Assert.IsNull(bids);
        }
    }
}
