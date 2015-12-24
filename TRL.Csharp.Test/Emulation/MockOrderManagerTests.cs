using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Csharp.Models;
using TRL.Csharp.Collections;
using TRL.Csharp.Data;
using System.Collections.Generic;

namespace TRL.Csharp.Test.Emulation
{
    [TestClass]
    public class MockOrderManagerTests
    {
        private DataContext tradingData;
        private OrderManager orderManager;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.orderManager = new MockOrderManager(this.tradingData);

            Assert.AreEqual(0, tradingData.Get<GenericObservableHashSet<Trade>>().Count());
        }

        [TestMethod]
        public void execute_Trade_on_PlaceOrder_test()
        {
            Order order = new Order(1, DateTime.Now, "ST12345-RF-01", "RTS-9.14", OrderType.Market, 120000, 15);
            orderManager.PlaceOrder(order);

            Assert.AreEqual(1, tradingData.Get<GenericObservableHashSet<Trade>>().Count());

            Trade trade = tradingData.Get<IEnumerable<Trade>>().Last();
            Assert.AreEqual(order, trade.Order);
            Assert.AreEqual(order.Price, trade.Price);
            Assert.AreEqual(order.Amount, trade.Amount);
        }
    }
}
