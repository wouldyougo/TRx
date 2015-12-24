using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;

namespace TRL.Common.Test.Data
{
    public class QuotesStorageObserver
    {
        private int counter;
        public int Counter
        {
            get
            {
                return this.counter;
            }
        }

        private string symbol;
        private ISymbolDataUpdatedNotifier notifier;

        public QuotesStorageObserver(string symbol, ISymbolDataUpdatedNotifier notifier)
        {
            this.symbol = symbol;
            this.notifier = notifier;
            this.notifier.OnQuotesUpdate += new SymbolDataUpdatedNotification(OnQuotesUpdate);
        }

        public void OnQuotesUpdate(string symbol)
        {
            if (!this.symbol.Equals(symbol))
                return;

            this.counter++;
        }
    }

    [TestClass]
    public class QuotesHasBeenUpdatedNotificationTests
    {
        private OrderBookContext quotesStorage;
        private QuotesStorageObserver quotesStorageObserver;

        private string symbol;

        [TestInitialize]
        public void Setup()
        {
            this.symbol = "RTS-12.13";

            this.quotesStorage = new OrderBookContext();
            this.quotesStorageObserver = new QuotesStorageObserver(symbol, this.quotesStorage);
        }

        [TestMethod]
        public void QuotesObserver_Shoots_On_new_quote_test()
        {
            Assert.AreEqual(0, this.quotesStorageObserver.Counter);

            this.quotesStorage.Update(0, this.symbol, 145880, 300,0,0);

            Assert.AreEqual(1, this.quotesStorageObserver.Counter);
        }

        [TestMethod]
        public void QuotesObserver_ignore_unknown_symbol_test()
        {
            Assert.AreEqual(0, this.quotesStorageObserver.Counter);

            string unknownSymbol = "Si-12.13";

            Assert.AreNotEqual(this.symbol, unknownSymbol);

            this.quotesStorage.Update(0, unknownSymbol, 145880, 300, 0, 0);

            Assert.AreEqual(0, this.quotesStorageObserver.Counter);
        }
    }
}
