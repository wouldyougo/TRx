using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Collections;
using TRL.Common.Models;

namespace TRL.Common.Test.Collections
{
    [TestClass]
    public class TradeSignalQueueTests
    {
        [TestMethod]
        public void Collections_TradeSignalQueue_Instance_Is_ObservableQueue()
        {
            Assert.IsInstanceOfType(SignalQueue.Instance, typeof(ObservableQueue<Signal>));
        }

        [TestMethod]
        public void Collections_TradeSignalQueue_Instance()
        {
            Assert.IsNotNull(SignalQueue.Instance);
        }

        [TestMethod]
        public void Collections_TradeSignalQueue_Is_Singleton()
        {
            SignalQueue q = SignalQueue.Instance;
            SignalQueue q2 = SignalQueue.Instance;

            Assert.AreSame(q, q2);
        }
    }
}
