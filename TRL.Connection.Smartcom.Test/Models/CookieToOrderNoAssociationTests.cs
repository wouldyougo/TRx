using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Models;

namespace TRL.Connect.Smartcom.Test.Models
{
    [TestClass]
    public class CookieToOrderNoAssociationTests
    {
        [TestMethod]
        public void CookieToOrderNoAssociation_constructor_test()
        {
            int cookie = 150;
            string orderNo = "12345678";
            CookieToOrderNoAssociation item = new CookieToOrderNoAssociation(cookie, orderNo);

            Assert.AreEqual(cookie, item.Cookie);
            Assert.AreEqual(orderNo, item.OrderNo);
        }

        [TestMethod]
        public void CookieToOrderNoAssociation_ToString_test()
        {
            int cookie = 150;
            string orderNo = "12345678";
            CookieToOrderNoAssociation item = new CookieToOrderNoAssociation(cookie, orderNo);

            string expected = String.Format("{0}, {1}", cookie, orderNo);
            Assert.AreEqual(expected, item.ToString());
        }
    }
}
