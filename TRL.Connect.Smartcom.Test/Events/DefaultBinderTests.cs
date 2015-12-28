using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Events;

namespace TRL.Connect.Smartcom.Test.Events
{
    [TestClass]
    public class DefaultBinderTests
    {
        [TestMethod]
        public void DefaultBinder_IsSingleton()
        {
            SmartComBinder b = DefaultBinder.Instance;
            SmartComBinder b2 = DefaultBinder.Instance;

            Assert.AreSame(b, b2);
        }
    }
}
