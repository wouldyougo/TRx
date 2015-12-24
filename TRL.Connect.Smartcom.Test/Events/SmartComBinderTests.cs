using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Test.Mocks;
using TRL.Common.Events;
using TRL.Connect.Smartcom.Events;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Data;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Test.Events
{
    [TestClass]
    public class SmartComBinderTests
    {
       
        private bool connectedExecuted = false;
        private bool otherConnectedExecuted = false;

        private IGenericSingleton<StServer> stServerSingleton;
        private StServer stServer;
        private SmartComHandlersDatabase handlers;
        private SmartComBinder binder;

        [TestInitialize]
        public void SetUp()
        {
            this.stServerSingleton = new StServerMockSingleton();
            this.stServer = this.stServerSingleton.Instance;
            this.handlers = new SmartComHandlersDatabase();
            this.binder = new SmartComBinder(this.stServerSingleton.Instance, this.handlers, new NullLogger());
        }

        [TestMethod]
        public void Bind_And_Unbind_An_Events()
        {
            this.handlers.Add<_IStClient_ConnectedEventHandler>(ConnectedHandler);
            this.handlers.Add<_IStClient_ConnectedEventHandler>(OtherConnectedHandler);

            this.binder.Bind();

            Assert.AreEqual(2, this.binder.BindedHandlersCounter);

            this.binder.Unbind();

            Assert.AreEqual(0, this.binder.BindedHandlersCounter);
        }

        [TestMethod]
        public void Bind_And_Execute_Handlers()
        {

            this.handlers.Add<_IStClient_ConnectedEventHandler>(ConnectedHandler);
            this.handlers.Add<_IStClient_ConnectedEventHandler>(OtherConnectedHandler);

            this.binder.Bind();

            this.stServer.connect("addr", 80, "login", "password");

            Assert.IsTrue(this.connectedExecuted);
            Assert.IsTrue(this.otherConnectedExecuted);

            this.binder.Unbind();
        }

        [TestMethod]
        public void Try_To_Bind_No_Handlers_Do_Nothing()
        {
            Assert.AreEqual(0, this.handlers.HandlerCounter);

            this.binder.Bind();

            Assert.AreEqual(0, this.binder.BindedHandlersCounter);
        }

        [TestMethod]
        public void Try_To_Unbind_When_No_Bindings_Do_Nothing()
        {
            this.binder.Unbind();

            Assert.AreEqual(0, this.binder.BindedHandlersCounter);
        }

        public void ConnectedHandler()
        {
            this.connectedExecuted = true;
        }

        public void OtherConnectedHandler()
        {
            this.otherConnectedExecuted = true;
        }
    }
}
