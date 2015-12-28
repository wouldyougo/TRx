using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Test.Mocks;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class StServerFactoryTests
    {
        private StServerMockSingleton factory;
        private StServer first;

        [TestInitialize]
        public void Setup()
        {
            this.factory = new StServerMockSingleton();
        }

        [TestMethod]
        public void Factory_Makes_New_StServer()
        {
            this.first = this.factory.Instance;

            Assert.IsTrue(this.first != null);
        }
    }
}
