using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Data;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class TickDataFeedUpdateTimeStampTests
    {        
        [TestMethod]
        public void TickDataFeedUpdateTimeStamp_is_singleton_test()
        {
            ItemAddedLastTimeStamped<Tick> first = TickDataFeedUpdateTimeStamp.Instance;
            ItemAddedLastTimeStamped<Tick> second = TickDataFeedUpdateTimeStamp.Instance;
            
            Assert.AreSame(first, second);
            Assert.AreEqual(DateTime.MinValue, first.DateTime);
            Assert.AreEqual(DateTime.MinValue, second.DateTime);
        }
    }
}
