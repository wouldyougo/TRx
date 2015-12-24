using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TRL.Connect.Smartcom.Test
{
    [TestClass]
    public class SmartComEventTypesTests
    {
        [TestMethod]
        public void SmartComEventsTypes_Contains_Twenty_Four_Records()
        {
            IEnumerable<Type> events = SmartComEventsTypes.Collection;

            Assert.AreEqual(23, events.Count());
        }

        [TestMethod]
        public void SmartComEventsTypes_Initialize()
        {
            IEnumerable<Type> t = SmartComEventsTypes.Collection;
            IEnumerable<Type> t2 = SmartComEventsTypes.Collection;

            Assert.AreSame(t, t2);            
        }

    }
}
