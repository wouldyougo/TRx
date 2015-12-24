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
    public class PositionTests
    {
        [TestMethod]
        public void Position_constructor_test()
        {
            Position p = new Position("BP12345-RF-01", "RTS-9.13_FT", 0);

            Assert.IsTrue(p.Id > 0);
            Assert.AreEqual("BP12345-RF-01", p.Portfolio);
            Assert.AreEqual("RTS-9.13_FT", p.Symbol);
            Assert.AreEqual(0D, p.Amount);
        }

        [TestMethod]
        public void Position_ToString()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            Position position = new Position ("ST30151-MS-01", "RTS-12.12_FT", 1);

            string result = String.Format("Position Id: {0}, Portfolio: {1}, Symbol: {2}, Amount: {3}",
                position.Id, position.Portfolio, position.Symbol, position.Amount.ToString("0.0000", ci));

            Assert.AreEqual(result, position.ToString());
        }

        [TestMethod]
        public void Position_ToImportString()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            Position position = new Position("ST30151-MS-01", "RTS-12.12_FT", 1);

            string result = String.Format("{0},{1},{2},{3}",
                position.Id, position.Portfolio, position.Symbol, position.Amount.ToString("0.0000", ci));

            Assert.AreEqual(result, position.ToImportString());
        }

        [TestMethod]
        public void Position_Parse()
        {
            string src = "1, ST30151-MS-01, RTS-12.12_FT, 3";

            Position position = Position.Parse(src);

            Assert.AreEqual(1, position.Id);
            Assert.AreEqual("ST30151-MS-01", position.Portfolio);
            Assert.AreEqual("RTS-12.12_FT", position.Symbol);
            Assert.AreEqual(3, position.Amount);
        }

        [TestMethod]
        public void Position_IsLong()
        {
            Position position = new Position(1, "BP12345-RF-01", "RTS-9.13_FT", 10);

            Assert.IsTrue(position.IsLong);
            Assert.IsFalse(position.IsShort);
        }

        [TestMethod]
        public void Position_IsShort()
        {
            Position position = new Position(10, "BP12345-RF-01", "RTS-9.13_FT", -8);

            Assert.IsTrue(position.IsShort);
            Assert.IsFalse(position.IsLong);
        }

    }
}
