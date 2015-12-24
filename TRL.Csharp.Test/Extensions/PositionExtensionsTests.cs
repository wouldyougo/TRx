using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Csharp.Test.Utility;
using TRL.Csharp.Collections;
using TRL.Csharp.Models;
using TRL.Csharp.Extensions;

namespace TRL.Csharp.Test.Extensions
{
    [TestClass]
    public class PositionExtensionsTests
    {
        private GenericObservableList<Trade> tradeList;

        [TestInitialize]
        public void Setup()
        {
            tradeList = new GenericObservableList<Trade>();
        }
        [TestMethod]
        public void GetPositionAmount_test()
        {
            tradeList.FillListWithEvenBuyAndOddSale(11);
            Assert.AreEqual(1, tradeList.GetPositionAmount());
        }
    }
}
