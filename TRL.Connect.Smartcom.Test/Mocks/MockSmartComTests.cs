using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartCOM3Lib;

namespace TRL.Connect.Smartcom.Test.Mocks
{
    [TestClass]
    public class MockSmartComTests
    {
        [TestMethod]
        public void MockSmartCom_Is_Singleton()
        {
            StServer s = MockSmartCom.Instance;
            StServer s2 = MockSmartCom.Instance;

            Assert.AreSame(s, s2);
        }
    }
}
