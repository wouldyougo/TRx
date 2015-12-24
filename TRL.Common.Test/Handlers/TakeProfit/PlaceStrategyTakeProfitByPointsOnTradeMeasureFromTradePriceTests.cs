using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common.Collections;
using System.Linq;
using TRL.Common.Handlers;
using TRL.Emulation;
using TRL.Common.Extensions.Data;
using TRL.Handlers.StopLoss;
using TRL.Handlers.TakeProfit;
using TRL.Logging;

namespace TRL.Common.Handlers.Test.TakeProfit
{
    [TestClass]
    public class PlaceStrategyTakeProfitByPointsOnTradeMeasureFromTradePriceTests
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private StrategyHeader strategyHeader;
        private ProfitPointsSettings ppSettings;
        private TakeProfitOrderSettings tpoSettings;

        private PlaceStrategyTakeProfitByPointsOnTrade handler;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();

            this.strategyHeader = new StrategyHeader(1, "Description", "ST12345-RF-01", "RTS-9.14", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.ppSettings = new ProfitPointsSettings(this.strategyHeader, 100, false);
            this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(this.ppSettings);

            this.tpoSettings = new TakeProfitOrderSettings(this.strategyHeader, 180);
            this.tradingData.Get<ICollection<TakeProfitOrderSettings>>().Add(this.tpoSettings);

            this.handler =
                new PlaceStrategyTakeProfitByPointsOnTrade(this.strategyHeader, this.tradingData, this.signalQueue, new NullLogger());

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Order>>().Count());
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void make_signal_to_sell_for_long_position_with_single_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price + 10);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Limit, closeSignal.OrderType);
            Assert.AreEqual(trade.Price + this.ppSettings.Points, closeSignal.Limit);
            Assert.AreEqual(trade.Price, closeSignal.Price);
            Assert.AreEqual(trade.Amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_sell_for_long_position_with_multiple_open_trades_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Limit, closeSignal.OrderType);
            Assert.AreEqual(secondTrade.Price + this.ppSettings.Points, closeSignal.Limit);
            Assert.AreEqual(secondTrade.Price, closeSignal.Price);
            Assert.AreEqual(amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_signal_to_sell_for_long_position_with_multiple_trades_and_cancelled_open_order_before_last_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            firstTrade.Order.Cancel(DateTime.Now, "cancel order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 3);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Sell, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Limit, closeSignal.OrderType);
            Assert.AreEqual(secondTrade.Price + this.ppSettings.Points, closeSignal.Limit);
            Assert.AreEqual(secondTrade.Price, closeSignal.Price);
            Assert.AreEqual(amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_no_signal_for_long_position_with_multiple_trades_and_cancelled_open_order_after_last_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 3);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));
            firstTrade.Order.Cancel(DateTime.Now, "cancel order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);
            Assert.AreEqual(0, this.signalQueue.Count);
        }


        [TestMethod]
        public void make_signal_for_short_position_with_single_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price + 10);
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Buy, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Limit, closeSignal.OrderType);
            Assert.AreEqual(trade.Price - this.ppSettings.Points, closeSignal.Limit);
            Assert.AreEqual(trade.Price, closeSignal.Price);
            Assert.AreEqual(Math.Abs(trade.Amount), closeSignal.Amount);
        }

        [TestMethod]
        public void make_signal_for_short_position_with_multiple_open_trades_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));

            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Buy, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Limit, closeSignal.OrderType);
            Assert.AreEqual(secondTrade.Price - this.ppSettings.Points, closeSignal.Limit);
            Assert.AreEqual(secondTrade.Price, closeSignal.Price);
            Assert.AreEqual(amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_signal_for_long_position_with_multiple_trades_and_cancelled_open_order_before_last_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            firstTrade.Order.Cancel(DateTime.Now, "cancel order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 3);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));
            Assert.AreEqual(1, this.signalQueue.Count);

            Signal closeSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(this.strategyHeader.Id, closeSignal.StrategyId);
            Assert.AreEqual(this.strategyHeader, closeSignal.Strategy);
            Assert.AreEqual(TradeAction.Buy, closeSignal.TradeAction);
            Assert.AreEqual(OrderType.Limit, closeSignal.OrderType);
            Assert.AreEqual(secondTrade.Price - this.ppSettings.Points, closeSignal.Limit);
            Assert.AreEqual(secondTrade.Price, closeSignal.Price);
            Assert.AreEqual(amount, closeSignal.Amount);
        }

        [TestMethod]
        public void make_no_signal_for_short_position_with_multiple_trades_and_cancelled_open_order_after_last_trade_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 125000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 5);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, openSignal.Price, 3);
            double amount = Math.Abs(this.tradingData.GetAmount(this.strategyHeader));
            firstTrade.Order.Cancel(DateTime.Now, "cancel order");
            Assert.IsTrue(firstTrade.Order.IsCanceled);
            Assert.AreEqual(0, this.signalQueue.Count);
        }

    }
}
