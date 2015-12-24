using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using TRL.Common.Collections;
using TRL.Common.Extensions.Data;
using TRL.Emulation;
using TRL.Handlers.StopLoss;
using TRL.Handlers.TakeProfit;
using TRL.Logging;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataContextGetMoveRequestsTests:TraderBaseInitializer
    {
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);

            StopPointsSettings spSettings = new StopPointsSettings(this.strategyHeader, 100, false);
            this.tradingData.Get<ObservableHashSet<StopPointsSettings>>().Add(spSettings);

            StopLossOrderSettings slSettings = new StopLossOrderSettings(this.strategyHeader, 3600);
            this.tradingData.Get<ObservableHashSet<StopLossOrderSettings>>().Add(slSettings);

            StrategyStopLossByPointsOnTick stopLossHandler =
                new StrategyStopLossByPointsOnTick(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());
            StrategyTakeProfitByPointsOnTick takeProfitHandler =
                new StrategyTakeProfitByPointsOnTick(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());

            PlaceStrategyStopLossByPointsOnTrade placeStopOnTradeHandler =
                new PlaceStrategyStopLossByPointsOnTrade(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());
            PlaceStrategyTakeProfitByPointsOnTrade placeTakeProfitOnTradeHandler =
                new PlaceStrategyTakeProfitByPointsOnTrade(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());

        }

        [TestMethod]
        public void TradingDataContext_GetMoveRequest_returns_empty_collection_when_no_any_requests_exists_test()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Assert.AreEqual(2, this.tradingData.Get<IEnumerable<Signal>>().Count());

            Order slOrder = this.tradingData.GetCloseOrders(this.strategyHeader).Last();

            Assert.AreEqual(0, this.tradingData.GetMoveRequests(slOrder).Count());
        }

        [TestMethod]
        public void TradingDataContext_GetMoveRequests_for_order_test()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Assert.AreEqual(2, this.tradingData.Get<IEnumerable<Signal>>().Count());

            Order slOrder = this.tradingData.GetCloseOrders(this.strategyHeader).Last();
            Assert.AreEqual(OrderType.Stop, slOrder.OrderType);
            Assert.AreEqual(149900, slOrder.Stop);

            OrderMoveRequest slMoveReq = new OrderMoveRequest(slOrder, 0, 150000, "Trail stop");
            this.tradingData.Get<ObservableCollection<OrderMoveRequest>>().Add(slMoveReq);

            OrderMoveRequest slMoveReq2 = new OrderMoveRequest(slOrder, 0, 150100, "Trail stop");
            this.tradingData.Get<ObservableCollection<OrderMoveRequest>>().Add(slMoveReq2);

            Assert.AreEqual(2, this.tradingData.GetMoveRequests(slOrder).Count());
        }

        [TestMethod]
        public void TradingDataContext_GetMoveRequests_ignore_other_orders_test()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(signal);

            Assert.AreEqual(2, this.tradingData.Get<IEnumerable<Signal>>().Count());

            Order slOrder = this.tradingData.GetCloseOrders(this.strategyHeader).Last();
            Assert.AreEqual(OrderType.Stop, slOrder.OrderType);
            Assert.AreEqual(149900, slOrder.Stop);

            OrderMoveRequest slMoveReq = new OrderMoveRequest(slOrder, 0, 150000, "Trail stop");
            this.tradingData.Get<ObservableCollection<OrderMoveRequest>>().Add(slMoveReq);

            OrderMoveRequest slMoveReq2 = new OrderMoveRequest(slOrder, 0, 150100, "Trail stop");
            this.tradingData.Get<ObservableCollection<OrderMoveRequest>>().Add(slMoveReq2);

            Signal tpSignal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 151000, 0, 151000);
            this.signalQueue.Enqueue(tpSignal);

            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Order>>().Count());

            Order tpOrder = this.tradingData.Get<IEnumerable<Order>>().Last();
            Assert.AreEqual(OrderType.Limit, tpOrder.OrderType);

            OrderMoveRequest tpMoveReq = new OrderMoveRequest(tpOrder, 152000, 0, "Trail profit");
            this.tradingData.Get<ObservableCollection<OrderMoveRequest>>().Add(tpMoveReq);

            Assert.AreEqual(2, this.tradingData.GetMoveRequests(slOrder).Count());
        }
    }
}
