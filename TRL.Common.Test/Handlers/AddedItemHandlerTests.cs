using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Collections;
using TRL.Common.Events;
using TRL.Common.Handlers;

namespace TRL.Common.Handlers.Test
{
    public class SampleAddedItemHandler : AddedItemHandler<double>
    {
        public SampleAddedItemHandler(ItemAddedNotifier<double> notifier)
            :base(notifier){}

        private double inner;
        public double Inner
        {
            get
            {
                return this.inner;
            }
        }

        public override void OnItemAdded(double item)
        {
            this.inner = item;
        }
    }

    [TestClass]
    public class AddedItemHandlerTests
    {
        private ObservableCollection<double> collection;
        private SampleAddedItemHandler handler;

        [TestInitialize]
        public void Handlers_Setup()
        {
            this.collection = new ObservableCollection<double>();
            this.handler = new SampleAddedItemHandler(this.collection);
        }

        [TestMethod]
        public void Handlers_AddedItemHander_Execute_OnItemAdded_test()
        {
            Assert.AreEqual(0, this.handler.Inner);

            this.collection.Add(28);

            Assert.AreEqual(28, this.handler.Inner);
        }
    }
}
