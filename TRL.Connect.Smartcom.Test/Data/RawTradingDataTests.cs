using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Data;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class RawTradingDataTests
    {
        [TestMethod]
        public void RawTradingData_Is_Singleton()
        {
            RawTradingData d = RawTradingData.Instance;
            RawTradingData d2 = RawTradingData.Instance;

            Assert.AreSame(d, d2);
        }

        [TestMethod]
        public void RawTradingData_Inherits_RawTradingDataContext()
        {
            Assert.IsInstanceOfType(RawTradingData.Instance, typeof(RawTradingDataContext));
        }

        [TestMethod]
        public void MarketData_Is_DataContext()
        {
            Assert.IsTrue(RawTradingData.Instance is BaseDataContext);
        }
    }
}
