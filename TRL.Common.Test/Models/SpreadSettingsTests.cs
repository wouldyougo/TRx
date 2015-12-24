using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class SpreadSettingsTests
    {
        [TestMethod]
        public void SpreadSettings_constructor_test()
        {
            double fairPrice = 1.3589;
            double sellAfter = 1.4358;
            double buyBefore = 1.1355;

            SpreadSettings spreadSettings =
                new SpreadSettings(fairPrice, sellAfter, buyBefore);

            Assert.AreEqual(fairPrice, spreadSettings.FairPrice);
            Assert.AreEqual(sellAfter, spreadSettings.SellAfterPrice);
            Assert.AreEqual(buyBefore, spreadSettings.BuyBeforePrice);
        }
    }
}
