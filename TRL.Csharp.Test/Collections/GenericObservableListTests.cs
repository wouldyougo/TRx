using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Csharp.Collections;
using TRL.Csharp.Models;
using System.Collections.Generic;

namespace TRL.Csharp.Test.Collections
{
    [TestClass]
    public class GenericObservableListTests
    {
        private string LastTradedSymbol;
        private GenericObservableList<Trade> tradeList;
        private int MethodLaunchCounter;

        private void UpdateLastTradedSymbol(Trade trade)
        {
            LastTradedSymbol = trade.Symbol;
            MethodLaunchCounter++;
        }

        [TestInitialize]
        public void Setup()
        {
            tradeList = new GenericObservableList<Trade>();
            tradeList.OnItemAdded += UpdateLastTradedSymbol;
        }

        [TestMethod]
        public void dont_throw_an_exception_when_event_is_not_assigned_test()
        {
            tradeList.OnItemAdded -= UpdateLastTradedSymbol;
            tradeList.Add(new Trade(DateTime.Now, 10, 11));
        }

        [TestMethod]
        public void link_an_event_twice_test()
        {
            tradeList.OnItemAdded += UpdateLastTradedSymbol;
            tradeList.Add(new Trade(DateTime.Now, 10, 11));

            Assert.AreEqual(2, MethodLaunchCounter);

        }

        [TestMethod]
        public void tradeList_is_List_of_Trade_test()
        {
            Assert.IsInstanceOfType(tradeList, typeof(List<Trade>));
        }


        [TestMethod]
        public void get_Count_of_tradeList_test()
        {
            for (int i = 0; i < 10; i++)
            {
                tradeList.Add(new Trade("ST12345-RF-01", "RTS-9.14", DateTime.Now, 125000, 5));
            }

            Assert.AreEqual(10, tradeList.Count);
        }

        [TestMethod]
        public void notify_on_Trade_added_to_tradeList_test()
        {
            Trade trade = new Trade("ST12345-RF-01", "RTS-9.14", DateTime.Now, 125000, 5);

            tradeList.Add(trade);
            Assert.AreEqual(trade.Symbol, LastTradedSymbol);
        }

        [TestMethod]
        public void remove_Trade_from_tradeList_test()
        {
            Trade trade = new Trade(DateTime.Now, 25.05, 10);

            tradeList.Add(trade);

            Assert.AreEqual(1, tradeList.Count);

            tradeList.Remove(trade);

            Assert.AreEqual(0, tradeList.Count);
        }


    }
}
