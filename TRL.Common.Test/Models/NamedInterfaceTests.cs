using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Collections;
using TRL.Common.Models;
using TRL.Common.Test.Mocks;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class NamedInterfaceTests
    {
        [TestMethod]
        public void NamedCollection_test()
        {
            NamedObservableCollection<Bar> bars = new NamedObservableCollection<Bar>("Bar's collection");
            Assert.IsInstanceOfType(bars, typeof(INamed));
            Assert.AreEqual("Bar's collection", bars.Name);
        }
    }
}
