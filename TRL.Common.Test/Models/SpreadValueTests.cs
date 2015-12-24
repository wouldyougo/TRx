using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using System.Globalization;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class SpreadValueTests
    {
        [TestMethod]
        public void SpreadValue_constructor_test()
        {
            int id = 1;
            DateTime dt = new DateTime(2199, 12,1);
            double sellAfterPrice = 1.3385;
            double buyBeforePrice = 1.3385;

            SpreadValue sv = new SpreadValue(id, dt, sellAfterPrice, buyBeforePrice);

            Assert.AreEqual(id, sv.Id);
            Assert.AreEqual(dt, sv.DateTime);
            Assert.AreEqual(sellAfterPrice, sv.SellAfterPrice);
            Assert.AreEqual(buyBeforePrice, sv.BuyBeforePrice);
        }

        [TestMethod]
        public void SpreadValue_ToString_test()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            DateTime date = DateTime.Now;

            SpreadValue sp = new SpreadValue(1, date, 1.255, 1.188);

            string expected = String.Format("SpreadValue: {0},{1:dd/MM/yyyy H:mm:ss.fff},{2},{3}",
                sp.Id,
                sp.DateTime,
                sp.SellAfterPrice.ToString("0.0000", ci),
                sp.BuyBeforePrice.ToString("0.0000", ci));

            Assert.AreEqual(expected, sp.ToString());
        }
    }
}
