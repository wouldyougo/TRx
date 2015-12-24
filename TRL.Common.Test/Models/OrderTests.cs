using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using TRL.Configuration;
using System.Globalization;
using TRL.Common.Data;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class OrderTests
    {
        private StrategyHeader strategyHeader;
        private Signal signal;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-6.13_FT", 10);
            this.signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150100);
        }

        [TestMethod]
        public void Buy_Order_UnfilledSignedAmount_test()
        {
            Order order = new Order { Id = 1, Portfolio = this.signal.Strategy.Portfolio, Symbol = this.signal.Strategy.Symbol, TradeAction = this.signal.TradeAction, Amount = this.signal.Strategy.Amount, FilledAmount = 3, Signal = this.signal };

            Assert.AreEqual(7, order.UnfilledSignedAmount);
        }

        [TestMethod]
        public void Sell_Order_UnfilledSignedAmount_test()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 149900);
            Order order = new Order { Id = 1, Portfolio = signal.Strategy.Portfolio, Symbol = signal.Strategy.Symbol, TradeAction = signal.TradeAction, Amount = signal.Strategy.Amount, FilledAmount = 3, Signal = signal };

            Assert.AreEqual(-7, order.UnfilledSignedAmount);
        }

        [TestMethod]
        public void ByDefault_Order_Expires_At_Midnight()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);

            Assert.AreEqual(this.strategyHeader.Portfolio, order.Portfolio);
            Assert.AreEqual(this.strategyHeader.Symbol, order.Symbol);
            Assert.AreEqual(signal.TradeAction, order.TradeAction);
            Assert.AreEqual(signal.OrderType, order.OrderType);
            Assert.AreEqual(0, order.Price);
            Assert.AreEqual(10, order.Amount);
            Assert.AreEqual(0, order.Stop);
            Assert.AreEqual(signal, order.Signal);
            Assert.AreEqual(signal.Id, order.SignalId);

            ITradingSchedule tradingSchedule = new FortsTradingSchedule();

            Assert.AreEqual(tradingSchedule.SessionEnd, order.ExpirationDate);
        }

        [TestMethod]
        public void ByDefault_Order_Is_Not_Expired()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);

            Assert.IsFalse(order.IsExpired);
        }

        [TestMethod]
        public void Order_Is_Expired()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);
            order.ExpirationDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(-5);

            Assert.IsTrue(order.IsExpired);
        }

        [TestMethod]
        public void Order_Is_Not_Filled()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);

            Assert.IsFalse(order.IsFilled);
        }

        [TestMethod]
        public void Order_IsFilled()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);

            order.FilledAmount = 10;

            Assert.IsTrue(order.IsFilled);
        }

        [TestMethod]
        public void New_Order_IsNotFilledPartially()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);

            Assert.IsFalse(order.IsFilledPartially);
        }

        [TestMethod]
        public void Order_IsFilledPartially()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);

            order.FilledAmount = 1;

            Assert.IsTrue(order.IsFilledPartially);
        }

        [TestMethod]
        public void Filled_Order_Is_Not_Filled_Partially()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);

            order.FilledAmount = 10;

            Assert.IsTrue(order.IsFilled);
            Assert.IsFalse(order.IsFilledPartially);
        }

        [TestMethod]
        public void ByDefault_Order_Is_Not_Rejected()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);

            Assert.IsFalse(order.IsRejected);
        }

        [TestMethod]
        public void Order_IsRejected()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);

            order.Reject(new DateTime(2013, 1, 1), "Rejected by broker");

            Assert.IsTrue(order.IsRejected);
            Assert.AreEqual(new DateTime(2013, 1, 1), order.RejectedDate);
            Assert.AreEqual("Rejected by broker", order.RejectReason);
        }

        [TestMethod]
        public void ByDefault_Order_Id_Is_Not_Null()
        {
            Order order = new Order();

            Assert.IsTrue(order.Id != 0);
        }

        [TestMethod]
        public void Orders_AreSimilar()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order first = new Order(signal);

            Signal signal1 = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 146000, 0, 0);

            Order second = new Order(signal1);

            Assert.IsTrue(first.IsLike(second));
        }

        [TestMethod]
        public void Order_ToString_Test()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);

            string result = String.Format("Order Id: {0}, DateTime: {1}, Portfolio: {2}, Symbol: {3}, Action: {4}, Type: {5}, Price: {6}, Amount: {7}, Stop: {8}, FilledAmount: {9}, DeliveryDate: {10}, RejectDate: {11}, RejectReason: {12}, ExpirationDate: {13}, CancellationDate: {14}, CancellationReason: {15}, Signal: {16}",
                order.Id, order.DateTime.ToString(ci), order.Portfolio, 
                order.Symbol, order.TradeAction, order.OrderType,
                order.Price.ToString("0.0000", ci), order.Amount.ToString("0.0000", ci), order.Stop.ToString("0.0000", ci), 
                order.FilledAmount.ToString("0.0000", ci), order.DeliveryDate.ToString(ci),
                order.RejectedDate.ToString(ci), 
                order.RejectReason, order.ExpirationDate.ToString(ci), 
                order.CancellationDate.ToString(ci), order.CancellationReason, signal.Id);

            Assert.AreEqual(result, order.ToString());
        }

        [TestMethod]
        public void Order_Without_Position_ToImportString_Test()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);

            string result = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                order.Id, order.DateTime.ToString(ci), order.Portfolio,
                order.Symbol, order.TradeAction, order.OrderType,
                order.Price.ToString("0.0000", ci), order.Amount.ToString("0.0000", ci), order.Stop.ToString("0.0000", ci),
                order.FilledAmount.ToString("0.0000", ci), order.DeliveryDate.ToString(ci), order.RejectedDate.ToString(ci),
                order.RejectReason, order.ExpirationDate.ToString(ci),
                order.CancellationDate.ToString(ci), order.CancellationReason, signal.Id);

            Assert.AreEqual(result, order.ToImportString());
        }

        [TestMethod]
        public void new_Order_is_not_canceled()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);
            Assert.IsFalse(order.IsCanceled);
        }

        [TestMethod]
        public void Cancel_Order()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);

            Order order = new Order(signal);
            Assert.IsFalse(order.IsCanceled);
            
            DateTime cancelDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(100);
            order.Cancel(cancelDate, "Цена ушла");

            Assert.IsTrue(order.IsCanceled);
            Assert.AreEqual(cancelDate, order.CancellationDate);
            Assert.AreEqual("Цена ушла", order.CancellationReason);
        }

        [TestMethod]
        public void Order_Expires_After_TimeToLive()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 0, 0);
            
            int timeToLiveSeconds = 300;
            DateTime expirationDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(300);

            Order order = new Order(signal, timeToLiveSeconds);

            Assert.AreEqual(this.strategyHeader.Portfolio, order.Portfolio);
            Assert.AreEqual(this.strategyHeader.Symbol, order.Symbol);
            Assert.AreEqual(signal.TradeAction, order.TradeAction);
            Assert.AreEqual(signal.OrderType, order.OrderType);
            Assert.AreEqual(0, order.Price);
            Assert.AreEqual(10, order.Amount);
            Assert.AreEqual(0, order.Stop);
            Assert.AreEqual(signal, order.Signal);
            Assert.AreEqual(signal.Id, order.SignalId);
            Assert.IsTrue(expirationDate <= order.ExpirationDate);
        }

        [TestMethod]
        public void construct_limit_order_from_signal_test()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 149000, 0, 140000);

            int timeToLiveSeconds = 300;
            DateTime expirationDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(300);

            Order order = new Order(signal, timeToLiveSeconds);

            Assert.AreEqual(this.strategyHeader.Portfolio, order.Portfolio);
            Assert.AreEqual(this.strategyHeader.Symbol, order.Symbol);
            Assert.AreEqual(signal.TradeAction, order.TradeAction);
            Assert.AreEqual(signal.OrderType, order.OrderType);
            Assert.AreEqual(140000, order.Price);
            Assert.AreEqual(10, order.Amount);
            Assert.AreEqual(0, order.Stop);
            Assert.AreEqual(signal, order.Signal);
            Assert.AreEqual(signal.Id, order.SignalId);
            Assert.IsTrue(expirationDate <= order.ExpirationDate);
        }

        [TestMethod]
        public void construct_stop_order_from_signal_test()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 149000, 148000, 0);

            int timeToLiveSeconds = 300;
            DateTime expirationDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(300);

            Order order = new Order(signal, timeToLiveSeconds);

            Assert.AreEqual(this.strategyHeader.Portfolio, order.Portfolio);
            Assert.AreEqual(this.strategyHeader.Symbol, order.Symbol);
            Assert.AreEqual(signal.TradeAction, order.TradeAction);
            Assert.AreEqual(signal.OrderType, order.OrderType);
            Assert.AreEqual(0, order.Price);
            Assert.AreEqual(10, order.Amount);
            Assert.AreEqual(148000, order.Stop);
            Assert.AreEqual(signal, order.Signal);
            Assert.AreEqual(signal.Id, order.SignalId);
            Assert.IsTrue(expirationDate <= order.ExpirationDate);
        }

        [TestMethod]
        public void order_is_not_delivered()
        {
            Order o = new Order(1,
                BrokerDateTime.Make(DateTime.Now),
                "BP12345-RF-01",
                "RTS-9.13_FT",
                TradeAction.Sell,
                OrderType.Market,
                10,
                0,
                0);
            Assert.IsFalse(o.IsDelivered);
        }

        [TestMethod]
        public void order_is_delivered()
        {
            Order o = new Order(1,
                BrokerDateTime.Make(DateTime.Now),
                "BP12345-RF-01",
                "RTS-9.13_FT",
                TradeAction.Sell,
                OrderType.Market,
                10,
                0,
                0);
            Assert.IsFalse(o.IsDelivered);

            o.DeliveryDate = BrokerDateTime.Make(DateTime.Now);
            Assert.IsTrue(o.IsDelivered);
        }

        [TestMethod]
        public void order_parse_test()
        {
            string importString = "1, 01/01/2013 12:24:40, BP12345-RF-01, RTS-9.13_FT, Buy, Limit, 150000.0000, 10.0000, 0.0000, 0.0000, 01/01/2013 12:24:41, 01/01/0001 00:00:00, , 01/01/0001 00:00:00, 01/01/0001 00:00:00, , 33";

            Order order = Order.Parse(importString);
        
            Assert.AreEqual("BP12345-RF-01", order.Portfolio);
            Assert.AreEqual("RTS-9.13_FT", order.Symbol);
            Assert.AreEqual(TradeAction.Buy, order.TradeAction);
            Assert.AreEqual(OrderType.Limit, order.OrderType);
            Assert.AreEqual(150000, order.Price);
            Assert.AreEqual(10, order.Amount);
            Assert.AreEqual(0, order.FilledAmount);
            Assert.AreEqual(0, order.Stop);
            Assert.AreEqual(new DateTime(2013, 1, 1, 12, 24, 40), order.DateTime);
            Assert.AreEqual(new DateTime(2013, 1, 1, 12, 24, 41), order.DeliveryDate);
            Assert.AreEqual(DateTime.MinValue, order.CancellationDate);
            Assert.AreEqual(string.Empty, order.CancellationReason);
            Assert.AreEqual(DateTime.MinValue, order.RejectedDate);
            Assert.AreEqual(string.Empty, order.RejectReason);
            Assert.AreEqual(DateTime.MinValue, order.ExpirationDate);
            Assert.AreEqual(33, order.SignalId);
        }
    }
}
