using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Collections;

namespace TRL.Common.Test.Collections
{
    [TestClass]
    public class SymbolOrderBookTests
    {
        [TestMethod]
        public void Collections_SymbolOrderBook_constructor_test()
        {
            int depth = 20;

            SymbolOrderBook book = new SymbolOrderBook("RTS-12.13_FT", depth);

            Assert.AreEqual(0, book.GetOfferPrice(0));
            Assert.AreEqual(0, book.GetBidPrice(0));
            Assert.AreEqual(0, book.GetOfferVolume(0));
            Assert.AreEqual(0, book.GetBidVolume(0));
            Assert.AreEqual(20, book.Depth);
            Assert.IsInstanceOfType(book, typeof(INamed));
            Assert.AreEqual("RTS-12.13_FT", book.Name);
        }

        [TestMethod]
        public void Collections_SymbolOrderBook_update_test()
        {
            SymbolOrderBook book = new SymbolOrderBook("RTS-12.13_FT", 5);

            book.Update(0, 145000, 100, 145010, 50);

            Assert.AreEqual(145000, book.GetBidPrice(0));
            Assert.AreEqual(100, book.GetBidVolume(0));
            Assert.AreEqual(145010, book.GetOfferPrice(0));
            Assert.AreEqual(50, book.GetOfferVolume(0));
        }
    }
}
