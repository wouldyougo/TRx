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
    public class BuySpreadDataDictionaryFactoryTests
    {
        public OrderBookContext qProvider;
        public List<string> leftLeg, rightLeg;

        [TestInitialize]
        public void Setup()
        {
            this.qProvider = new OrderBookContext();
            this.leftLeg = new List<string>();
            this.rightLeg = new List<string>();
        }

        [TestMethod]
        public void BuySpreadDataDictionary_makes_dictionary()
        {
            this.leftLeg.Add("RTS-12.13_FT");
            this.rightLeg.Add("Si-12.13_FT");
            this.rightLeg.Add("Eu-12.13_FT");

            this.qProvider.Update(0, "RTS-12.13_FT", 0, 0, 143100, 300);
            this.qProvider.Update(0, "Si-12.13_FT", 33200, 100, 0, 0);
            this.qProvider.Update(0, "Eu-12.13_FT", 41000, 200, 0, 0);

            IGenericFactory<IDictionary<string, double>> factory =
                new BuySpreadDataDictionaryFactory(this.qProvider, this.leftLeg, this.rightLeg);

            IDictionary<string, double> result = factory.Make();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(143100, result["RTS-12.13_FT"]);
            Assert.AreEqual(33200, result["Si-12.13_FT"]);
            Assert.AreEqual(41000, result["Eu-12.13_FT"]);
        }

        [TestMethod]
        public void BuySpreadDataDictionary_makes_empty_dictionary()
        {
            this.leftLeg.Add("RTS-12.13_FT");
            this.rightLeg.Add("Si-12.13_FT");
            this.rightLeg.Add("Eu-12.13_FT");

            IGenericFactory<IDictionary<string, double>> factory =
                new BuySpreadDataDictionaryFactory(this.qProvider, this.leftLeg, this.rightLeg);

            IDictionary<string, double> result = factory.Make();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void BuySpreadDataDictionary_makes_without_symbols_makes_empty_dictionary()
        {
            this.qProvider.Update(0, "RTS-12.13_FT", 0, 0, 143100, 300);
            this.qProvider.Update(0, "Si-12.13_FT", 33200, 100, 0, 0);
            this.qProvider.Update(0, "Eu-12.13_FT", 41000, 200, 0, 0);

            IGenericFactory<IDictionary<string, double>> factory =
                new BuySpreadDataDictionaryFactory(this.qProvider, this.leftLeg, this.rightLeg);

            IDictionary<string, double> result = factory.Make();

            Assert.AreEqual(0, result.Count);
        }

    }
}
