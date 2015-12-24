using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Csharp.Models;
using System.Collections.Generic;
using TRL.Csharp.Collections;

namespace TRL.Csharp.Test.Collections
{
    [TestClass]
    public class ListTests
    {
        private List<Trade> trades;

        [TestInitialize]
        public void Setup()
        {
            trades = new List<Trade>();
            Assert.AreEqual(0, trades.Count);
        }

        [TestMethod]
        public void Trade_add_into_collection_test()
        {
            Trade trade = new Trade(DateTime.Now, 25.05, 10);

            trades.Add(trade);

            Assert.AreEqual(1, trades.Count);
        }

        [TestMethod]
        public void delete_Trade_from_collection_test()
        {
            Trade trade = new Trade(DateTime.Now, 25.05, 10);

            trades.Add(trade);

            Assert.AreEqual(1, trades.Count);

            trades.Remove(trade);

            Assert.AreEqual(0, trades.Count);
        }

        [TestMethod]
        public void try_to_delete_nonexistent_Trade_from_collection_test()
        {
            Trade first = new Trade(DateTime.Now, 25.05, 10);

            trades.Add(first);
            Assert.AreEqual(1, trades.Count);

            Trade second = new Trade(DateTime.Now, 25.05, 10);

            trades.Remove(second);
            Assert.AreEqual(1, trades.Count);
        }

        [TestMethod]
        public void collection_contains_Trade_test()
        {
            Trade first = new Trade(DateTime.Now, 25.05, 10);
            trades.Add(first);
            Trade second = new Trade(DateTime.Now, 25.05, 10);
            trades.Add(second);

            Assert.AreEqual(2, trades.Count);
            Assert.IsTrue(trades.Contains(second));

        }

        [TestMethod]
        public void clear_collection_test()
        {
            int count = 15;
            FillCollectionWithTrades(count);
            Assert.AreEqual(15, trades.Count);

            trades.Clear();
            Assert.AreEqual(0, trades.Count);
        }

        private void FillCollectionWithTrades(int count)
        {
            for (int i = 0; i < count; i++) 
            { 
                trades.Add(new Trade(new DateTime(2014, 5, i + 1), i, i));
            }
        }

        [TestMethod]
        public void find_Trade_by_price_test()
        {
            FillCollectionWithTrades(10);

            Trade trade = trades.SingleOrDefault(t => t.Price == 5);

            Assert.IsNotNull(trade);
            Assert.AreEqual(5, trade.Price);

        }

        [TestMethod]
        public void find_Trade_by_amount_test()
        {
            FillCollectionWithTrades(10);

            Trade trade = trades.SingleOrDefault(t => t.Amount == 5);

            Assert.IsNotNull(trade);
            Assert.AreEqual(5, trade.Amount);

        }

        [TestMethod]
        public void find_nonexistent_Trade_test()
        {
            FillCollectionWithTrades(10);

            Trade trade = trades.SingleOrDefault(t => t.Amount == 35);
            Assert.IsNull(trade);
        }

        [TestMethod]
        public void find_Trades_test()
        {
            FillCollectionWithTrades(20);

            DateTime date = new DateTime(2014, 5, 5);

            IEnumerable<Trade> tradeSet = trades.Where(t => t.DateTime > date);

            Assert.IsNotNull(tradeSet);
            Assert.AreEqual(15, tradeSet.Count());

            foreach (Trade item in tradeSet)
                Assert.IsTrue(item.DateTime > date);
        }

        [TestMethod]
        public void find_nonexistent_Trades_test()
        {
            FillCollectionWithTrades(20);

            DateTime date = new DateTime(2014, 6, 5);

            IEnumerable<Trade> tradeSet = trades.Where(t => t.DateTime > date);

            Assert.AreEqual(0, tradeSet.Count());
        }

        [TestMethod]
        public void get_last_Trade_from_collection_test()
        {
            FillCollectionWithTrades(20);

            Trade trade = trades.Last();
            Assert.AreEqual(new DateTime(2014, 5, 20), trade.DateTime);
            Assert.AreEqual(19, trade.Price);
            Assert.AreEqual(19, trade.Amount);
        }

        [TestMethod]
        public void get_first_Trade_from_collection_test()
        {
            FillCollectionWithTrades(20);

            Trade trade = trades.First();
            Assert.AreEqual(new DateTime(2014, 5, 1), trade.DateTime);
            Assert.AreEqual(0, trade.Price);
            Assert.AreEqual(0, trade.Amount);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void SingleOrDefault_when_multiple_matches_exists_test()
        {
            FillCollectionWithTrades(10);

            Trade trade = trades.SingleOrDefault(t => t.Symbol == String.Empty);
        }

    }
}
