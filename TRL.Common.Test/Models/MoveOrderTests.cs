using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using System.Globalization;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class MoveOrderTests
    {
        private StrategyHeader strategyHeader;
        private Signal signal;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader = new StrategyHeader(1, "Strategy", "Portfolio", "Symbol", 10);
            this.signal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Stop, 150000, 149900, 0);
        }

        [TestMethod]
        public void MoveOrder_constructor_test()
        {
            DateTime date = DateTime.Now;
            Order order = new Order(this.signal);
            double price = 149910;
            string description = "Текущая цена сдвинулась на 10 пунктов в сторону прибыли.";

            MoveOrder moveOrder = new MoveOrder(order, price, date, description);

            Assert.AreEqual(order.Id, moveOrder.OrderId);
            Assert.AreEqual(order, moveOrder.Order);
            Assert.AreEqual(date, moveOrder.DateTime);
            Assert.AreEqual(DateTime.MinValue, moveOrder.DeliveryDate);
            Assert.AreEqual(DateTime.MinValue, moveOrder.MoveDate);
            Assert.AreEqual(price, moveOrder.Price);
            Assert.AreEqual(description, moveOrder.Description);
            Assert.IsFalse(moveOrder.IsDelivered);
            Assert.IsFalse(moveOrder.IsMoved);
        }

        [TestMethod]
        public void MoveOrder_ToString_test()
        {
            DateTime date = DateTime.Now;
            Order order = new Order(this.signal);
            double price = 149910;
            string description = "Текущая цена сдвинулась на 10 пунктов в сторону прибыли.";

            MoveOrder moveOrder = new MoveOrder(order, price, date, description);

            CultureInfo ci = CultureInfo.InvariantCulture;

            string result = String.Format("Запрос на перемещение заявки: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
                moveOrder.Id,
                moveOrder.OrderId,
                moveOrder.DateTime.ToString(ci),
                moveOrder.Price.ToString("0.0000", ci),
                moveOrder.Description,
                moveOrder.DeliveryDate.ToString(ci),
                moveOrder.MoveDate.ToString(ci),
                moveOrder.Order);

            Assert.AreEqual(result, moveOrder.ToString());
        }

        [TestMethod]
        public void MoveOrder_IsDelivered_test()
        {
            DateTime date = DateTime.Now;
            Order order = new Order(this.signal);
            double price = 149910;
            string description = "Текущая цена сдвинулась на 10 пунктов в сторону прибыли.";

            MoveOrder moveOrder = new MoveOrder(order, price, date, description);
            moveOrder.DeliveryDate = DateTime.Now;

            Assert.IsTrue(moveOrder.IsDelivered);
        }

        [TestMethod]
        public void MoveOrder_IsMoved_test()
        {
            DateTime date = DateTime.Now;
            Order order = new Order(this.signal);
            double price = 149910;
            string description = "Текущая цена сдвинулась на 10 пунктов в сторону прибыли.";

            MoveOrder moveOrder = new MoveOrder(order, price, date, description);
            moveOrder.MoveDate = DateTime.Now;

            Assert.IsTrue(moveOrder.IsMoved);
        }

    }
}
