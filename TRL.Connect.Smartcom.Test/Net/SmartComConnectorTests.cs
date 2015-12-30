using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Message;
using TRL.Connect.Smartcom.Test.Mocks;
using TRL.Connect.Smartcom;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Events;
using TRL.Common.Data;
using System.Threading;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Test.Connector
{
	[TestClass]
	public class SmartComConnectorTests
	{
        private StServer stServer;
        private SmartComHandlersDatabase handlers;
        private SmartComBinder binder;

        private SmartComConnector connector;


        [TestInitialize]
        public void Setup()
        {
            this.stServer = new StServerClassMock();
            this.handlers = new SmartComHandlersDatabase();
            this.binder = new SmartComBinder(this.stServer, this.handlers, new NullLogger());
            this.connector = new SmartComConnector(this.stServer, this.handlers, new NullLogger());
            this.binder.Bind();
        }

        [TestCleanup]
        public void TearDown()
        {
            this.binder.Unbind();
        }

		[TestMethod]
		public void Connector_Connects()
		{
            Assert.IsFalse(this.connector.IsConnected);

            this.connector.Connect();
       
            Assert.IsTrue(this.connector.IsConnected);
        }

        [TestMethod]
        public void Connector_Disconnect()
        {
            this.connector.Connect();

            this.connector.Disconnect();

            Assert.IsFalse(this.connector.IsConnected);
        }

        [TestMethod]
        public void Connector_Reports_About_Connection_Lost()
        {
            this.connector.Connect();

            ((StServerClassMock)this.stServer).EmulateDisconnect();

            Assert.IsFalse(this.connector.IsConnected);
        }

    }
}
