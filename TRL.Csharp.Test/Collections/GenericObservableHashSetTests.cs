using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Csharp.Models;
using System.Collections.Generic;
using TRL.Csharp.Collections;

namespace TRL.Csharp.Test.Collections
{
    [TestClass]
    public class GenericObservableHashSetTests
    {
        private GenericObservableHashSet<Trade> tradeSet;

        [TestInitialize]
        public void Setup()
        {
            tradeSet = new GenericObservableHashSet<Trade>(new IdentifiedEqualityComparer());
            tradeSet.OnItemAdded += Update_LastTradeId;
        }

        [TestMethod]
        public void dont_add_trade_duplicate_into_set_test()
        {
            Trade first = new Trade(1, "ST12345-RF-01", "RTS-9.14", DateTime.Now, 125000, 10);
            Trade second = new Trade(1, "ST12345-RF-01", "RTS-9.14", DateTime.Now, 125000, 10);

            tradeSet.Add(first);
            tradeSet.Add(second);
            Assert.AreEqual(1, tradeSet.Count);
        }

        private int LastTradeId;

        public void Update_LastTradeId(Trade item)
        {
            LastTradeId = item.Id;
        }
        [TestMethod]
        public void raise_an_event_on_trade_added_test()
        {
            Trade first = new Trade(55, "ST12345-RF-01", "RTS-9.14", DateTime.Now, 125000, 10);
            tradeSet.Add(first);
            Assert.AreEqual(first.Id, LastTradeId);
        }
    }
}
