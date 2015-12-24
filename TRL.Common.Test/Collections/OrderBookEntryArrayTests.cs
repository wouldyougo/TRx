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
    public class OrderBookEntryArrayTests
    {
        [TestMethod]
        public void Collections_OrderEntryArray_test()
        {
            OrderBookEntryArray array = new OrderBookEntryArray(50);

            Assert.AreEqual(50, array.Length);

            for (int i = 0; i < 50; i++)
            {
                Assert.AreEqual(0, array[i].Price);
                Assert.AreEqual(0, array[i].Volume);
            }
        }

        [TestMethod]
        public void Collections_OrderEntryArray_change_element_with_update_method()
        {
            OrderBookEntryArray array = new OrderBookEntryArray(50);

            array.Update(0, 145000, 100);
            Assert.AreEqual(145000, array[0].Price);
            Assert.AreEqual(100, array[0].Volume);

            array.Update(10, 146000, 200);
            Assert.AreEqual(146000, array[10].Price);
            Assert.AreEqual(200, array[10].Volume);
        }

        [TestMethod]
        public void Collections_OrderEntryArray_do_nothing_when_position_more_than_length()
        {
            OrderBookEntryArray array = new OrderBookEntryArray(50);

            array.Update(110, 145000, 100);

            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(0, array[i].Price);
                Assert.AreEqual(0, array[i].Volume);
            }
        }
    }
}
