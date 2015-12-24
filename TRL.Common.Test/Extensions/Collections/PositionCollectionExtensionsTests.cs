using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
//using TRL.Common.Extensions.Data;
//using TRL.Common.Extensions.Models;
using TRL.Common.Extensions.Collections;

namespace TRL.Common.Extensions.Collections.Test
{
    [TestClass]
    public class PositionCollectionExtensionsTests
    {
        private IDataContext tradingData;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
        }

        [TestMethod]
        public void GetAmount_when_no_positions_exists()
        {
            Assert.AreEqual(0, this.tradingData.Get<ICollection<Position>>().GetAmount("BP12345-RF-01", "RTS-9.13_FT"));
        }

        [TestMethod]
        public void no_amount_for_symbol()
        {
            Assert.AreEqual(0, this.tradingData.Get<ICollection<Position>>().GetAmount("RTS-9.13_FT"));
        }

        [TestMethod]
        public void get_long_position_amount_for_symbol()
        {
            this.tradingData.Get<ICollection<Position>>().Add(new Position(1, "BP12345-RF-01", "RTS-9.13_FT", 10));

            Assert.AreEqual(10, this.tradingData.Get<ICollection<Position>>().GetAmount("RTS-9.13_FT"));
        }

        [TestMethod]
        public void get_short_position_amount_for_symbol()
        {
            this.tradingData.Get<ICollection<Position>>().Add(new Position(1, "BP12345-RF-01", "RTS-9.13_FT", -10));

            Assert.AreEqual(-10, this.tradingData.Get<ICollection<Position>>().GetAmount("RTS-9.13_FT"));
        }

        [TestMethod]
        public void Position_not_exists()
        {
            Assert.IsFalse(this.tradingData.Get<ICollection<Position>>().Exists("BP12345-RF-01", "RTS-9.13_FT"));
        }

        [TestMethod]
        public void Position_exists()
        {
            this.tradingData.Get<ICollection<Position>>().Add(new Position(1, "BP12345-RF-01", "RTS-9.13_FT", 0));
            Assert.IsTrue(this.tradingData.Get<ICollection<Position>>().Exists("BP12345-RF-01", "RTS-9.13_FT"));
        }
    }
}
