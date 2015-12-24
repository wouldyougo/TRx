using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Data.Spreads;

namespace TRL.Common.Test.Data.Spreads
{
    [TestClass]
    public class SellSpreadFactoryTests
    {
        private OrderBookContext orderBook;
        private List<StrategyHeader> leftLeg, rightLeg;

        [TestInitialize]
        public void Setup()
        {
            this.orderBook = new OrderBookContext(50);
            this.leftLeg = new List<StrategyHeader>();
            this.leftLeg.Add(new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-12.13_FT", 1));

            this.rightLeg = new List<StrategyHeader>();
            this.rightLeg.Add(new StrategyHeader(2, "Strategy", "BP12345-RF-01", "Si-12.13_FT", 2));
            this.rightLeg.Add(new StrategyHeader(3, "Strategy", "BP12345-RF-01", "Eu-12.13_FT", 1));
        }

        [TestMethod]
        public void SellSpreadFactory_returns_spread()
        {
            this.orderBook.Update(0, "RTS-12.13_FT", 140000, 100, 140010, 200);
            this.orderBook.Update(0, "Si-12.13_FT", 32000, 85, 32001, 25);
            this.orderBook.Update(0, "Eu-12.13_FT", 43800, 200, 43805, 210);

            IGenericFactory<double> factory =
                new SellSpreadFactory(this.leftLeg, this.rightLeg, this.orderBook);

            Assert.AreEqual(1.2986, factory.Make());
        }

        [TestMethod]
        public void SellSpreadFactory_returns_zero()
        {
            this.orderBook.Update(0, "Si-12.13_FT", 32000, 85, 32001, 25);
            this.orderBook.Update(0, "Eu-12.13_FT", 43800, 200, 43805, 210);

            IGenericFactory<double> factory =
                new SellSpreadFactory(this.leftLeg, this.rightLeg, this.orderBook);

            Assert.AreEqual(0, factory.Make());
        }
    }
}
