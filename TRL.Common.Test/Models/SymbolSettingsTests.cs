using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class SymbolSettingsTests
    {
        [TestMethod]
        public void SymbolSettings_constructor_test()
        {
            SymbolSettings ss =
                new SymbolSettings("RTS-12.13_FT",
                    "RIZ3",
                    "Фьючерсный контракт на Индекс РТС",
                    "Фьючерс",
                    5,
                    1,
                    6.37572,
                    10,
                    new DateTime(2013, 12, 16),
                    35,
                    149020);

            Assert.IsInstanceOfType(ss, typeof(INamed));
            Assert.IsInstanceOfType(ss, typeof(IMutable<SymbolSettings>));
            Assert.AreEqual("RTS-12.13_FT", ss.Name);
            Assert.AreEqual("RIZ3", ss.ShortName);
            Assert.AreEqual("Фьючерсный контракт на Индекс РТС", ss.Description);
            Assert.AreEqual("Фьючерс", ss.Type);
            Assert.AreEqual(5, ss.PricePrecision);
            Assert.AreEqual(1, ss.LotSize);
            Assert.AreEqual(6.37572, ss.MinStepPrice);
            Assert.AreEqual(10, ss.MinPriceStep);
            Assert.AreEqual(new DateTime(2013, 12, 16), ss.ExpirationDate);
            Assert.AreEqual(35, ss.DaysBeforeExpiration);
            Assert.AreEqual(149020, ss.LastPrice);
        }

        [TestMethod]
        public void SymbolSettings_Update_test()
        {
            SymbolSettings ss =
                new SymbolSettings("RTS-12.13_FT",
                    "RIZ3",
                    "Фьючерсный контракт на Индекс РТС",
                    "Фьючерс",
                    5,
                    1,
                    6.37572,
                    10,
                    new DateTime(2013, 12, 16),
                    35,
                    149020);

            ss.Update(new SymbolSettings("RTS-12.13_FT",
                    "RIZ3",
                    "Фьючерсный контракт на Индекс РТС",
                    "Фьючерс",
                    5,
                    1,
                    6.38358,
                    10,
                    new DateTime(2013, 12, 16),
                    34,
                    150050));

            Assert.AreEqual("RTS-12.13_FT", ss.Name);
            Assert.AreEqual("RIZ3", ss.ShortName);
            Assert.AreEqual("Фьючерсный контракт на Индекс РТС", ss.Description);
            Assert.AreEqual("Фьючерс", ss.Type);
            Assert.AreEqual(5, ss.PricePrecision);
            Assert.AreEqual(1, ss.LotSize);
            Assert.AreEqual(6.38358, ss.MinStepPrice);
            Assert.AreEqual(10, ss.MinPriceStep);
            Assert.AreEqual(new DateTime(2013, 12, 16), ss.ExpirationDate);
            Assert.AreEqual(34, ss.DaysBeforeExpiration);
            Assert.AreEqual(150050, ss.LastPrice);
        }

        [TestMethod]
        public void SymbolSettings_ignore_Update_test()
        {
            SymbolSettings ss =
                new SymbolSettings("RTS-12.13_FT",
                    "RIZ3",
                    "Фьючерсный контракт на Индекс РТС",
                    "Фьючерс",
                    5,
                    1,
                    6.37572,
                    10,
                    new DateTime(2013, 12, 16),
                    35,
                    149020);

            ss.Update(new SymbolSettings("Si-12.13_FT",
                    "SiZ3",
                    "Фьючерсный контракт на курс доллар США - российский рубль",
                    "Фьючерс",
                    5,
                    1000,
                    1,
                    1,
                    new DateTime(2013, 12, 16),
                    35,
                    32149));

            Assert.AreEqual("RTS-12.13_FT", ss.Name);
            Assert.AreEqual("RIZ3", ss.ShortName);
            Assert.AreEqual("Фьючерсный контракт на Индекс РТС", ss.Description);
            Assert.AreEqual("Фьючерс", ss.Type);
            Assert.AreEqual(5, ss.PricePrecision);
            Assert.AreEqual(1, ss.LotSize);
            Assert.AreEqual(6.37572, ss.MinStepPrice);
            Assert.AreEqual(10, ss.MinPriceStep);
            Assert.AreEqual(new DateTime(2013, 12, 16), ss.ExpirationDate);
            Assert.AreEqual(35, ss.DaysBeforeExpiration);
            Assert.AreEqual(149020, ss.LastPrice);
        }

        [TestMethod]
        public void SymbolSettings_ToString_test()
        {
            DateTime expirationDate = new DateTime(2013, 12, 16);

            SymbolSettings ss =
                new SymbolSettings("RTS-12.13_FT",
                    "RIZ3",
                    "Фьючерсный контракт на Индекс РТС",
                    "Фьючерс",
                    5,
                    1,
                    6.37572,
                    10,
                    expirationDate,
                    35,
                    149020);

            string result = String.Format("SymbolSettings: {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                ss.Name,
                ss.ShortName,
                ss.Description,
                ss.Type,
                ss.PricePrecision,
                ss.LotSize,
                ss.MinStepPrice,
                ss.MinPriceStep,
                ss.ExpirationDate,
                ss.DaysBeforeExpiration,
                ss.LastPrice);

            Assert.AreEqual(result, ss.ToString());
        }
    }
}
