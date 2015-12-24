using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
//using TRL.Common.Extensions.Data;
//using TRL.Common.Extensions.Models;
using TRL.Common.Extensions.Collections;

namespace TRL.Common.Extensions.Collections.Test
{
    [TestClass]
    public class ListExtensionsTests
    {
        [TestMethod]
        public void ListExtensions_Collection_IsEmpty()
        {
            List<Tick> ticks = new List<Tick>();

            Assert.IsFalse(ticks.ItemsAreOlderThanSeconds(300));
        }

        [TestMethod]
        public void ListExtensions_ItemsAreOlder()
        {
            List<Tick> ticks = new List<Tick>();

            ticks.Add(new Tick { DateTime = BrokerDateTime.Make(DateTime.Now).AddSeconds(-350) });

            Assert.IsTrue(ticks.ItemsAreOlderThanSeconds(300));
        }

        [TestMethod]
        public void ListExtensions_ItemsAreFresh()
        {
            List<Tick> ticks = new List<Tick>();

            ticks.Add(new Tick { DateTime = BrokerDateTime.Make(DateTime.Now) });

            Assert.IsFalse(ticks.ItemsAreOlderThanSeconds(300));
        }
    }
}
