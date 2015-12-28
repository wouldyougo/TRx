using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Test.Mocks;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Events;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Net;
using TRL.Message;
using System.Threading;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Test
{
    [TestClass]
    public class SmartComAdapterTests
    {
        private StServerMockSingleton stServerSingleton;
        private bool isConnected;

        private StServer stServer;
        private SmartComHandlersDatabase handlers;
        private SmartComBinder binder;
        private SmartComConnector connector;
        private SmartComSubscriber subscriber;

        private SmartComAdapter trader;

        [TestInitialize]
        public void Setup()
        {
            this.stServerSingleton = new StServerMockSingleton();
            this.stServer = this.stServerSingleton.Instance;
            this.handlers = new SmartComHandlersDatabase();
            this.handlers.Add<_IStClient_ConnectedEventHandler>(ConnectedHandler);
            this.handlers.Add<_IStClient_DisconnectedEventHandler>(DisconnectedHandler);
            this.binder = new SmartComBinder(this.stServer, this.handlers, new NullLogger());
            this.connector = new SmartComConnector(this.stServer, this.handlers, new NullLogger());
            this.subscriber = new SmartComSubscriber(this.stServer, new NullLogger());
            this.subscriber.Portfolios.Add("ST88888-RF-01");
            this.subscriber.Ticks.Add("RTS-6.13_FT");
            this.subscriber.Quotes.Add("RTS-6.13_FT");
            this.subscriber.BidsAndAsks.Add("RTS-6.13_FT");

            this.trader = new SmartComAdapter(this.connector, 
                this.handlers, 
                this.binder, 
                this.subscriber, 
                this.stServerSingleton, 
                new NullLogger(),
                1);

            Assert.AreEqual(0, this.subscriber.SubscriptionsCounter);
        }

        [TestCleanup]
        public void Teardown()
        {
            this.trader.Stop();
        }

        public void ConnectedHandler()
        {
            this.isConnected = true;
        }

        public void DisconnectedHandler(string reason)
        {
            this.isConnected = false;
        }

        [TestMethod]
        public void SmartComTrader_Bind_Handlers_After_Start()
        {
            Assert.AreEqual(6, this.handlers.HandlerCounter);
            Assert.AreEqual(0, this.binder.BindedHandlersCounter);

            this.trader.Start();

            Assert.IsTrue(this.trader.IsRunning);
            Assert.AreEqual(6, this.binder.BindedHandlersCounter);
        }

        [TestMethod]
        public void SmartComTrader_Unbind_Handlers_After_Stop()
        {
            this.trader.Start();

            Assert.IsTrue(this.trader.IsRunning);
            Assert.AreEqual(6, this.binder.BindedHandlersCounter);

            this.trader.Stop();

            Assert.AreEqual(0, this.binder.BindedHandlersCounter);
            Assert.IsFalse(this.trader.IsRunning);
        }

        [TestMethod]
        public void SmartComTrader_Connect_After_Start()
        {
            Assert.IsFalse(this.isConnected);

            this.trader.Start();

            Assert.IsTrue(this.isConnected);

            this.trader.Stop();

        }

        [TestMethod]
        public void SmartComTrader_Disconnects_After_Stop()
        {
            Assert.IsFalse(this.isConnected);

            this.trader.Start();

            Assert.IsTrue(this.isConnected);

            this.trader.Stop();

            Assert.IsFalse(this.isConnected);
        }

        [TestMethod]
        public void SmartComTrader_Restore_Lost_Connection()
        {
            Assert.IsFalse(this.isConnected);

            this.trader.Start();

            Assert.IsTrue(this.isConnected);

            ((StServerClassMock)this.stServer).EmulateDisconnect();

            Thread.Sleep(1100);

            Assert.IsTrue(this.isConnected);

            this.trader.Stop();
        }

        [TestMethod]
        public void SmartComTrader_Subscribe_After_Start()
        {
            this.trader.Start();

            Assert.AreEqual(4, this.subscriber.SubscriptionsCounter);

            this.trader.Stop();
        }

        [TestMethod]
        public void Unsubscribe_just_if_connection_was_established_test()
        {
            ((StServerClassMock)this.stServer).ProhibitConnection();

            this.trader.Start();

            Assert.AreEqual(0, this.subscriber.SubscriptionsCounter);

            this.trader.Stop();

            Assert.AreEqual(0, this.subscriber.SubscriptionsCounter);
        }
    }
}
