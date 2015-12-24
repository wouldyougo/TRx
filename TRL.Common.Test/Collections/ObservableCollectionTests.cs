using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Collections;
using TRL.Common.Events;

namespace TRL.Common.Test.Collections
{
    public class SampleObserver: IGenericObserver<double>
    {
        private ObservableCollection<double> collection;

        public SampleObserver(ObservableCollection<double> collection)
        {
            this.collection = collection;
            this.collection.RegisterObserver(this);
        }

        private double inner;
        public double Inner
        {
            get
            {
                return this.inner;
            }
        }

        public void Update(double item)
        {
            this.inner = item;
        }
    }

    [TestClass]
    public class ObservableCollectionTests
    {
        private ObservableCollection<double> collection;
        private SampleObserver observer;
        private double lastValue;

        [TestInitialize]
        public void Collections_Setup()
        {
            this.collection = new ObservableCollection<double>();
            this.observer = new SampleObserver(this.collection);
            this.collection.OnItemAdded += new ItemAddedNotification<double>(Update);
        }

        private void Update(double value)
        {
            this.lastValue = value;
        }

        [TestMethod]
        public void Collections_ObservableCollection_ExecuteUpdate_OnItemAdded_test()
        {
            Assert.AreEqual(0, this.lastValue);

            this.collection.Add(255.00);

            Assert.AreEqual(255.0, this.lastValue);

            this.collection.Add(38.01);

            Assert.AreEqual(38.01, this.lastValue);
        }

        [TestMethod]
        public void Collections_ObservableCollection_Updates_internal_test()
        {
            Assert.AreEqual(0, this.observer.Inner);

            this.collection.Add(55.25);

            Assert.AreEqual(55.25, this.observer.Inner);
        }
    }
}
