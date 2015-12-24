using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using System.Globalization;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class OrderCancellationConfirmationTests
    {
        [TestMethod]
        public void OrderCancellationConfirmation_constructor_test()
        {
            Order o = new Order { Portfolio = "BP12345-RF-01", Symbol = "RTS-6.13_FT", TradeAction = TradeAction.Buy, OrderType = OrderType.Market, Amount = 10 };

            DateTime cancelDate = BrokerDateTime.Make(DateTime.Now);

            OrderCancellationConfirmation co = new OrderCancellationConfirmation(o, cancelDate, "Отменен пользователем");

            Assert.IsTrue(co.Id > 0);
            Assert.AreEqual(o.Id, co.OrderId);
            Assert.AreEqual(o, co.Order);
            Assert.AreEqual(cancelDate, co.DateTime);
        }

        [TestMethod]
        public void OrderCancellationConfirmation_ToString_test()
        {
            DateTime date = DateTime.Now;

            StrategyHeader strategyHeader = new StrategyHeader(1, "Description", "Portfolio", "Symbol", 10);
            Signal signal = new Signal(strategyHeader, date, TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            Order order = new Order(signal);

            OrderCancellationConfirmation co = new OrderCancellationConfirmation(order, date, "Отменен пользователем");

            string result = String.Format("Подтверждение об отмене заявки {0}, {1}, {2}",
                date.ToString(CultureInfo.InvariantCulture),
                co.Description,
                order.ToString());

            Assert.AreEqual(result, co.ToString());
                
        }
    }
}
