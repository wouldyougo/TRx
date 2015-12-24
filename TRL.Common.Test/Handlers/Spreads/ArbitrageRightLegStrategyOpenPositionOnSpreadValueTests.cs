using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Handlers.Spreads;
using TRL.Common.Extensions.Data;
using TRL.Logging;

namespace TRL.Common.Handlers.Test.Spreads
{
    [TestClass]
    public class ArbitrageRightLegStrategyOpenPositionOnSpreadValueTests
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

            ArbitrageOpenPositionOnSpreadValue handler =
                new ArbitrageOpenPositionOnSpreadValue(this.arbitrageSettings, this.rStrategy, this.tradingData, this.signalQueue, new NullLogger());
        }

        [TestMethod]
        public void Make_Signal_To_Buy_Spread_RightLeg_test()
        {
            Assert.AreEqual(0, this.signalQueue.Count);

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.251, 1.24));

            Assert.AreEqual(1, this.signalQueue.Count);

            Signal lastSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(TradeAction.Buy, lastSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, lastSignal.OrderType);
            Assert.AreEqual(1.251, lastSignal.Price);
            Assert.AreEqual(this.rStrategyVolumeChangeStep.Amount, lastSignal.Amount);
        }

        [TestMethod]
        public void Make_Signal_To_Sell_Spread_RightLeg_test()
        {
            Assert.AreEqual(0, this.signalQueue.Count);

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.06, 1.01));

            Assert.AreEqual(1, this.signalQueue.Count);

            Signal lastSignal = this.signalQueue.Dequeue();
            Assert.AreEqual(TradeAction.Sell, lastSignal.TradeAction);
            Assert.AreEqual(OrderType.Market, lastSignal.OrderType);
            Assert.AreEqual(1.01, lastSignal.Price);
            Assert.AreEqual(this.rStrategyVolumeChangeStep.Amount, lastSignal.Amount);
        }

        [TestMethod]
        public void Make_Signal_To_Do_Nothing()
        {
            Assert.AreEqual(0, this.signalQueue.Count);

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.06, 1.02));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Ignore_signal_to_sell_if_position_amount_equals_strategy_amount_test()
        {
            Signal openSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 1, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Order openOrder = new Order(openSignal);
            this.tradingData.Get<ICollection<Order>>().Add(openOrder);

            Trade openTrade = new Trade(openOrder, openOrder.Portfolio, openOrder.Symbol, openOrder.Price, openOrder.Amount, DateTime.Now);
            this.tradingData.Get<ICollection<Trade>>().Add(openTrade);
            openOrder.FilledAmount = openOrder.Amount;

            Assert.AreEqual(11, this.tradingData.GetAmount(this.rStrategy));
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.251, 1.24));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Ignore_signal_to_buy_if_position_amount_equals_strategy_amount_test()
        {
            Signal openSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 1, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Order openOrder = new Order(openSignal);
            this.tradingData.Get<ICollection<Order>>().Add(openOrder);

            Trade openTrade = new Trade(openOrder, openOrder.Portfolio, openOrder.Symbol, openOrder.Price, -openOrder.Amount, DateTime.Now);
            this.tradingData.Get<ICollection<Trade>>().Add(openTrade);
            openOrder.FilledAmount = openOrder.Amount;

            Assert.AreEqual(-11, this.tradingData.GetAmount(this.rStrategy));
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.251, 1.24));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Ignore_signal_to_buy_if_unfilled_orders_exists_test()
        {
            Signal openSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 1, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Order openOrder = new Order(openSignal);
            this.tradingData.Get<ICollection<Order>>().Add(openOrder);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.251, 1.24));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Ignore_signal_to_sell_if_unfilled_orders_exists_test()
        {
            Signal openSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 1, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Order openOrder = new Order(openSignal);
            this.tradingData.Get<ICollection<Order>>().Add(openOrder);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.251, 1.24));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Change_signal_to_buy_amount_if_volume_step_greater_than_unfilled_amount_test()
        {
            Signal openSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Sell, OrderType.Market, 1, 0, 0);
            openSignal.Amount = 10;
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Order openOrder = new Order(openSignal);
            this.tradingData.Get<ICollection<Order>>().Add(openOrder);

            Trade openTrade = new Trade(openOrder, openOrder.Portfolio, openOrder.Symbol, openOrder.Price, -openOrder.Amount, DateTime.Now);
            this.tradingData.Get<ICollection<Trade>>().Add(openTrade);
            openOrder.FilledAmount = openOrder.Amount;

            Assert.AreEqual(-10, this.tradingData.GetAmount(this.rStrategy));
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.251, 1.24));

            Assert.AreEqual(1, this.signalQueue.Count);
            Signal signal = this.signalQueue.Dequeue();

            Assert.AreEqual(1, signal.Amount);
        }

        [TestMethod]
        public void Change_signal_to_sell_amount_if_volume_step_greater_than_unfilled_amount_test()
        {
            Signal openSignal = new Signal(this.rStrategy, DateTime.Now, TradeAction.Buy, OrderType.Market, 1, 0, 0);
            openSignal.Amount = 10;
            this.tradingData.Get<ICollection<Signal>>().Add(openSignal);

            Order openOrder = new Order(openSignal);
            this.tradingData.Get<ICollection<Order>>().Add(openOrder);

            Trade openTrade = new Trade(openOrder, openOrder.Portfolio, openOrder.Symbol, openOrder.Price, openOrder.Amount, DateTime.Now);
            this.tradingData.Get<ICollection<Trade>>().Add(openTrade);
            openOrder.FilledAmount = openOrder.Amount;

            Assert.AreEqual(10, this.tradingData.GetAmount(this.rStrategy));
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(new SpreadValue(this.arbitrageSettings.Id, DateTime.Now, 1.251, 1.24));

            Assert.AreEqual(1, this.signalQueue.Count);
            Signal signal = this.signalQueue.Dequeue();

            Assert.AreEqual(1, signal.Amount);
        }

    }
}
