using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TRL.Common.Test
{
    [TestClass]
    public class ConnectionCredentialsTests
    {
        [TestMethod]
        public void Common_ConnectionCredentialsConstructs()
        {
            ConnectionCredentials connectionCredentials = new ConnectionCredentials();

            Assert.AreEqual("95.131.26.246", connectionCredentials.Host);
            Assert.AreEqual(8090, connectionCredentials.Port);
            Assert.AreEqual("ST30151", connectionCredentials.Login);
            Assert.AreEqual("39SWRK", connectionCredentials.Password);
        }
    }
}
