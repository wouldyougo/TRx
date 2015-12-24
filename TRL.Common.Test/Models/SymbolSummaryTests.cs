using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class SymbolSummaryTests
    {
        [TestMethod]
        public void SymbolSummary_constructor_test()
        {
            DateTime lastDealDate = DateTime.Now;

            SymbolSummary sSummary =
                new SymbolSummary("RTS-12.13_FT",
                    lastDealDate,
                    145000,
                    146000,
                    144000,
                    144990,
                    145800,
                    250,
                    50,
                    145790,
                    145780,
                    30,
                    40,
                    1000000,
                    8000,
                    8100,
                    151000,
                    142000,
                    true);

            Assert.IsInstanceOfType(sSummary, typeof(INamed));
            Assert.IsInstanceOfType(sSummary, typeof(IMutable<SymbolSummary>));

            Assert.AreEqual("RTS-12.13_FT", sSummary.Name);
            Assert.AreEqual(lastDealDate, sSummary.LastDealDate);
            Assert.AreEqual(145000, sSummary.Open);
            Assert.AreEqual(146000, sSummary.High);
            Assert.AreEqual(144000, sSummary.Low);
            Assert.AreEqual(144990, sSummary.Close);
            Assert.AreEqual(145800, sSummary.LastDealPrice);
            Assert.AreEqual(250, sSummary.Volume);
            Assert.AreEqual(50, sSummary.LastDealVolume);
            Assert.AreEqual(145790, sSummary.Offer);
            Assert.AreEqual(145780, sSummary.Bid);
            Assert.AreEqual(30, sSummary.OfferSize);
            Assert.AreEqual(40, sSummary.BidSize);
            Assert.AreEqual(1000000, sSummary.OpenVolume);
            Assert.AreEqual(8000, sSummary.BuyWarrantySum);
            Assert.AreEqual(8100, sSummary.SellWarrantySum);
            Assert.AreEqual(151000, sSummary.HighLimitPrice);
            Assert.AreEqual(142000, sSummary.LowLimitPrice);
            Assert.IsTrue(sSummary.IsTraded);
        }

        [TestMethod]
        public void SymbolSummary_update_test()
        {
            DateTime lastDealDate = DateTime.Now;

            SymbolSummary sSummary =
                new SymbolSummary("RTS-12.13_FT",
                    lastDealDate,
                    145000,
                    146000,
                    144000,
                    144990,
                    145800,
                    250,
                    50,
                    145790,
                    145780,
                    30,
                    40,
                    1000000,
                    8000,
                    8100,
                    151000,
                    142000,
                    true);

            sSummary.Update(new SymbolSummary("RTS-12.13_FT",
                    lastDealDate.AddDays(1),
                    145100,
                    146100,
                    144100,
                    144990,
                    145800,
                    250,
                    50,
                    145790,
                    145780,
                    30,
                    40,
                    1000000,
                    8000,
                    8100,
                    151000,
                    142000,
                    true));

            Assert.IsInstanceOfType(sSummary, typeof(INamed));
            Assert.IsInstanceOfType(sSummary, typeof(IMutable<SymbolSummary>));

            Assert.AreEqual("RTS-12.13_FT", sSummary.Name);
            Assert.AreEqual(lastDealDate.AddDays(1), sSummary.LastDealDate);
            Assert.AreEqual(145100, sSummary.Open);
            Assert.AreEqual(146100, sSummary.High);
            Assert.AreEqual(144100, sSummary.Low);
            Assert.AreEqual(144990, sSummary.Close);
            Assert.AreEqual(145800, sSummary.LastDealPrice);
            Assert.AreEqual(250, sSummary.Volume);
            Assert.AreEqual(50, sSummary.LastDealVolume);
            Assert.AreEqual(145790, sSummary.Offer);
            Assert.AreEqual(145780, sSummary.Bid);
            Assert.AreEqual(30, sSummary.OfferSize);
            Assert.AreEqual(40, sSummary.BidSize);
            Assert.AreEqual(1000000, sSummary.OpenVolume);
            Assert.AreEqual(8000, sSummary.BuyWarrantySum);
            Assert.AreEqual(8100, sSummary.SellWarrantySum);
            Assert.AreEqual(151000, sSummary.HighLimitPrice);
            Assert.AreEqual(142000, sSummary.LowLimitPrice);
            Assert.IsTrue(sSummary.IsTraded);
        }

        [TestMethod]
        public void SymbolSummary_ignore_update_test()
        {
            DateTime lastDealDate = DateTime.Now;

            SymbolSummary sSummary =
                new SymbolSummary("RTS-12.13_FT",
                    lastDealDate,
                    145000,
                    146000,
                    144000,
                    144990,
                    145800,
                    250,
                    50,
                    145790,
                    145780,
                    30,
                    40,
                    1000000,
                    8000,
                    8100,
                    151000,
                    142000,
                    true);

            sSummary.Update(new SymbolSummary("RTS-3.14_FT",
                    lastDealDate.AddDays(1),
                    145100,
                    146100,
                    144100,
                    144990,
                    145800,
                    250,
                    50,
                    145790,
                    145780,
                    30,
                    40,
                    1000000,
                    8000,
                    8100,
                    151000,
                    142000,
                    true));

            Assert.IsInstanceOfType(sSummary, typeof(INamed));
            Assert.IsInstanceOfType(sSummary, typeof(IMutable<SymbolSummary>));

            Assert.AreEqual("RTS-12.13_FT", sSummary.Name);
            Assert.AreEqual(lastDealDate, sSummary.LastDealDate);
            Assert.AreEqual(145000, sSummary.Open);
            Assert.AreEqual(146000, sSummary.High);
            Assert.AreEqual(144000, sSummary.Low);
            Assert.AreEqual(144990, sSummary.Close);
            Assert.AreEqual(145800, sSummary.LastDealPrice);
            Assert.AreEqual(250, sSummary.Volume);
            Assert.AreEqual(50, sSummary.LastDealVolume);
            Assert.AreEqual(145790, sSummary.Offer);
            Assert.AreEqual(145780, sSummary.Bid);
            Assert.AreEqual(30, sSummary.OfferSize);
            Assert.AreEqual(40, sSummary.BidSize);
            Assert.AreEqual(1000000, sSummary.OpenVolume);
            Assert.AreEqual(8000, sSummary.BuyWarrantySum);
            Assert.AreEqual(8100, sSummary.SellWarrantySum);
            Assert.AreEqual(151000, sSummary.HighLimitPrice);
            Assert.AreEqual(142000, sSummary.LowLimitPrice);
            Assert.IsTrue(sSummary.IsTraded);
        }

        [TestMethod]
        public void SymbolSummary_ToString_test()
        {
            DateTime lastDealDate = DateTime.Now;

            SymbolSummary sSummary =
                new SymbolSummary("RTS-12.13_FT",
                    lastDealDate,
                    145000,
                    146000,
                    144000,
                    144990,
                    145800,
                    250,
                    50,
                    145790,
                    145780,
                    30,
                    40,
                    1000000,
                    8000,
                    8100,
                    151000,
                    142000,
                    true);

            string result = String.Format("SymbolSummary: {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}",
                sSummary.Name,
                sSummary.LastDealDate,
                sSummary.Open,
                sSummary.High,
                sSummary.Low,
                sSummary.Close,
                sSummary.LastDealPrice,
                sSummary.Volume,
                sSummary.Offer,
                sSummary.Bid,
                sSummary.OfferSize,
                sSummary.BidSize,
                sSummary.OpenVolume,
                sSummary.BuyWarrantySum,
                sSummary.SellWarrantySum,
                sSummary.HighLimitPrice,
                sSummary.LowLimitPrice,
                sSummary.IsTraded);

            Assert.AreEqual(result, sSummary.ToString());
        }
    }
}
