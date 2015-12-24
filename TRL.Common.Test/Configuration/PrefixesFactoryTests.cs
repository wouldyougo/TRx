using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Configuration;

namespace TRL.Common.Test.Configuration
{
    [TestClass]
    public class PrefixesFactoryTests
    {
        [TestMethod]
        public void Configuration_Make_Prefixes()
        {
            IEnumerable<string> prefixes = Prefixes.Make();

            Assert.AreEqual(3, prefixes.Count());
            Assert.AreEqual("RTSX", prefixes.ElementAt(0));
            Assert.AreEqual("RTSZ", prefixes.ElementAt(1));
            Assert.AreEqual("SiB", prefixes.ElementAt(2));
        }
    }
}
