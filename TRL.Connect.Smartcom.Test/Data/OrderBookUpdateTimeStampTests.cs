using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Data;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class OrderBookUpdateTimeStampTests
    {        
        [TestMethod]
        public void OrderBookUpdateTimeStamp_is_singleton_test()
        {
            OrderBookLastUpdateTimeStamped first = OrderBookUpdateTimeStamp.Instance;
            OrderBookLastUpdateTimeStamped second = OrderBookUpdateTimeStamp.Instance;
            
            Assert.AreSame(first, second);
            Assert.AreEqual(DateTime.MinValue, first.DateTime);
            Assert.AreEqual(DateTime.MinValue, second.DateTime);
        }
    }
}
