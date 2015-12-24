using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Test.Mocks;
using TRL.Common.Collections;

namespace TRL.Common.Test.Collections
{
    [TestClass]
    public class ObservableQueueTests
    {

        private ObservableQueue<string> queue;
        private MockQueueObserver observer;

        [TestInitialize]
        public void Collections_SetUp()
        {
            this.queue = new ObservableQueue<string>();

            this.observer = new MockQueueObserver(this.queue);

            this.queue.RegisterObserver(this.observer);
        }

        [TestMethod]
        public void Collections_ObservableQueue_Notify_Observer()
        {
            Assert.AreEqual("No data", this.observer.Data);

            this.queue.Enqueue("New data");

            Assert.AreEqual("New data", this.observer.Data);
        }
    }
}
