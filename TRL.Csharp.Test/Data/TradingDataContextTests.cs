using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Csharp.Collections;
using TRL.Csharp.Models;
using TRL.Csharp.Data;
using System.Collections.Generic;

namespace TRL.Csharp.Test.Data
{
    [TestClass]
    public class TradingDataContextTests
    {
        private DataContext tradingDataContext;

        [TestInitialize]
        public void Setup()
        {
            this.tradingDataContext = new TradingDataContext();
        }
    
        [TestMethod]
        public void TradingDataContext_contains_HashSet_of_Trades_test()
        {
            Assert.IsNotNull(tradingDataContext.Get<GenericObservableHashSet<Trade>>());
            Assert.IsNotNull(tradingDataContext.Get<ICollection<Trade>>());
        }

        [TestMethod]
        public void TradingDataContext_contains_HashSet_of_Orders_test()
        {
            Assert.IsNotNull(tradingDataContext.Get<GenericObservableHashSet<Order>>());
        }

        [TestMethod]
        public void TradingDataContext_contains_List_of_Bars_test()
        {
            Assert.IsNotNull(tradingDataContext.Get<GenericObservableList<Bar>>());
        }
    }
}
