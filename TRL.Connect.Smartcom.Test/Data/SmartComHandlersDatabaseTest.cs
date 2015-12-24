using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Data;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class SmartComHandlersDatabaseTest
    {
        private SmartComHandlersDatabase smartComHandlers;

        [TestInitialize]
        public void Setup()
        {
            this.smartComHandlers = new SmartComHandlersDatabase();
        }

        [TestMethod]
        public void Add_Handler()
        {
            Assert.AreEqual(0, this.smartComHandlers.GetData<_IStClient_ConnectedEventHandler>().Count);

            this.smartComHandlers.Add<_IStClient_ConnectedEventHandler>(FirstConnectedHandler);

            Assert.AreEqual(1, this.smartComHandlers.GetData<_IStClient_ConnectedEventHandler>().Count);
            Assert.AreEqual(1, this.smartComHandlers.HandlerCounter);
        }

        [TestMethod]
        public void You_Can_Add_One_Handler_Only_One_Time()
        {
            Assert.AreEqual(0, this.smartComHandlers.GetData<_IStClient_ConnectedEventHandler>().Count);

            this.smartComHandlers.Add<_IStClient_ConnectedEventHandler>(FirstConnectedHandler);
            this.smartComHandlers.Add<_IStClient_ConnectedEventHandler>(FirstConnectedHandler);

            Assert.AreEqual(1, this.smartComHandlers.GetData<_IStClient_ConnectedEventHandler>().Count);
            Assert.AreEqual(1, this.smartComHandlers.HandlerCounter);
        }

        [TestMethod]
        public void You_Can_Add_Many_Different_Handlers()
        {
            Assert.AreEqual(0, this.smartComHandlers.GetData<_IStClient_ConnectedEventHandler>().Count);

            this.smartComHandlers.Add<_IStClient_ConnectedEventHandler>(FirstConnectedHandler);
            this.smartComHandlers.Add<_IStClient_ConnectedEventHandler>(SecondConnectedHandler);

            Assert.AreEqual(2, this.smartComHandlers.GetData<_IStClient_ConnectedEventHandler>().Count);
            Assert.AreEqual(2, this.smartComHandlers.HandlerCounter);
        }

        public void FirstConnectedHandler()
        {
        }

        public void SecondConnectedHandler()
        {
        }
    }
}
