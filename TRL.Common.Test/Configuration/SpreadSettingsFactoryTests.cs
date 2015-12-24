using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Configuration;

namespace TRL.Common.Test.Configuration
{
    [TestClass]
    public class SpreadSettingsFactoryTests
    {
        [TestMethod]
        public void Configuration_SpreadSettingsFactory_returns_model_test()
        {
            string prefix = "SSTR";

            IGenericFactory<SpreadSettings> factory =
                new SpreadSettingsFactory(prefix);

            SpreadSettings sSettings = factory.Make();

            Assert.IsNotNull(sSettings);
            Assert.AreEqual(1.12, sSettings.FairPrice);
            Assert.AreEqual(1.28, sSettings.SellAfterPrice);
            Assert.AreEqual(0.91, sSettings.BuyBeforePrice);
        }

        [TestMethod]
        public void Configuration_SpreadSettingsFactory_returns_null_for_nonexistent_prefix()
        {
            string prefix = "STTB";

            IGenericFactory<SpreadSettings> factory =
                new SpreadSettingsFactory(prefix);

            Assert.IsNull(factory.Make());
        }
    }
}
