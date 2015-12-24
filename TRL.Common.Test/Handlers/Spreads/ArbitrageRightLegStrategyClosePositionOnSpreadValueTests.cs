using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Collections;
using System.Collections.Generic;
using TRL.Handlers.Spreads;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Logging;

namespace TRL.Common.Handlers.Test.Spreads
{
    [TestClass]
    public class ArbitrageRightLegStrategyClosePositionOnSpreadValueTests
    {
        private ArbitrageSettings arbitrageSettings;
        private StrategyHeader lStrategy, rStrategy;
        private List<StrategyHeader> lLeg, rLeg;
        private ObservableQueue<Signal> signalQueue;
        private IDataContext tradingData;
        private StrategyVolumeChangeStep rStrategyVolumeChangeStep;

        [TestInitialize]
        public void Setup()
        {
            this.signalQueue = new ObservableQueue<Signal>();
            this.tradingData = new TradingDataContext();

            this.lLeg = new List<StrategyHeader>();
            this.rLeg = new List<StrategyHeader>();

            this.lStrategy = new StrategyHeader(1, "Left leg strategyHeader", "BP12345-RF-01", "SBRF-3.14_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(lStrategy);
            this.lLeg.Add(this.lStrategy);

            this.rStrategy = new StrategyHeader(2, "Right leg strategyHeader", "BP12345-RF-01", "SBPR-3.13_FT", 11);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(rStrategy);
            this.rLeg.Add(this.rStrategy);

            this.arbitrageSettings = new ArbitrageSettings(1, this.lLeg, this.rLeg, new SpreadSettings(1.12, 1.25, 1.01));
            this.tradingData.Get<ICollection<ArbitrageSettings>>().Add(this.arbitrageSettings);

            this.rStrategyVolumeChangeStep = new StrategyVolumeChangeStep(this.rStrategy, 2);
            this.tradingData.Get<ICollection<StrategyVolumeChangeStep>>().Add(this.rStrategyVolumeChangeStep);

            ArbitrageClosePositionOnSpreadValue handler =
                new ArbitrageClosePositionOnSpreadValue(this.arbitrageSettings, this.rStrategy, this.tradingData, this.signalQueue, new NullLogger());
        }

        [TestMethod]
        public void Make_signal_to_close_short_position_test()
        {
            Signal openSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 1.25, 0, 0);
            openSignal.Amount = this.rStrategy.Amount;
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Order openOrder = new Order(openSignal);
            this.tradingData.Get<ICollection<Order>>().Add(openOrder);

            Trade trade = new Trade(openOrder, openOrder.Portfolio, openOrder.Symbol, openOrder.Price, -openOrder.Amount, DateTime.Now);
            this.tradingData.Get<ICollection<Trade>>().Add(trade);
            openOrder.FilledAmount = openOrder.Amount;

            Assert.AreEqual(-11, this.tradingData.GetAmount(this.rStrategy));

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.12, 1.09));

            Assert.AreEqual(1, this.signalQueue.Count);

            Signal signal = this.signalQueue.Dequeue();

