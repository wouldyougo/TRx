using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Data;
using SmartCOM3Lib;
using TRL.Common.Data;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class SmartComHandlersTests
    {
        [TestMethod]
        public void SmartComHandlers_Is_Database()
        {
            Assert.IsTrue(SmartComHandlers.Instance is IDatabase);
        }

        [TestMethod]
        public void SmartComHandlers_Is_StrictDataContext()
        {
            Assert.IsTrue(SmartComHandlers.Instance is IStrictDataContext);
        }

        [TestMethod]
        public void Create_SmartComHandlers_Test()
        {
            SmartComHandlers h = SmartComHandlers.Instance;
            SmartComHandlers h2 = SmartComHandlers.Instance;

            Assert.AreSame(h, h2);
            Assert.AreEqual(h2.HandlerCounter, h.HandlerCounter);
        }

        [TestMethod]
        public void SmartComHandlers_Add_Handler()
        {
            SmartComHandlers h = SmartComHandlers.Instance;

            int count = h.HandlerCounter;

            h.Add<_IStClient_ConnectedEventHandler>(ConnectedHandler);

            Assert.AreEqual(count + 1, h.HandlerCounter);
        }

        [TestMethod]
        public void SmartComHandlers_Add_Different_Handlers()
        {
            SmartComHandlers h = SmartComHandlers.Instance;
            SmartComHandlers h2 = SmartComHandlers.Instance;

            int count = h.HandlerCounter;

            h.Add<_IStClient_ConnectedEventHandler>(ConnectedHandler);
            h2.Add<_IStClient_DisconnectedEventHandler>(DisconnectedHandler);

            Assert.AreEqual(count + 2, h.HandlerCounter);
            Assert.AreEqual(h2.HandlerCounter, h.HandlerCounter);
        }

        [TestMethod]
        public void SmartComHandlers_Remove_Handler()
        {
            SmartComHandlers h = SmartComHandlers.Instance;
            SmartComHandlers h2 = SmartComHandlers.Instance;

            int count = h.HandlerCounter;

            h.Add<_IStClient_ConnectedEventHandler>(ConnectedHandler);
            h2.Add<_IStClient_DisconnectedEventHandler>(DisconnectedHandler);

            h.Remove<_IStClient_ConnectedEventHandler>(ConnectedHandler);
            h2.Remove<_IStClient_DisconnectedEventHandler>(DisconnectedHandler);

            Assert.AreEqual(count, h.HandlerCounter);
            Assert.AreEqual(h2.HandlerCounter, h.HandlerCounter);
        }

        public void ConnectedHandler()
        {
        }

        public void DisconnectedHandler(string str)
        {
        }
    }
}
