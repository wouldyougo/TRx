using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Events;
using TRL.Connect.Smartcom;

namespace TRL.Connect.Smartcom.Test.Mocks
{
    [TestClass]
    public class FakeAdapterBaseTests
    {
        private FakeAdapterBase adapterBase;

        [TestInitialize]
        public void Setup()
        {
            this.adapterBase = new FakeAdapterBase();
        }

        [TestMethod]
        public void FakeAdapterBase_contains_HandlersDatabase_test()
        {
            Assert.IsNotNull(this.adapterBase.Handlers);
            Assert.IsInstanceOfType(this.adapterBase.Handlers, typeof(SmartComHandlersDatabase));
        }

        [TestMethod]
        public void FakeAdapterBase_contains_Binder_test()
        {
            Assert.IsNotNull(this.adapterBase.Binder);
            Assert.IsInstanceOfType(this.adapterBase.Binder, typeof(SmartComBinder));
        }

        [TestMethod]
        public void FakeAdapterBase_contains_StServerMockSingleton_test()
        {
            Assert.IsNotNull(this.adapterBase.StServerMockSingleton);
            Assert.IsInstanceOfType(this.adapterBase.StServerMockSingleton, typeof(StServerMockSingleton));
        }

        [TestMethod]
        public void FakeAdapterBase_contains_SmartComSubscriber_test()
        {
            Assert.IsNotNull(this.adapterBase.Subscriber);
            Assert.IsInstanceOfType(this.adapterBase.Subscriber, typeof(SmartComSubscriber));
        }

        [TestMethod]
        public void FakeAdapterBase_contains_SmartComConnector_test()
        {
            Assert.IsNotNull(this.adapterBase.Connector);
            Assert.IsInstanceOfType(this.adapterBase.Connector, typeof(SmartComConnector));
        }
    }
}
