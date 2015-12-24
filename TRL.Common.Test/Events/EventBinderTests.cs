using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Events;
using TRL.Common.Test.Mocks;

namespace TRL.Common.Test.Events
{
    [TestClass]
    public class EventBinderTests
    {
        [TestMethod]
        public void EventBinder_Bind_And_Unbind_An_Events()
        {
            IGenericBinder binder = new MockEventBinder();

            MockEventBinder mockEventBinder = (MockEventBinder)binder;

            binder.Bind<MockEvent>(MockEventHandler);
            Assert.AreEqual(1, mockEventBinder.BindedEventsCounter);

            binder.Bind<MockEvent>(MockEventHandler);
            Assert.AreEqual(2, mockEventBinder.BindedEventsCounter);



            binder.Unbind<MockEvent>(MockEventHandler);
            Assert.AreEqual(1, mockEventBinder.BindedEventsCounter);

            binder.Unbind<MockEvent>(MockEventHandler);
            Assert.AreEqual(0, mockEventBinder.BindedEventsCounter);
        }

        public void MockEventHandler()
        {
        }
    }
}
