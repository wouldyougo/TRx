using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class PositionEqualityComparerTests
    {
        [TestMethod]
        public void Positions_Are_Equals()
        {
            EqualityComparer<Position> pec = new PositionEqualityComparer();

            Position one = new Position("ST88888-RF-01", "RTS-9.13_FT", 0);
            Position two = new Position("ST88888-RF-01", "RTS-9.13_FT", 0);

            Assert.IsTrue(pec.Equals(one, two));
        }

        [TestMethod]
        public void Positions_Are_Not_Equals()
        {
            EqualityComparer<Position> pec = new PositionEqualityComparer();

            Position one = new Position("ST88888-RF-01", "RTS-9.13_FT", 0);
            Position two = new Position("ST88888-RF-02", "RTS-9.13_FT", 0);

            Assert.IsFalse(pec.Equals(one, two));
        }

        [TestMethod]
        public void Positions_With_Same_Portfolio_Name_Are_Equals()
        {
            EqualityComparer<Position> pec = new PositionEqualityComparer();

            HashSet<Position> positions = new HashSet<Position>(pec);

            Position one = new Position("ST88888-RF-01", "RTS-9.13_FT", 0);
            Position two = new Position("ST88888-RF-01", "RTS-9.13_FT", 0);

            positions.Add(one);
            Assert.AreEqual(1, positions.Count);

            positions.Add(two);
            Assert.AreEqual(1, positions.Count);
        }

        [TestMethod]
        public void Positions_With_Different_Portfolio_Name_Are_Not_Equals()
        {
            EqualityComparer<Position> pec = new PositionEqualityComparer();

            HashSet<Position> positions = new HashSet<Position>(pec);

            Position one = new Position("ST88888-RF-01", "RTS-9.13_FT", 0);
            Position two = new Position("ST88888-RF-02", "RTS-9.13_FT", 0);

            positions.Add(one);
            Assert.AreEqual(1, positions.Count);

            positions.Add(two);
            Assert.AreEqual(2, positions.Count);
        }
    }
}
