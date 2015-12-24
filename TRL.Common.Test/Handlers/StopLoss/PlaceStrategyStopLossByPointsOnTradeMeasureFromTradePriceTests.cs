using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Emulation;
using TRL.Handlers.StopLoss;
using TRL.Common.Extensions.Data;
using TRL.Logging;

namespace TRL.Common.Handlers.Test.StopLoss
{
    [TestClass]
    public class PlaceStrategyStopLossByPointsOnTradeMeasureFromTradePriceTests
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private StrategyHeader strategyHeader;
        private StopPointsSettings spSettings;
        private StopLossOrderSettings slSettings;

        private PlaceStrategyStopLossByPointsOnTrade handler;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();

            this.strategyHeader = new StrategyHeader(1, "Description", "ST12345-RF-01", "RTS-9.14", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.spSettings = new StopPointsSettings(this.strategyHeader, 100, false);
            this.tradingData.Get<ICollection<StopPointsSettings>>().Add(this.spSettings);

            this.slSettings = new StopLossOrderSettings(this.strategyHeader, 180);
            this.tradingData.Get<ICollection<StopLossOrderSettings>>().Add(this.slSettings);

            this.handler =
                new PlaceStrategyStopLossByPointsOnTrade(this.strategyHeader, this.tradingData, this.signalQueue, new NullLogger());

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void PlaceStrategyStopLossByPointsOnTrade_is_Identified_test()
        {
            Assert.IsInstanceOfType(this.handler, typeof(IIdentified));
            Assert.AreEqual(this.strategyHeader.Id, this.handler.Id);
        }

        private Trade MakeTrade()
        {
            Trade trade = new Trade();
            trade.Id = 1;
            trade.Portfolio = "ST12345-RF-01";
            trade.Symbol = "RTS-9.14";
            trade.Amount = 9;
            trade.Price = 120000;
            return trade;
        }

        [TestMethod]
        public void do_nothing_when_here_is_no_signal_and_order_for_trade_test()
        {
            Trade trade = MakeTrade();

            this.tradingData.Get<ObservableHashSet<Trade>>().Add(trade);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_signal_to_sell_on_stop_for_long_position_with_single_open_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Stop, closeSignal.OrderType);
            Assert.AreEqual(trade.Price, closeSignal.Price);
            Assert.AreEqual(trade.Price - this.spSettings.Points, closeSignal.Stop);
            Assert.AreEqual(trade.Amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_sell_on_stop_for_long_position_with_multiple_open_trades_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Assert.AreEqual(0, this.signalQueue.Count);
            
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Stop, closeSignal.OrderType);
            Assert.AreEqual(firstTrade.Price, closeSignal.Price);
            Assert.AreEqual(firstTrade.Price - this.spSettings.Points, closeSignal.Stop);
            Assert.AreEqual(firstTrade.Amount + secondTrade.Amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_stop_for_long_with_multiple_open_trades_when_order_is_canceled_before_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Assert.AreEqual(0, this.signalQueue.Count);

            firstTrade.Order.Cancel(DateTime.Now, "cancel order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);

            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price + 10, 3);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Stop, closeSignal.OrderType);
            Assert.AreEqual(secondTrade.Price, closeSignal.Price);
            Assert.AreEqual(secondTrade.Price - this.spSettings.Points, closeSignal.Stop);
            Assert.AreEqual(amount, closeSignal.Amount);
        }


        [TestMethod]
        public void make_no_signal_for_long_with_multiple_open_trades_when_order_is_canceled_after_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Assert.AreEqual(0, this.signalQueue.Count);

            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 3);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));
            firstTrade.Order.Cancel(DateTime.Now, "cancel order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_for_long_when_unfilled_stop_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);
            Assert.AreEqual(1, this.signalQueue.Count);

            Order stopOrder = this.tradingData.AddSignalAndItsOrder(this.signalQueue.Dequeue());

            Signal reBuySignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(reBuySignal);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_for_long_when_unfilled_market_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);
            Assert.AreEqual(1, this.signalQueue.Count);
            this.signalQueue.Dequeue();

            Signal marketSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            this.tradingData.AddSignalAndItsOrder(marketSignal);

            Signal reBuySignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(reBuySignal);
            Assert.AreEqual(0, this.signalQueue.Count);
        }


        [TestMethod]
        public void make_signal_to_buy_on_stop_for_short_position_with_single_open_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Buy, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Stop, closeSignal.OrderType);
            Assert.AreEqual(trade.Price, closeSignal.Price);
            Assert.AreEqual(trade.Price + this.spSettings.Points, closeSignal.Stop);
            Assert.AreEqual(Math.Abs(trade.Amount), closeSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_buy_on_stop_for_short_position_with_multiple_open_trades_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Assert.AreEqual(0, this.signalQueue.Count);

            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price - 10, 5);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Buy, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Stop, closeSignal.OrderType);
            Assert.AreEqual(secondTrade.Price, closeSignal.Price);
            Assert.AreEqual(secondTrade.Price + this.spSettings.Points, closeSignal.Stop);
            Assert.AreEqual(Math.Abs(firstTrade.Amount + secondTrade.Amount), closeSignal.Amount);
        }

        [TestMethod]
        public void make_stop_for_short_with_multiple_open_trades_when_order_is_canceled_before_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Assert.AreEqual(0, this.signalQueue.Count);

            firstTrade.Order.Cancel(DateTime.Now, "cancel order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);

            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price - 10, 3);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Buy, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Stop, closeSignal.OrderType);
            Assert.AreEqual(secondTrade.Price, closeSignal.Price);
            Assert.AreEqual(secondTrade.Price + this.spSettings.Points, closeSignal.Stop);
            Assert.AreEqual(amount, closeSignal.Amount);
        }


        [TestMethod]
        public void make_no_signal_for_short_with_multiple_open_trades_when_order_is_canceled_after_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Assert.AreEqual(0, this.signalQueue.Count);

            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 3);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));
            firstTrade.Order.Cancel(DateTime.Now, "cancel order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_for_short_when_unfilled_stop_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);
            Assert.AreEqual(1, this.signalQueue.Count);

            Order stopOrder = this.tradingData.AddSignalAndItsOrder(this.signalQueue.Dequeue());

            Signal reBuySignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(reBuySignal);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_no_signal_for_short_when_unfilled_market_order_exists_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);
            Assert.AreEqual(1, this.signalQueue.Count);
            this.signalQueue.Dequeue();

            Signal marketSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            this.tradingData.AddSignalAndItsOrder(marketSignal);

            Signal reBuySignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(reBuySignal);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

    }
}
