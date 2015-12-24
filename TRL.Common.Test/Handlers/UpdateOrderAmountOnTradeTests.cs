using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Emulation;
using TRL.Common.Data;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common.TimeHelpers;
using TRL.Common.Collections;
using TRL.Common.Handlers;
using TRL.Logging;

namespace TRL.Common.Handlers.Test
{
    [TestClass]
    public class UpdateOrderAmountOnTradeTests
    {
        private IDataContext tradingData;
        private StrategyHeader strategyHeader;
        private Signal openSignal;
        private Order openOrder;
        private UpdateOrderAmountOnTrade handler;

        [TestInitialize]
        public void Handlers_Setup()
        {
            this.tradingData = new TradingDataContext();
            this.strategyHeader = new StrategyHeader(1, "Sample strategyHeader", "ST12345-RF-01", "RTS-9.14", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);
            this.handler =
                new UpdateOrderAmountOnTrade(this.tradingData, new NullLogger());
        }

        [TestMethod]
        public void Handlers_update_buy_order_with_single_Trade_test()
        {
            this.openSignal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 135000, 0, 0);
            this.openOrder = this.tradingData.AddSignalAndItsOrder(this.openSignal);
            Assert.IsFalse(this.openOrder.IsFilled);
            Assert.IsFalse(this.openOrder.IsFilledPartially);
            Assert.AreEqual(0, this.openOrder.FilledAmount);

            Trade openTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, 3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(openTrade);
            Assert.IsFalse(this.openOrder.IsFilled);
            Assert.IsTrue(this.openOrder.IsFilledPartially);
            Assert.AreEqual(openTrade.Amount, this.openOrder.FilledAmount);
        }

        [TestMethod]
        public void Handlers_update_sell_order_with_single_Trade_test()
        {
            this.openSignal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 135000, 0, 0);
            this.openOrder = this.tradingData.AddSignalAndItsOrder(this.openSignal);
            Assert.IsFalse(this.openOrder.IsFilled);
            Assert.IsFalse(this.openOrder.IsFilledPartially);
            Assert.AreEqual(0, this.openOrder.FilledAmount);

            Trade openTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, -3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(openTrade);
            Assert.IsFalse(this.openOrder.IsFilled);
            Assert.IsTrue(this.openOrder.IsFilledPartially);
            Assert.AreEqual(openTrade.AbsoluteAmount, this.openOrder.FilledAmount);
        }

        [TestMethod]
        public void Handlers_update_buy_order_with_multiple_Trades_test()
        {
            this.openSignal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 135000, 0, 0);
            this.openOrder = this.tradingData.AddSignalAndItsOrder(this.openSignal);
            Assert.IsFalse(this.openOrder.IsFilled);
            Assert.IsFalse(this.openOrder.IsFilledPartially);
            Assert.AreEqual(0, this.openOrder.FilledAmount);

            Trade firstTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, 3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(firstTrade);

            Trade secondTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, 3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(secondTrade);

            Trade thirdTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, 4, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(thirdTrade);

            Assert.IsTrue(this.openOrder.IsFilled);
            Assert.IsFalse(this.openOrder.IsFilledPartially);
            Assert.AreEqual(firstTrade.Amount + secondTrade.Amount + thirdTrade.Amount, this.openOrder.FilledAmount);
        }

        [TestMethod]
        public void Handlers_update_sell_order_with_multiple_Trades_test()
        {
            this.openSignal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 135000, 0, 0);
            this.openOrder = this.tradingData.AddSignalAndItsOrder(this.openSignal);
            Assert.IsFalse(this.openOrder.IsFilled);
            Assert.IsFalse(this.openOrder.IsFilledPartially);
            Assert.AreEqual(0, this.openOrder.FilledAmount);

            Trade firstTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, -3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(firstTrade);

            Trade secondTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, -3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(secondTrade);

            Trade thirdTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, -4, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(thirdTrade);

            Assert.IsTrue(this.openOrder.IsFilled);
            Assert.IsFalse(this.openOrder.IsFilledPartially);
            Assert.AreEqual(firstTrade.AbsoluteAmount + secondTrade.AbsoluteAmount + thirdTrade.AbsoluteAmount, this.openOrder.FilledAmount);
        }

        [TestMethod]
        public void Handlers_update_buy_order_with_multiple_Trades_with_overamount_test()
        {
            this.openSignal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 135000, 0, 0);
            this.openOrder = this.tradingData.AddSignalAndItsOrder(this.openSignal);
            Assert.IsFalse(this.openOrder.IsFilled);
            Assert.IsFalse(this.openOrder.IsFilledPartially);
            Assert.AreEqual(0, this.openOrder.FilledAmount);

            Trade firstTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, 3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(firstTrade);

            Trade secondTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, 3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(secondTrade);

            Trade thirdTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, 5, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(thirdTrade);

            Assert.IsFalse(this.openOrder.IsFilled);
            Assert.IsTrue(this.openOrder.IsFilledPartially);
            Assert.AreEqual(firstTrade.Amount + secondTrade.Amount, this.openOrder.FilledAmount);
        }

        [TestMethod]
        public void Handlers_update_sell_order_with_multiple_Trades_with_overamount_test()
        {
            this.openSignal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 135000, 0, 0);
            this.openOrder = this.tradingData.AddSignalAndItsOrder(this.openSignal);
            Assert.IsFalse(this.openOrder.IsFilled);
            Assert.IsFalse(this.openOrder.IsFilledPartially);
            Assert.AreEqual(0, this.openOrder.FilledAmount);

            Trade firstTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, -3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(firstTrade);

            Trade secondTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, -3, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(secondTrade);

            Trade thirdTrade = new Trade(this.openOrder, this.openOrder.Portfolio, this.openOrder.Symbol, this.openOrder.Price, -5, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(thirdTrade);

            Assert.IsFalse(this.openOrder.IsFilled);
            Assert.IsTrue(this.openOrder.IsFilledPartially);
            Assert.AreEqual(firstTrade.AbsoluteAmount + secondTrade.AbsoluteAmount, this.openOrder.FilledAmount);
        }
    }
}
