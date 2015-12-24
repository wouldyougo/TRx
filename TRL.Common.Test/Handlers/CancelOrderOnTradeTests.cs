using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
//using TRL.Common.Extensions.Models;
using TRL.Common.Extensions.Data;
using TRL.Common.Test.Mocks;
using TRL.Common.TimeHelpers;
using TRL.Common.Handlers;
using TRL.Common.Collections;
using TRL.Common.Extensions.Collections;
using TRL.Logging;

namespace TRL.Common.Handlers.Test
{
    [TestClass]
    public class CancelOrderOnTradeTests
    {
        private IDataContext tradingData;
        private StrategyHeader str1, str2, str3;
        private Signal s1, s2, s3;

        [TestInitialize]
        public void Handlers_Setup()
        {
            this.tradingData = new TradingDataContext();

            UpdatePositionOnTrade updateHandler = new UpdatePositionOnTrade(this.tradingData, new NullLogger());
            CancelOrderOnTrade cancelHandler = new CancelOrderOnTrade(this.tradingData, new NullLogger());

            this.str1 = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.str1);

            this.str2 = new StrategyHeader(2, "Strategy 2", "BP12345-RF-01", "Si-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.str2);

            this.str3 = new StrategyHeader(3, "Strategy 3", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.str3);

            this.s1 = new Signal(this.str1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150100);
            this.tradingData.Get<ICollection<Signal>>().Add(this.s1);

            Order o1 = new Order(this.s1);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Trade t1 = new Trade(o1, this.str1.Portfolio, this.str1.Symbol, this.s1.Limit, this.str1.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t1);

            this.s2 = new Signal(this.str2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 33000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(this.s2);

            Order o2 = new Order(this.s2);
            this.tradingData.Get<ICollection<Order>>().Add(o2);

            Trade t2 = new Trade(o2, this.str2.Portfolio, this.str2.Symbol, 33030, -this.str2.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t2);

            this.s3 = new Signal(this.str3, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(this.s1);

            Order o3 = new Order(this.s3);
            this.tradingData.Get<ICollection<Order>>().Add(o3);

            Trade t3 = new Trade(o3, this.str3.Portfolio, this.str3.Symbol, this.s3.Limit, this.str3.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t3);
        }

        [TestMethod]
        public void Handlers_do_nothing_when_long_position_closed_partially()
        {
            Signal sl = new Signal(this.str1, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Stop, 150000, 149100, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(sl);

            Order slo = new Order(sl);
            this.tradingData.Get<ICollection<Order>>().Add(slo);

            Signal tp = new Signal(this.str1, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 150100);
            this.tradingData.Get<ICollection<Signal>>().Add(tp);

            Order tpo = new Order(tp);
            this.tradingData.Get<ICollection<Order>>().Add(tpo);

            Assert.AreEqual(10, this.tradingData.GetAmount(this.str1));
            Assert.AreEqual(2, this.tradingData.Get<ICollection<Order>>().GetUnfilled(this.str1).Count());
            Assert.AreEqual(-20, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.str1));
            Assert.AreEqual(0, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);

            Trade t = new Trade(tpo, this.str1.Portfolio, this.str1.Symbol, 149100, -3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t);
            Assert.AreEqual(0, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);
        }

        [TestMethod]
        public void Handlers_cancel_limit_order_when_long_position_closed_by_stop()
        {
            Signal sl = new Signal(this.str1, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Stop, 150000, 149100, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(sl);

            Order slo = new Order(sl);
            this.tradingData.Get<ICollection<Order>>().Add(slo);

            Signal tp = new Signal(this.str1, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 150100);
            this.tradingData.Get<ICollection<Signal>>().Add(tp);

            Order tpo = new Order(tp);
            this.tradingData.Get<ICollection<Order>>().Add(tpo);

            Assert.AreEqual(10, this.tradingData.GetAmount(this.str1));
            Assert.AreEqual(2, this.tradingData.Get<ICollection<Order>>().GetUnfilled(this.str1).Count());
            Assert.AreEqual(-20, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.str1));
            Assert.AreEqual(0, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);

            Trade t = new Trade(tpo, this.str1.Portfolio, this.str1.Symbol, 149100, -this.str1.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t);
            Assert.AreEqual(1, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);

            OrderCancellationRequest request = this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Last();
            Assert.AreEqual(slo, request.Order);
            Assert.AreEqual(slo.Id, request.OrderId);
            Assert.AreEqual(String.Format("Отменить заявку {0}, потому что позиция была закрыта заявкой {1}", slo.ToString(), tpo.ToString()), request.Description);
        }

        [TestMethod]
        public void Handlers_cancel_stop_order_when_long_position_closed_by_limit()
        {
            Signal sl = new Signal(this.str1, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Stop, 150000, 149100, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(sl);

            Order slo = new Order(sl);
            this.tradingData.Get<ICollection<Order>>().Add(slo);

            Signal tp = new Signal(this.str1, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 150100);
            this.tradingData.Get<ICollection<Signal>>().Add(tp);

            Order tpo = new Order(tp);
            this.tradingData.Get<ICollection<Order>>().Add(tpo);

            Assert.AreEqual(10,this.tradingData.GetAmount(this.str1));
            Assert.AreEqual(2, this.tradingData.Get<ICollection<Order>>().GetUnfilled(this.str1).Count());
            Assert.AreEqual(-20, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.str1));
            Assert.AreEqual(0, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);

            Trade t = new Trade(slo, this.str1.Portfolio, this.str1.Symbol, 149100, -this.str1.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t);
            Assert.AreEqual(1, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);

            OrderCancellationRequest request = this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Last();
            Assert.AreEqual(tpo, request.Order);
            Assert.AreEqual(tpo.Id, request.OrderId);
            Assert.AreEqual(String.Format("Отменить заявку {0}, потому что позиция была закрыта заявкой {1}", tpo.ToString(), slo.ToString()), request.Description);
        }

        [TestMethod]
        public void Handlers_do_nothing_when_short_position_closed_partially()
        {
            Signal sl = new Signal(this.str2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Stop, 33000, 33100, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(sl);

            Order slo = new Order(sl);
            this.tradingData.Get<ICollection<Order>>().Add(slo);

            Signal tp = new Signal(this.str2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 33000, 0, 32900);
            this.tradingData.Get<ICollection<Signal>>().Add(tp);

            Order tpo = new Order(tp);
            this.tradingData.Get<ICollection<Order>>().Add(tpo);

            Assert.AreEqual(-10, this.tradingData.GetAmount(this.str2));
            Assert.AreEqual(2, this.tradingData.Get<ICollection<Order>>().GetUnfilled(this.str2).Count());
            Assert.AreEqual(20, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.str2));
            Assert.AreEqual(0, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);

            Trade t = new Trade(tpo, this.str2.Portfolio, this.str2.Symbol, 32900, 3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t);
            Assert.AreEqual(0, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);
        }

        [TestMethod]
        public void Handlers_cancel_limit_order_when_short_position_closed_by_stop()
        {
            Signal sl = new Signal(this.str2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Stop, 33000, 33100, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(sl);

            Order slo = new Order(sl);
            this.tradingData.Get<ICollection<Order>>().Add(slo);

            Signal tp = new Signal(this.str2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 33000, 0, 32900);
            this.tradingData.Get<ICollection<Signal>>().Add(tp);

            Order tpo = new Order(tp);
            this.tradingData.Get<ICollection<Order>>().Add(tpo);

            Assert.AreEqual(-10, this.tradingData.GetAmount(this.str2));
            Assert.AreEqual(2, this.tradingData.Get<ICollection<Order>>().GetUnfilled(this.str2).Count());
            Assert.AreEqual(20, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.str2));
            Assert.AreEqual(0, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);

            Trade t = new Trade(tpo, this.str2.Portfolio, this.str2.Symbol, 32900, this.str1.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t);
            Assert.AreEqual(1, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);

            OrderCancellationRequest request = this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Last();
            Assert.AreEqual(slo, request.Order);
            Assert.AreEqual(slo.Id, request.OrderId);
            Assert.AreEqual(String.Format("Отменить заявку {0}, потому что позиция была закрыта заявкой {1}", slo.ToString(), tpo.ToString()), request.Description);
        }

        [TestMethod]
        public void Handlers_cancel_stop_order_when_short_position_closed_by_limit()
        {
            Signal sl = new Signal(this.str2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Stop, 33000, 33100, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(sl);

            Order slo = new Order(sl);
            this.tradingData.Get<ICollection<Order>>().Add(slo);

            Signal tp = new Signal(this.str2, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 33000, 0, 32900);
            this.tradingData.Get<ICollection<Signal>>().Add(tp);

            Order tpo = new Order(tp);
            this.tradingData.Get<ICollection<Order>>().Add(tpo);

            Assert.AreEqual(-10, this.tradingData.GetAmount(this.str2));
            Assert.AreEqual(2, this.tradingData.Get<ICollection<Order>>().GetUnfilled(this.str2).Count());
            Assert.AreEqual(20, this.tradingData.Get<ICollection<Order>>().GetUnfilledSignedAmount(this.str2));
            Assert.AreEqual(0, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);

            Trade t = new Trade(slo, this.str2.Portfolio, this.str2.Symbol, 33100, this.str1.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t);
            Assert.AreEqual(1, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);

            OrderCancellationRequest request = this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Last();
            Assert.AreEqual(tpo, request.Order);
            Assert.AreEqual(tpo.Id, request.OrderId);
            Assert.AreEqual(String.Format("Отменить заявку {0}, потому что позиция была закрыта заявкой {1}", tpo.ToString(), slo.ToString()), request.Description);
        }
    }
}
