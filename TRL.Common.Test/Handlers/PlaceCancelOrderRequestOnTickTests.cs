using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Handlers;
using TRL.Common.Models;
using TRL.Logging;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Handlers.Test
{
    [TestClass]
    public class PlaceCancelOrderRequestOnTickTests
    {
        private IDataContext tradingData;
        private StrategyHeader str1, str2, str3;
        private Signal s1, s2, s3;

        [TestInitialize]
        public void Handlers_Setup()
        {
            this.tradingData = new TradingDataContext();

            PlaceCancelOrderRequestOnTick handler = new PlaceCancelOrderRequestOnTick(this.tradingData, new NullLogger());

            this.tradingData.Get<ICollection<Symbol>>().Add(new Symbol("RTS-9.13_FT", 1, 8, 10, BrokerDateTime.Make(DateTime.Now)));

            this.str1 = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.str1);

            this.str2 = new StrategyHeader(2, "Strategy 2", "BP12345-RF-01", "Si-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.str2);

            this.str3 = new StrategyHeader(3, "Strategy 3", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.str3);

            this.s1 = new Signal(this.str1, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150100);
            this.tradingData.Get<ObservableHashSet<Signal>>().Add(this.s1);

            this.s2 = new Signal(this.str2, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 33000, 0, 0);
            this.tradingData.Get<ObservableHashSet<Signal>>().Add(this.s2);

            this.s3 = new Signal(this.str3, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150100, 0, 150200);
            this.tradingData.Get<ObservableHashSet<Signal>>().Add(this.s3);

            StopPointsSettings sls1 = new StopPointsSettings(this.str1, 50, false);
            this.tradingData.Get<ICollection<StopPointsSettings>>().Add(sls1);

            StopPointsSettings sls2 = new StopPointsSettings(this.str2, 10, false);
            this.tradingData.Get<ICollection<StopPointsSettings>>().Add(sls2);

            StopPointsSettings sls3 = new StopPointsSettings(this.str3, 35, false);
            this.tradingData.Get<ICollection<StopPointsSettings>>().Add(sls3);

            ProfitPointsSettings tps1 = new ProfitPointsSettings(this.str1, 80, false);
            this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(tps1);

            ProfitPointsSettings tps2 = new ProfitPointsSettings(this.str2, 20, false);
            this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(tps2);

            ProfitPointsSettings tps3 = new ProfitPointsSettings(this.str3, 50, false);
            this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(tps3);
        }

        [TestMethod]
        public void Handlers_do_nothing_when_no_unfilled_orders_exists()
        {
            Order o1 = new Order(this.s1);
            this.tradingData.Get<ObservableHashSet<Order>>().Add(o1);

            Trade t1 = new Trade(o1, this.str1.Portfolio, this.str1.Symbol, 150100, this.str1.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t1);
            o1.FilledAmount = t1.Amount;
            Assert.IsTrue(o1.IsFilled);
            Assert.IsFalse(o1.IsFilledPartially);

            Order o2 = new Order(this.s2);
            this.tradingData.Get<ObservableHashSet<Order>>().Add(o2);

            Trade t2 = new Trade(o2, this.str2.Portfolio, this.str2.Symbol, 33100, this.str2.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t2);
            o2.FilledAmount = t2.Amount;
            Assert.IsTrue(o2.IsFilled);
            Assert.IsFalse(o2.IsFilledPartially);

            Order o3 = new Order(this.s3);
            this.tradingData.Get<ObservableHashSet<Order>>().Add(o3);

            Trade t3 = new Trade(o3, this.str3.Portfolio, this.str3.Symbol, 150100, this.str3.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t3);
            o3.FilledAmount = t3.Amount;
            Assert.IsTrue(o3.IsFilled);
            Assert.IsFalse(o3.IsFilledPartially);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<OrderCancellationRequest>>().Count());

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = BrokerDateTime.Make(DateTime.Now), Symbol = "RTS-9.13_FT", Price = 150200, Volume = 50, TradeAction = TradeAction.Buy });

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<OrderCancellationRequest>>().Count());
        }

        [TestMethod]
        public void Handlers_cancel_order_when_tick_price_near_stop()
        {
            Order o1 = new Order(this.s1);
            this.tradingData.Get<ObservableHashSet<Order>>().Add(o1);

            Trade t1 = new Trade(o1, this.str1.Portfolio, this.str1.Symbol, 150100, 3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(t1);
            o1.FilledAmount = t1.Amount;
            Assert.IsFalse(o1.IsFilled);
            Assert.IsTrue(o1.IsFilledPartially);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<OrderCancellationRequest>>().Count());

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { DateTime = BrokerDateTime.Make(DateTime.Now), Symbol = "RTS-9.13_FT", Price = 150060, Volume = 50, TradeAction = TradeAction.Buy });

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OrderCancellationRequest>>().Count());
        }
    }
}
