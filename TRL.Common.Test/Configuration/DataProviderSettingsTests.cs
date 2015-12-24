using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Configuration;

namespace TRL.Common.Test.Configuration
{
    [TestClass]
    public class DataProviderSettingsTests
    {
        [TestMethod]
        public void Configuration_Make_DataProviderSettings_From_AppConfig()
        {
            DataProviderSettings settings = new DataProviderSettings();

            Assert.IsTrue(settings.ListenPortfolio);
            Assert.IsFalse(settings.ListenQuotes);
            Assert.IsFalse(settings.ListenBidAsk);
            Assert.IsTrue(settings.ListenTicks);
        }
    }
}