            Assert.AreEqual(TradeAction.Buy, signal.TradeAction);
            Assert.AreEqual(OrderType.Market, signal.OrderType);
            Assert.AreEqual(1.12, signal.Price);
            Assert.AreEqual(this.rStrategyVolumeChangeStep.Amount, signal.Amount);
        }

        [TestMethod]
        public void Make_signal_to_close_long_position_test()
        {
            Signal openSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 1.25, 0, 0);
            openSignal.Amount = this.rStrategy.Amount;
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Order openOrder = new Order(openSignal);
            this.tradingData.Get<ICollection<Order>>().Add(openOrder);

            Trade trade = new Trade(openOrder, openOrder.Portfolio, openOrder.Symbol, openOrder.Price, openOrder.Amount, DateTime.Now);
            this.tradingData.Get<ICollection<Trade>>().Add(trade);
            openOrder.FilledAmount = openOrder.Amount;

            Assert.AreEqual(11, this.tradingData.GetAmount(this.rStrategy));

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.15, 1.12));

            Assert.AreEqual(1, this.signalQueue.Count);

            Signal signal = this.signalQueue.Dequeue();

            Assert.AreEqual(TradeAction.Sell, signal.TradeAction);
            Assert.AreEqual(OrderType.Market, signal.OrderType);
            Assert.AreEqual(1.12, signal.Price);
            Assert.AreEqual(this.rStrategyVolumeChangeStep.Amount, signal.Amount);
        }

        [TestMethod]
        public void Make_Signal_To_Do_Nothing()
        {
            Assert.AreEqual(0, this.signalQueue.Count);

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.15, 1.02));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Ignore_fair_price_when_no_long_position_exists_test()
        {
            Assert.AreEqual(0, this.tradingData.GetAmount(this.rStrategy));
            Assert.AreEqual(0, this.signalQueue.Count);

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.15, 1.12));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Ignore_fair_price_when_no_short_position_exists_test()
        {
            Assert.AreEqual(0, this.tradingData.GetAmount(this.rStrategy));
            Assert.AreEqual(0, this.signalQueue.Count);

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.12, 1.10));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Ignore_SpreadValue_if_unfilled_order_for_long_position_exists_test()
        {
            Signal openSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 1.25, 0, 0);
            openSignal.Amount = this.rStrategy.Amount;
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Order openOrder = new Order(openSignal);
            this.tradingData.Get<ICollection<Order>>().Add(openOrder);

            Trade trade = new Trade(openOrder, openOrder.Portfolio, openOrder.Symbol, openOrder.Price, openOrder.Amount, DateTime.Now);
            this.tradingData.Get<ICollection<Trade>>().Add(trade);
            openOrder.FilledAmount = openOrder.Amount;

            Signal closeSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 1.25, 0, 0);
            closeSignal.Amount = this.rStrategy.Amount;
            this.tradingData.Get<ICollection<Signal>>().Add(closeSignal);

            Order closeOrder = new Order(closeSignal);
            this.tradingData.Get<ICollection<Order>>().Add(closeOrder);

            Assert.AreEqual(11, this.tradingData.GetAmount(this.rStrategy));
            Assert.AreEqual(0, this.signalQueue.Count);

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.12, 1.10));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Ignore_SpreadValue_if_unfilled_order_for_short_position_exists_test()
        {
            Signal openSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 1.25, 0, 0);
            openSignal.Amount = this.rStrategy.Amount;
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Order openOrder = new Order(openSignal);
            this.tradingData.Get<ICollection<Order>>().Add(openOrder);

            Trade trade = new Trade(openOrder, openOrder.Portfolio, openOrder.Symbol, openOrder.Price, -openOrder.Amount, DateTime.Now);
            this.tradingData.Get<ICollection<Trade>>().Add(trade);
            openOrder.FilledAmount = openOrder.Amount;

            Signal closeSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 1.25, 0, 0);
            closeSignal.Amount = this.rStrategy.Amount;
            this.tradingData.Get<ICollection<Signal>>().Add(closeSignal);

            Order closeOrder = new Order(closeSignal);
            this.tradingData.Get<ICollection<Order>>().Add(closeOrder);

            Assert.AreEqual(-11, this.tradingData.GetAmount(this.rStrategy));
            Assert.AreEqual(0, this.signalQueue.Count);

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.12, 1.10));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Make_signal_to_close_short_position_tail_test()
        {
            Signal openSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 1.25, 0, 0);
            openSignal.Amount = 1;
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Order openOrder = new Order(openSignal);
            this.tradingData.Get<ICollection<Order>>().Add(openOrder);

            Trade trade = new Trade(openOrder, openOrder.Portfolio, openOrder.Symbol, openOrder.Price, -openOrder.Amount, DateTime.Now);
            this.tradingData.Get<ICollection<Trade>>().Add(trade);
            openOrder.FilledAmount = openOrder.Amount;

            Assert.AreEqual(-1, this.tradingData.GetAmount(this.rStrategy));
            Assert.AreEqual(0, this.signalQueue.Count);

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.12, 1.09));

            Assert.AreEqual(1, this.signalQueue.Count);
            Signal signal = this.signalQueue.Dequeue();
            Assert.AreEqual(TradeAction.Buy, signal.TradeAction);
            Assert.AreEqual(OrderType.Market, signal.OrderType);
            Assert.AreEqual(1, signal.Amount);
            Assert.AreEqual(1.12, signal.Price);
        }

        [TestMethod]
        public void Make_signal_to_close_long_position_tail_test()
        {
            Signal openSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 1.25, 0, 0);
            openSignal.Amount = 1;
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Order openOrder = new Order(openSignal);
            this.tradingData.Get<ICollection<Order>>().Add(openOrder);

            Trade trade = new Trade(openOrder, openOrder.Portfolio, openOrder.Symbol, openOrder.Price, openOrder.Amount, DateTime.Now);
            this.tradingData.Get<ICollection<Trade>>().Add(trade);
            openOrder.FilledAmount = openOrder.Amount;

            Assert.AreEqual(1, this.tradingData.GetAmount(this.rStrategy));
            Assert.AreEqual(0, this.signalQueue.Count);

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.15, 1.12));

            Assert.AreEqual(1, this.signalQueue.Count);
            Signal signal = this.signalQueue.Dequeue();
            Assert.AreEqual(TradeAction.Sell, signal.TradeAction);
            Assert.AreEqual(OrderType.Market, signal.OrderType);
            Assert.AreEqual(1, signal.Amount);
            Assert.AreEqual(1.12, signal.Price);
        }
    }
}
