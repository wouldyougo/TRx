using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Csharp.Collections;
using TRL.Csharp.Models;
using TRL.Csharp.Extensions;
using TRL.Csharp.Test.Utility;

namespace TRL.Csharp.Test.Extensions
{
    [TestClass]
    public class ProfitAndLossExtensionsTests
    {
        private GenericObservableList<Trade> tradeList;

        [TestInitialize]
        public void Setup() 
        {
            tradeList = new GenericObservableList<Trade>();
        }

        [TestMethod]
        public void GetProfitAndLossPoints_test()
        {
            tradeList.FillListWithEvenBuyAndOddSale(10);
            Assert.AreEqual(5, tradeList.GetProfitAndLossPoints());
        }

    }
}
