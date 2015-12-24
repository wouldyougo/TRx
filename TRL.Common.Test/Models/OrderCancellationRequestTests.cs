using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class OrderCancellationRequestTests
    {
        [TestMethod]
        public void OrderCancellationRequest_constructor_test()
        {
            Order order = new Order{ Portfolio = "BP12345-RF-01", Symbol = "RTS-9.13_FT", TradeAction = TradeAction.Buy, OrderType = OrderType.Limit, Amount = 10 };

            OrderCancellationRequest request = new OrderCancellationRequest(order, "Cигнал номер 3923");

            Assert.IsTrue(request.Id > 0);
            Assert.AreEqual(order.Id, request.OrderId);
            Assert.AreEqual("Cигнал номер 3923", request.Description);
            Assert.AreEqual(order, request.Order);
            Assert.IsTrue(request.DateTime >= order.DateTime);
        }

        [TestMethod]
        public void OrderCancellationRequest_duplicates()
        {
            Order order = new Order { Portfolio = "BP12345-RF-01", Symbol = "RTS-9.13_FT", TradeAction = TradeAction.Buy, OrderType = OrderType.Limit, Amount = 10 };

            OrderCancellationRequest request = new OrderCancellationRequest(order, "Cигнал номер 3923");

            HashSet<OrderCancellationRequest> collection = new HashSet<OrderCancellationRequest>(new IdentifiedComparer());

            collection.Add(request);
            Assert.AreEqual(1, collection.Count);

            collection.Add(request);
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void OrderCancellationRequest_ToString_test()
        {
            Order order = new Order(1,
                BrokerDateTime.Make(DateTime.Now),
                "BP12345-RF-01",
                "RTS-12.13_FT",
                TradeAction.Buy,
                OrderType.Limit,
                1,
                150000,
                0);
            
            OrderCancellationRequest request = new OrderCancellationRequest(order, "Лучшая цена предложения ушла от цены заявки на 100 пунктов.");
            string result = String.Format("Запрос на отмену заявки Id: {0}, DateTime: {1}, Description: {2}, OrderId: {3}",request.Id, request.DateTime, request.Description, request.OrderId );

            Assert.AreEqual(result, request.ToString());
        }
    }
}
