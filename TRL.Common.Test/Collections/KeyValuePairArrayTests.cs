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
    public class KeyValuePairArrayTests
    {
        [TestMethod]
        public void Collections_KeyValuePairArray_test()
        {
            KeyValuePairArray<double, double> array = new KeyValuePairArray<double, double>(50);

            Assert.AreEqual(50, array.Length);

            for (int i = 0; i < 50; i++)
            {
                Assert.AreEqual(0, array[i].Key);
                Assert.AreEqual(0, array[i].Value);
            }
        }

        [TestMethod]
        public void Collections_KeyValuePairArray_change_element_with_indexer()
        {
            KeyValuePairArray<double, double> array = new KeyValuePairArray<double, double>(50);

            array[0] = new KeyValuePair<double, double>(145000, 100);

            Assert.AreEqual(145000, array[0].Key);
            Assert.AreEqual(100, array[0].Value);
        }

        [TestMethod]
        public void Collections_KeyValuePairArray_change_element_with_update_method()
        {
            KeyValuePairArray<double, double> array = new KeyValuePairArray<double, double>(50);

            array.Update(0, 145000, 100);
            Assert.AreEqual(145000, array[0].Key);
            Assert.AreEqual(100, array[0].Value);

            array.Update(10, 146000, 200);
            Assert.AreEqual(146000, array[10].Key);
            Assert.AreEqual(200, array[10].Value);
        }

        [TestMethod]
        public void Collections_KeyValuePairArray_do_nothing_when_position_more_than_length()
        {
            KeyValuePairArray<double, double> array = new KeyValuePairArray<double, double>(50);

            array[100] = new KeyValuePair<double, double>(150000, 250);
            array.Update(110, 145000, 100);

            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(0, array[i].Key);
                Assert.AreEqual(0, array[i].Value);
            }
        }
    }
}
