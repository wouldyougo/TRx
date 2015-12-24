using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Logging;

namespace TRL.Common.Test
{
    [TestClass]
    public class DefaultLoggerTests
    {
        [TestMethod]
        public void Common_DefaultLogger_Instance_Is_Singleton()
        {
            ILogger l = DefaultLogger.Instance;
            ILogger l1 = DefaultLogger.Instance;

            Assert.AreSame(l, l1);
        }
    }
}
