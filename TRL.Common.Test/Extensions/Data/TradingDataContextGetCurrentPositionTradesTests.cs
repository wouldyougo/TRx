using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Emulation;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataContextGetCurrentPositionTradesTests
    {
        private IDataContext tradingData;
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-3.14_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);
        }

        [TestMethod]
        public void GetCurrentPositionTrades_for_first_long_position_test()
        {
            Signal firstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal);

            ICollection<Trade> trades = this.tradingData.GetPositionTrades(this.strategyHeader);
            Assert.AreEqual(1, trades.Count);
            Assert.AreEqual(firstTrade.Id, trades.Last().Id);
        }

        [TestMethod]
        public void GetCurrentPositionTrades_for_first_long_position_with_multiple_trades_test()
        {
            Signal firstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 3);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 5);
            Trade thirdTrade = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 2);

            ICollection<Trade> trades = this.tradingData.GetPositionTrades(this.strategyHeader);
            Assert.AreEqual(3, trades.Count);
            Assert.AreEqual(firstTrade.Id, trades.Last().Id);
            Assert.AreEqual(secondTrade.Id, trades.ElementAt(1).Id);
            Assert.AreEqual(thirdTrade.Id, trades.First().Id);
        }

        [TestMethod]
        public void GetCurrentPositionTrades_for_first_short_position_test()
        {
            Signal firstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 140000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal);

            ICollection<Trade> trades = this.tradingData.GetPositionTrades(this.strategyHeader);
            Assert.AreEqual(1, trades.Count);
            Assert.AreEqual(firstTrade.Id, trades.Last().Id);
        }

        [TestMethod]
        public void GetCurrentPositionTrades_for_first_short_position_with_multiple_trades_test()
        {
            Signal firstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 140000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 1);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 3);
            Trade thirdTrade = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 6);

            ICollection<Trade> trades = this.tradingData.GetPositionTrades(this.strategyHeader);
            Assert.AreEqual(3, trades.Count);
            Assert.AreEqual(firstTrade.Id, trades.Last().Id);
            Assert.AreEqual(secondTrade.Id, trades.ElementAt(1).Id);
            Assert.AreEqual(thirdTrade.Id, trades.First().Id);
        }

        [TestMethod]
        public void GetCurrentPositionTrades_for_second_long_position_test()
        {
            Signal firstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal);

            Signal closeFirstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 141000, 0, 0);
            Trade closeFirstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(closeFirstSignal);

            Signal secondSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 141000, 0, 0);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal);

            ICollection<Trade> trades = this.tradingData.GetPositionTrades(this.strategyHeader);
            Assert.AreEqual(1, trades.Count);
            Assert.AreEqual(secondTrade.Id, trades.Last().Id);
        }

        [TestMethod]
        public void GetCurrentPositionTrades_for_second_short_position_test()
        {
            Signal firstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 140000, 0, 0);
            Trade firstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal);

            Signal closeFirstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 141000, 0, 0);
            Trade closeFirstTrade = this.tradingData.AddSignalAndItsOrderAndTrade(closeFirstSignal);

            Signal secondSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 141000, 0, 0);
            Trade secondTrade = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal);

            ICollection<Trade> trades = this.tradingData.GetPositionTrades(this.strategyHeader);
            Assert.AreEqual(1, trades.Count);
            Assert.AreEqual(secondTrade.Id, trades.Last().Id);
        }

        [TestMethod]
        public void GetCurrentPositionTrades_for_second_long_position_with_multiple_trades_test()
        {
            Signal firstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 4);
            this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 6);

            Signal closeFirstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 141000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeFirstSignal);

            Signal secondSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 141000, 0, 0);
            Trade t1 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 7);
            Trade t2 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 1);
            Trade t3 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 1);
            Trade t4 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 1);

            ICollection<Trade> trades = this.tradingData.GetPositionTrades(this.strategyHeader);
            Assert.AreEqual(4, trades.Count);
            Assert.AreEqual(t1.Id, trades.Last().Id);
            Assert.AreEqual(t2.Id, trades.ElementAt(2).Id);
            Assert.AreEqual(t3.Id, trades.ElementAt(1).Id);
            Assert.AreEqual(t4.Id, trades.First().Id);
        }

        [TestMethod]
        public void GetCurrentPositionTrades_for_second_short_position_with_multiple_trades_test()
        {
            Signal firstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 1);
            this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 4);
            this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 5);

            Signal closeFirstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 141000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeFirstSignal);

            Signal secondSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 141000, 0, 0);
            Trade t1 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 2);
            Trade t2 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 2);
            Trade t3 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 3);
            Trade t4 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 1);
            Trade t5 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 2);

            ICollection<Trade> trades = this.tradingData.GetPositionTrades(this.strategyHeader);
            Assert.AreEqual(5, trades.Count);
            Assert.AreEqual(t1.Id, trades.Last().Id);
            Assert.AreEqual(t2.Id, trades.ElementAt(3).Id);
            Assert.AreEqual(t3.Id, trades.ElementAt(2).Id);
            Assert.AreEqual(t4.Id, trades.ElementAt(1).Id);
            Assert.AreEqual(t5.Id, trades.First().Id);
        }

        [TestMethod]
        public void GetCurrentPositionTrades_for_second_long_position_with_multiple_trades_close_partially_test()
        {
            Signal firstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 4);
            Trade t0 = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 6);

            Signal closeFirstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 141000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeFirstSignal, 141000, 8);

            Signal secondSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 141000, 0, 0);
            Trade t1 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 7);
            Trade t2 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 1);
            Trade t3 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 1);
            Trade t4 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 1);

            ICollection<Trade> trades = this.tradingData.GetPositionTrades(this.strategyHeader);
            Assert.AreEqual(5, trades.Count);
            Assert.AreEqual(t0.Id, trades.Last().Id);
            Assert.AreEqual(2, trades.Last().Amount);
            Assert.AreEqual(t1.Id, trades.ElementAt(3).Id);
            Assert.AreEqual(t2.Id, trades.ElementAt(2).Id);
            Assert.AreEqual(t3.Id, trades.ElementAt(1).Id);
            Assert.AreEqual(t4.Id, trades.First().Id);
        }

        [TestMethod]
        public void GetCurrentPositionTrades_for_second_short_position_with_multiple_trades_close_partially_test()
        {
            Signal firstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 140000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 1);
            Trade tp = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 4);
            Trade t0 = this.tradingData.AddSignalAndItsOrderAndTrade(firstSignal, 140000, 5);

            Signal closeFirstSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 141000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeFirstSignal, 141000, 3);

            Signal secondSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 141000, 0, 0);
            Trade t1 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 2);
            Trade t2 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 2);
            Trade t3 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 3);
            Trade t4 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 1);
            Trade t5 = this.tradingData.AddSignalAndItsOrderAndTrade(secondSignal, 141000, 2);

            ICollection<Trade> trades = this.tradingData.GetPositionTrades(this.strategyHeader);
            Assert.AreEqual(7, trades.Count);
            Assert.AreEqual(tp.Id, trades.Last().Id);
            Assert.AreEqual(-2, trades.Last().Amount);
            Assert.AreEqual(t0.Id, trades.ElementAt(5).Id);
            Assert.AreEqual(t1.Id, trades.ElementAt(4).Id);
            Assert.AreEqual(t2.Id, trades.ElementAt(3).Id);
            Assert.AreEqual(t3.Id, trades.ElementAt(2).Id);
            Assert.AreEqual(t4.Id, trades.ElementAt(1).Id);
            Assert.AreEqual(t5.Id, trades.First().Id);
        }

    }
}
