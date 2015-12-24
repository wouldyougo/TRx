using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Test.Mocks;

namespace TRL.Common.Test
{
    [TestClass]
    public class MockTraderServiceTests
    {
        private IService service;

        [TestInitialize]
        public void Common_Setup()
        {
            this.service = new MockTraderService();
        }

        [TestMethod]
        public void Common_Service_Start_Stop()
        {
            this.service.Start();

            Assert.IsTrue(this.service.IsRunning);

            this.service.Stop();

            Assert.IsFalse(this.service.IsRunning);
        }

        [TestMethod]
        public void Common_Service_Restart()
        {
            Assert.IsFalse(this.service.IsRunning);

            this.service.Restart();

            Assert.IsTrue(this.service.IsRunning);
        }

        [TestMethod]
        public void Common_Service_Restart_Running()
        {
            this.service.Start();

            Assert.IsTrue(this.service.IsRunning);

            this.service.Restart();

            Assert.IsTrue(this.service.IsRunning);
        }

    }
}
