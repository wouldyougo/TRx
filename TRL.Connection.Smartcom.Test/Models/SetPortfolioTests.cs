using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Models;

namespace TRL.Connect.Smartcom.Test.Models
{
    [TestClass]
    public class SetPortfolioTests
    {
        [TestMethod]
        public void SetPortfolio_Test()
        {
            DateTime now = DateTime.Now;

            SetPortfolio setPortfolio = new SetPortfolio("ST88888-RF-01", 55000, 1, 0, 60000);

            Assert.AreEqual(now.Year, setPortfolio.DateTime.Year);
            Assert.AreEqual(now.Month, setPortfolio.DateTime.Month);
            Assert.AreEqual(now.Day, setPortfolio.DateTime.Day);
            Assert.AreEqual(now.Hour, setPortfolio.DateTime.Hour);
            Assert.AreEqual(now.Minute, setPortfolio.DateTime.Minute);
            Assert.AreEqual(now.Second, setPortfolio.DateTime.Second);

            Assert.AreEqual("ST88888-RF-01", setPortfolio.Portfolio);
            Assert.AreEqual(55000, setPortfolio.Cash);
            Assert.AreEqual(1, setPortfolio.Leverage);
            Assert.AreEqual(0, setPortfolio.Commision);
            Assert.AreEqual(60000, setPortfolio.Saldo);

            Assert.IsInstanceOfType(setPortfolio.Cash, typeof(double));
            Assert.IsInstanceOfType(setPortfolio.Leverage, typeof(double));
            Assert.IsInstanceOfType(setPortfolio.Commision, typeof(double));
            Assert.IsInstanceOfType(setPortfolio.Saldo, typeof(double));
        }
    }
}
