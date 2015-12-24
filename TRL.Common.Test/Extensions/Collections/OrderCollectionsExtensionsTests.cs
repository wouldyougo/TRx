using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
//using TRL.Common.Extensions.Data;
//using TRL.Common.Extensions.Models;
using TRL.Common.Extensions.Collections;

namespace TRL.Common.Extensions.Collections.Test
{
    [TestClass]
    public class OrderCollectionsExtensionsTests
    {
        private IDataContext tradingData;
        private StrategyHeader s1;
        private StrategyHeader s2;
        private StrategyHeader s3;
        private Signal signal1;
        private Signal signal2;
        private Signal signal3;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.s1 = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.s2 = new StrategyHeader(2, "Strategy 2", "BP12345-RF-01", "Si-9.13_FT", 10);
            this.s3 = new StrategyHeader(3, "Strategy 3", "BP12345-RF-01", "RTS-9.13_FT", 10);

            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.s1);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.s2);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.s3);

            this.signal1 = new Signal(this.s1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 130000, 0, 0);
            this.signal2 = new Signal(this.s2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 31000, 0, 0);
            this.signal3 = new Signal(this.s3, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 130000, 0, 0);

            this.tradingData.Get<ICollection<Signal>>().Add(this.signal1);
            this.tradingData.Get<ICollection<Signal>>().Add(this.signal2);
            this.tradingData.Get<ICollection<Signal>>().Add(this.signal3);
        }

        [TestMethod]
        public void OrderCollectionExtensions_GetUnfilled()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, FilledAmount = this.signal1.Strategy.Amount };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 3 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(1, this.tradingData.Get<ICollection<Order>>().GetUnfilled(this.s1.Portfolio, this.s1.Symbol).Count());
        }

        [TestMethod]
        public void OrderCollectionExtensions_GetUnfilledAmount()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, FilledAmount = 9 };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 3 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(8, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.s1.Portfolio, this.s1.Symbol));
        }

        [TestMethod]
        public void OrderCollectionExtensions_GetUnfilled_ignore_expired_orders()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, ExpirationDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(-1) };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 3 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(7, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.s1.Portfolio, this.s1.Symbol));
        }

        [TestMethod]
        public void OrderCollectionExtensions_GetUnfilled_ignore_rejected_orders()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, RejectedDate = BrokerDateTime.Make(DateTime.Now), RejectReason = "Rejected" };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 5 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(5, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.s1.Portfolio, this.s1.Symbol));
        }

        [TestMethod]
        public void OrderCollectionExtensions_GetUnfilled_ignore_cancelled_orders()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, CancellationDate = BrokerDateTime.Make(DateTime.Now), CancellationReason = "Canceled" };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 2 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(8, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.s1.Portfolio, this.s1.Symbol));
        }

        [TestMethod]
        public void OrderCollectionExtensions_get_strategy_unfilled()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, FilledAmount = this.signal1.Strategy.Amount };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 3 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(1, this.tradingData.Get<ICollection<Order>>().GetUnfilled(this.s3).Count());
        }

        [TestMethod]
        public void OrderCollectionExtensions_get_unfilled_strategy_amount()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, FilledAmount = 9 };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 3 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(7, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.s3));
        }

        [TestMethod]
        public void OrderCollectionExtensions_get_unfilled_another_strategy_amount()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, FilledAmount = 9 };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 3 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(1, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.s1));
        }

        [TestMethod]
        public void OrderCollectionExtensions_strategy_unfilled_ignore_expired_orders()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, ExpirationDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(-1) };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 3 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.s1));
        }

        [TestMethod]
        public void OrderCollectionExtensions_strategy_unfilled_ignore_rejected_orders()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, RejectedDate = BrokerDateTime.Make(DateTime.Now), RejectReason = "Rejected" };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 5 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.s1));
        }

        [TestMethod]
        public void OrderCollectionExtensions_strategy_unfilled_ignore_cancelled_orders()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, CancellationDate = BrokerDateTime.Make(DateTime.Now), CancellationReason = "Canceled" };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 2 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(0, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.s1));
        }

        [TestMethod]
        public void get_unfilled_strategy_order_like_signal()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, FilledAmount = this.signal1.Strategy.Amount };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 2 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Signal signal = new Signal(this.s3, BrokerDateTime.Make(DateTime.Now), this.signal3.TradeAction, this.signal3.OrderType, this.signal3.Price, this.signal3.Stop, this.signal3.Limit);

            Assert.AreEqual(1, this.tradingData.Get<ICollection<Order>>().GetUnfilledOrderJustLikeASignal(signal).Count());
        }

        [TestMethod]
        public void orders_can_be_cleared()
        {
            Order o1 = new Order(this.signal1, 60);
            o1.FilledAmount = 10;
            Assert.IsTrue(o1.IsFilled);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Order o2 = new Order(this.signal2, 60);
            this.tradingData.Get<ICollection<Order>>().Add(o2);

            Order o3 = new Order(this.signal3, 60);
            o3.Cancel(BrokerDateTime.Make(DateTime.Now).AddSeconds(-61), "Cancel");
            Assert.IsTrue(o3.IsCanceled);
            this.tradingData.Get<ICollection<Order>>().Add(o3);

            Order o4 = new Order(this.signal1, 300);
            o4.Reject(BrokerDateTime.Make(DateTime.Now).AddSeconds(-301), "Reject");
            Assert.IsTrue(o4.IsRejected);
            this.tradingData.Get<ICollection<Order>>().Add(o4);

            Order o5 = new Order(this.signal2, 300);
            this.tradingData.Get<ICollection<Order>>().Add(o5);

            Order o6 = new Order(this.signal3, 300);
            this.tradingData.Get<ICollection<Order>>().Add(o6);

            Signal s = new Signal(this.s1, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            Order order = new Order(s, 60);

            IEnumerable<Order> orders = this.tradingData.Get<ICollection<Order>>().GetOrdersThatCanBeClearedWith(order);

            Assert.IsNotNull(orders);
            Assert.AreEqual(1, orders.Count());
            Assert.AreSame(o6, orders.Last());
        }

        [TestMethod]
        public void no_orders_that_can_be_cleared()
        {
            Order o1 = new Order(this.signal1, 60);
            o1.FilledAmount = 10;
            Assert.IsTrue(o1.IsFilled);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Order o2 = new Order(this.signal2, 60);
            this.tradingData.Get<ICollection<Order>>().Add(o2);

            Order o3 = new Order(this.signal3, 60);
            o3.Cancel(BrokerDateTime.Make(DateTime.Now).AddSeconds(-61), "Cancel");
            Assert.IsTrue(o3.IsCanceled);
            this.tradingData.Get<ICollection<Order>>().Add(o3);

            Order o4 = new Order(this.signal1, 300);
            o4.Reject(BrokerDateTime.Make(DateTime.Now).AddSeconds(-301), "Reject");
            Assert.IsTrue(o4.IsRejected);
            this.tradingData.Get<ICollection<Order>>().Add(o4);

            Order o5 = new Order(this.signal2, 300);
            this.tradingData.Get<ICollection<Order>>().Add(o5);

            Signal s = new Signal(this.s1, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 150000, 0, 0);
            Order order = new Order(s, 60);

            IEnumerable<Order> orders = this.tradingData.Get<ICollection<Order>>().GetOrdersThatCanBeClearedWith(order);

            Assert.IsNotNull(orders);
            Assert.AreEqual(0, orders.Count());
        }

        [TestMethod]
        public void find_unfilled_orders_for_symbol()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, FilledAmount = this.signal1.Strategy.Amount };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 3 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(1, this.tradingData.Get<ICollection<Order>>().GetUnfilled(this.s1.Symbol).Count());
        }

        [TestMethod]
        public void OrderCollectionExtensions_GetUnfilled_for_symbol_ignore_expired_orders()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, ExpirationDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(-1) };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 3 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(1, this.tradingData.Get<ICollection<Order>>().GetUnfilled(this.s1.Symbol).Count());
        }

        [TestMethod]
        public void OrderCollectionExtensions_GetUnfilled_for_symbol_ignore_rejected_orders()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, RejectedDate = BrokerDateTime.Make(DateTime.Now), RejectReason = "Rejected" };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 5 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(1, this.tradingData.Get<ICollection<Order>>().GetUnfilled(this.s1.Symbol).Count());
        }

        [TestMethod]
        public void OrderCollectionExtensions_GetUnfilled_for_symbol_ignore_cancelled_orders()
        {
            Order order1 = new Order { Id = 1, Portfolio = this.s1.Portfolio, Symbol = this.s1.Symbol, Amount = this.s1.Amount, Signal = this.signal1, SignalId = this.signal1.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal1.TradeAction, OrderType = this.signal1.OrderType, CancellationDate = BrokerDateTime.Make(DateTime.Now), CancellationReason = "Canceled" };
            Order order2 = new Order { Id = 2, Portfolio = this.s2.Portfolio, Symbol = this.s2.Symbol, Amount = this.s2.Amount, Signal = this.signal2, SignalId = this.signal2.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal2.TradeAction, OrderType = this.signal2.OrderType, FilledAmount = this.signal2.Strategy.Amount };
            Order order3 = new Order { Id = 3, Portfolio = this.s3.Portfolio, Symbol = this.s3.Symbol, Amount = this.s3.Amount, Signal = this.signal3, SignalId = this.signal3.Id, DateTime = BrokerDateTime.Make(DateTime.Now), TradeAction = this.signal3.TradeAction, OrderType = this.signal3.OrderType, FilledAmount = 2 };

            this.tradingData.Get<ICollection<Order>>().Add(order1);
            this.tradingData.Get<ICollection<Order>>().Add(order2);
            this.tradingData.Get<ICollection<Order>>().Add(order3);

            Assert.AreEqual(1, this.tradingData.Get<ICollection<Order>>().GetUnfilled(this.s1.Symbol).Count());
        }

        [TestMethod]
        public void OrderCollectionExtensions_get_oldest_order_by_with_price()
        {
            List<Order> orders = new List<Order>();

            orders.Add(new Order(1, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150000, 0));
            orders.Add(new Order(2, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150010, 0));
            orders.Add(new Order(3, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150010, 0));
            orders.Add(new Order(4, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150020, 0));
            orders.Add(new Order(5, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150030, 0));
            orders.Add(new Order(6, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150020, 0));
            orders.Add(new Order(7, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150010, 0));
            orders.Add(new Order(8, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150000, 0));
            orders.Add(new Order(9, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150050, 0));
            orders.Add(new Order(10, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150040, 0));

            Order order = orders.GetOldestOrderWithPrice(150020);
            Assert.AreEqual(4, order.Id);
        }

        [TestMethod]
        public void OrderCollectionExtensions_get_oldest_order_by_with_price_returns_null()
        {
            List<Order> orders = new List<Order>();

            orders.Add(new Order(1, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150000, 0));
            orders.Add(new Order(2, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150010, 0));
            orders.Add(new Order(3, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150010, 0));
            orders.Add(new Order(4, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150020, 0));
            orders.Add(new Order(5, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150030, 0));
            orders.Add(new Order(6, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150020, 0));
            orders.Add(new Order(7, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150010, 0));
            orders.Add(new Order(8, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150000, 0));
            orders.Add(new Order(9, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150050, 0));
            orders.Add(new Order(10, BrokerDateTime.Make(DateTime.Now), "BP12345-RF-01", "RTS-9.13_FT", TradeAction.Buy, OrderType.Limit, 100, 150040, 0));

            Order order = orders.GetOldestOrderWithPrice(150060);
            Assert.IsNull(order);
        }
    }
}
