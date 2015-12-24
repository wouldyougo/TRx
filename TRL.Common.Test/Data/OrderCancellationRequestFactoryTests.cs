using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Test.Data
{
    [TestClass]
    public class OrderCancellationRequestFactoryTests
    {
        private TradingDataContext tradingData;
        private Symbol symbol;
        private StrategyHeader strategyHeader;
        private StopPointsSettings slSettings;
        private ProfitPointsSettings tpSettings;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();

            this.symbol = new Symbol("RTS-9.13_FT", 1, 8, 10, BrokerDateTime.Make(DateTime.Now).AddDays(1));
            this.tradingData.Get<ICollection<Symbol>>().Add(this.symbol);

            this.strategyHeader = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.slSettings = new StopPointsSettings(this.strategyHeader, 50, false);
            this.tradingData.Get<ICollection<StopPointsSettings>>().Add(this.slSettings);

            this.tpSettings = new ProfitPointsSettings(this.strategyHeader, 80, false);
            this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(this.tpSettings);
        }

        [TestMethod]
        public void cancel_partially_filled_for_limit_to_buy_order_when_current_price_near_take_profit()
        {
            Signal s1 = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Trade t1 = new Trade(o1, this.strategyHeader.Portfolio, this.strategyHeader.Symbol, 150000, 3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ICollection<Trade>>().Add(t1);
            o1.FilledAmount = t1.Amount;
            Assert.IsFalse(o1.IsFilled);
            Assert.IsTrue(o1.IsFilledPartially);

            double currentPrice = 150070;
            DateTime date = BrokerDateTime.Make(DateTime.Now);
            string description = String.Format("Текущая цена {0} на расстоянии одного шага от take profit цены {1} стратегии.", currentPrice, s1.Limit + this.tpSettings.Points); 

            IGenericFactory<OrderCancellationRequest> factory = new UnfilledOrderCancellationRequestFactory(currentPrice, o1, tradingData);

            OrderCancellationRequest request = factory.Make();

            Assert.IsTrue(request.Id > 0);
            Assert.AreEqual(o1, request.Order);
            Assert.AreEqual(o1.Id, request.OrderId);
            Assert.IsTrue(request.DateTime >= date);
            Assert.AreEqual(description, request.Description);
        }

        [TestMethod]
        public void cancel_partially_filled_for_limit_to_buy_order_when_current_price_near_stop_loss()
        {
            Signal s1 = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Trade t1 = new Trade(o1, this.strategyHeader.Portfolio, this.strategyHeader.Symbol, 150000, 3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ICollection<Trade>>().Add(t1);
            o1.FilledAmount = t1.Amount;
            Assert.IsFalse(o1.IsFilled);
            Assert.IsTrue(o1.IsFilledPartially);

            double currentPrice = 149960;
            DateTime date = BrokerDateTime.Make(DateTime.Now);
            string description = String.Format("Текущая цена {0} на расстоянии одного шага от stop loss цены {1} стратегии.", currentPrice, s1.Limit - this.slSettings.Points);

            IGenericFactory<OrderCancellationRequest> factory = new UnfilledOrderCancellationRequestFactory(currentPrice, o1, tradingData);

            OrderCancellationRequest request = factory.Make();

            Assert.IsTrue(request.Id > 0);
            Assert.AreEqual(o1, request.Order);
            Assert.AreEqual(o1.Id, request.OrderId);
            Assert.IsTrue(request.DateTime >= date);
            Assert.AreEqual(description, request.Description);
        }

        [TestMethod]
        public void cancel_unfilled_for_limit_to_buy_order_when_current_price_near_take_profit()
        {
            Signal s1 = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Assert.IsFalse(o1.IsFilled);
            Assert.IsFalse(o1.IsFilledPartially);

            double currentPrice = 150070;
            DateTime date = BrokerDateTime.Make(DateTime.Now);
            string description = String.Format("Текущая цена {0} на расстоянии одного шага от take profit цены {1} стратегии.", currentPrice, s1.Limit + this.tpSettings.Points);

            IGenericFactory<OrderCancellationRequest> factory = new UnfilledOrderCancellationRequestFactory(currentPrice, o1, tradingData);

            OrderCancellationRequest request = factory.Make();

            Assert.IsTrue(request.Id > 0);
            Assert.AreEqual(o1, request.Order);
            Assert.AreEqual(o1.Id, request.OrderId);
            Assert.IsTrue(request.DateTime >= date);
            Assert.AreEqual(description, request.Description);
        }

        [TestMethod]
        public void cancel_unfilled_for_limit_to_buy_order_when_current_price_near_stop_loss()
        {
            Signal s1 = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Assert.IsFalse(o1.IsFilled);
            Assert.IsFalse(o1.IsFilledPartially);

            double currentPrice = 149960;
            DateTime date = BrokerDateTime.Make(DateTime.Now);
            string description = String.Format("Текущая цена {0} на расстоянии одного шага от stop loss цены {1} стратегии.", currentPrice, s1.Limit - this.slSettings.Points);

            IGenericFactory<OrderCancellationRequest> factory = new UnfilledOrderCancellationRequestFactory(currentPrice, o1, tradingData);

            OrderCancellationRequest request = factory.Make();

            Assert.IsTrue(request.Id > 0);
            Assert.AreEqual(o1, request.Order);
            Assert.AreEqual(o1.Id, request.OrderId);
            Assert.IsTrue(request.DateTime >= date);
            Assert.AreEqual(description, request.Description);
        }

        [TestMethod]
        public void make_request_for_limit_to_sell_order_when_current_price_near_take_profit()
        {
            Signal s1 = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Trade t1 = new Trade(o1, this.strategyHeader.Portfolio, this.strategyHeader.Symbol, 150000, -3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ICollection<Trade>>().Add(t1);
            o1.FilledAmount = t1.Amount * -1;
            Assert.IsFalse(o1.IsFilled);
            Assert.IsTrue(o1.IsFilledPartially);

            double currentPrice = 149930;
            DateTime date = BrokerDateTime.Make(DateTime.Now);
            string description = String.Format("Текущая цена {0} на расстоянии одного шага от take profit цены {1} стратегии.", currentPrice, s1.Limit - this.tpSettings.Points);

            IGenericFactory<OrderCancellationRequest> factory = new UnfilledOrderCancellationRequestFactory(currentPrice, o1, tradingData);

            OrderCancellationRequest request = factory.Make();

            Assert.IsTrue(request.Id > 0);
            Assert.AreEqual(o1, request.Order);
            Assert.AreEqual(o1.Id, request.OrderId);
            Assert.IsTrue(request.DateTime >= date);
            Assert.AreEqual(description, request.Description);
        }

        [TestMethod]
        public void make_request_for_limit_to_sell_order_when_current_price_near_stop_loss()
        {
            Signal s1 = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Trade t1 = new Trade(o1, this.strategyHeader.Portfolio, this.strategyHeader.Symbol, 150000, -3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ICollection<Trade>>().Add(t1);
            o1.FilledAmount = t1.Amount * -1;
            Assert.IsFalse(o1.IsFilled);
            Assert.IsTrue(o1.IsFilledPartially);

            double currentPrice = 150040;
            DateTime date = BrokerDateTime.Make(DateTime.Now);
            string description = String.Format("Текущая цена {0} на расстоянии одного шага от stop loss цены {1} стратегии.", currentPrice, s1.Limit + this.slSettings.Points);

            IGenericFactory<OrderCancellationRequest> factory = new UnfilledOrderCancellationRequestFactory(currentPrice, o1, tradingData);

            OrderCancellationRequest request = factory.Make();

            Assert.IsTrue(request.Id > 0);
            Assert.AreEqual(o1, request.Order);
            Assert.AreEqual(o1.Id, request.OrderId);
            Assert.IsTrue(request.DateTime >= date);
            Assert.AreEqual(description, request.Description);
        }

        [TestMethod]
        public void make_request_for_unfilled_limit_to_sell_order_when_current_price_near_take_profit()
        {
            Signal s1 = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Assert.IsFalse(o1.IsFilled);
            Assert.IsFalse(o1.IsFilledPartially);

            double currentPrice = 149930;
            DateTime date = BrokerDateTime.Make(DateTime.Now);
            string description = String.Format("Текущая цена {0} на расстоянии одного шага от take profit цены {1} стратегии.", currentPrice, s1.Limit - this.tpSettings.Points);

            IGenericFactory<OrderCancellationRequest> factory = new UnfilledOrderCancellationRequestFactory(currentPrice, o1, tradingData);

            OrderCancellationRequest request = factory.Make();

            Assert.IsTrue(request.Id > 0);
            Assert.AreEqual(o1, request.Order);
            Assert.AreEqual(o1.Id, request.OrderId);
            Assert.IsTrue(request.DateTime >= date);
            Assert.AreEqual(description, request.Description);
        }

        [TestMethod]
        public void make_request_for_unfilled_limit_to_sell_order_when_current_price_near_stop_loss()
        {
            Signal s1 = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Assert.IsFalse(o1.IsFilled);
            Assert.IsFalse(o1.IsFilledPartially);

            double currentPrice = 150040;
            DateTime date = BrokerDateTime.Make(DateTime.Now);
            string description = String.Format("Текущая цена {0} на расстоянии одного шага от stop loss цены {1} стратегии.", currentPrice, s1.Limit + this.slSettings.Points);

            IGenericFactory<OrderCancellationRequest> factory = new UnfilledOrderCancellationRequestFactory(currentPrice, o1, tradingData);

            OrderCancellationRequest request = factory.Make();

            Assert.IsTrue(request.Id > 0);
            Assert.AreEqual(o1, request.Order);
            Assert.AreEqual(o1.Id, request.OrderId);
            Assert.IsTrue(request.DateTime >= date);
            Assert.AreEqual(description, request.Description);
        }

        [TestMethod]
        public void do_nothing_if_order_is_filled()
        {
            Signal s1 = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(s1);

            Order o1 = new Order(s1);
            this.tradingData.Get<ICollection<Order>>().Add(o1);

            Trade t1 = new Trade(o1, this.strategyHeader.Portfolio, this.strategyHeader.Symbol, 150000, -10, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ICollection<Trade>>().Add(t1);
            o1.FilledAmount = t1.Amount * -1;
            Assert.IsTrue(o1.IsFilled);
            Assert.IsFalse(o1.IsFilledPartially);

            double currentPrice = 150040;
            DateTime date = BrokerDateTime.Make(DateTime.Now);
            string description = String.Format("Текущая цена {0} на расстоянии одного шага от stop loss цены {1} стратегии.", currentPrice, s1.Limit + this.slSettings.Points);

            IGenericFactory<OrderCancellationRequest> factory = new UnfilledOrderCancellationRequestFactory(currentPrice, o1, tradingData);

            Assert.IsNull(factory.Make());
        }
    }
}
