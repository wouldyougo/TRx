using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Test.Mocks;
using TRL.Message;

namespace TRL.Message.Test
{
    [TestClass]
    public class ConnectorTests
    {
        private IConnector connector;

        [TestInitialize]
        public void Setup()
        {
            this.connector = new MockConnector();
        }

        [TestMethod]
        public void Message_Connection_Lost()
        {
            Assert.IsFalse(this.connector.IsConnected);

            this.connector.Connect();

            Assert.IsTrue(this.connector.IsConnected);

            EmulateDisconnect();

            Assert.IsFalse(this.connector.IsConnected);
        }

        private void EmulateDisconnect()
        {
            MockConnector mc = (MockConnector)this.connector;

            mc.EmulateDisconnect();
        }
    }
}
