using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Csharp.Models;

namespace TRL.Csharp.Test.Models
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void Order_constructor_test()
        {
            int id = 95;
            DateTime date = DateTime.Now;
            string portfolio = "ST12345-RF-01";
            string symbol = "RTS-9.14";
            OrderType orderType = OrderType.Limit;
            double price = 124000;
            double amount = -10;

            Order order = new Order(id, date, portfolio, symbol, orderType, price, amount);

            Assert.AreEqual(id, order.Id);
            Assert.AreEqual(date, order.DateTime);
            Assert.AreEqual(portfolio, order.Portfolio);
            Assert.AreEqual(symbol, order.Symbol);
            Assert.AreEqual(orderType, order.OrderType);
            Assert.AreEqual(price, order.Price);
            Assert.AreEqual(amount, order.Amount);

        }
    }
}
