using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Test.Mocks;
using TRL.Common.Models;
using TRL.Common.Handlers;
using TRL.Common.Collections;
using TRL.Configuration;
using TRL.Extensions.Inputs;
using TRL.Common.TimeHelpers;
using TRL.Emulation;
using TRL.Common.Extensions.Data;
using TRL.Logging;
using TRL.Common;
using TRL.Handlers.Inputs;

namespace TRL.Extensions.Inputs.Test
{
    [TestClass]
    public class BreakOutOnTickTests
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private StrategyHeader strategyHeader;
        private BarSettings barSettings;
        private BreakOutOnTick handler;

        [TestInitialize]
        public void Inputs_Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();

            this.strategyHeader = new StrategyHeader(1, "Break out", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.barSettings = new BarSettings(this.strategyHeader, "RTS-9.13_FT", 3600, 19);
            this.tradingData.Get<ICollection<BarSettings>>().Add(this.barSettings);

            this.handler =
                new BreakOutOnTick(this.strategyHeader, this.tradingData, this.signalQueue, new NullLogger());

            FillTradingDataWithBars();

            Assert.AreEqual(0, this.signalQueue.Count);
            Assert.IsFalse(this.tradingData.PositionExists(this.strategyHeader));
            Assert.IsFalse(this.tradingData.UnfilledExists(this.strategyHeader));
        }

        private void FillTradingDataWithBars()
        {
            for (int i = 0; i < this.barSettings.Period; i++ )
                this.tradingData.Get<ObservableCollection<Bar>>().Add(
                    new Bar(this.strategyHeader.Symbol,
                        this.barSettings.Interval,
                        BrokerDateTime.Make(DateTime.Now),
                        i,
                        i,
                        i,
                        i,
                        i));
        }

        [TestMethod]
        public void Inputs_do_nothing_when_tick_has_other_symbol_test()
        {
            this.tradingData.Get<ObservableCollection<Tick>>().Add(
                new Tick("Si", DateTime.Now, 1000000, 1));

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_do_nothing_when_tick_price_has_no_break_out_value_test()
        {
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { Symbol = "RTS-9.13_FT", Price = 5 });

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_do_nothing_on_break_to_high_when_long_position_exists_for_strategy_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 0, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { Symbol = "RTS-9.13_FT", Price = 151000 });

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_do_nothing_on_break_to_high_when_short_position_exists_for_strategy_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 0, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { Symbol = "RTS-9.13_FT", Price = 151000 });

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_do_nothing_on_break_to_low_when_long_position_exists_for_strategy_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 0, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { Symbol = "RTS-9.13_FT", Price = -1 });

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_do_nothing_on_break_to_low_when_short_position_exists_for_strategy_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 0, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { Symbol = "RTS-9.13_FT", Price = -1 });

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_make_signal_to_buy_when_price_breaks_to_high_test()
        {
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { Symbol = "RTS-9.13_FT", Price = 20 });

            Assert.AreEqual(1, this.signalQueue.Count);

            Signal signal = this.signalQueue.Dequeue();

            Assert.AreEqual(0, signal.Stop);
            Assert.AreEqual(0, signal.Limit);
            Assert.AreEqual(18, signal.Price);
            Assert.AreEqual(OrderType.Market, signal.OrderType);
            Assert.AreEqual(TradeAction.Buy, signal.TradeAction);
            Assert.AreEqual(this.strategyHeader, signal.Strategy);
            Assert.AreEqual(this.strategyHeader.Id, signal.StrategyId);
            Assert.IsTrue(signal.Id > 0);
        }

        [TestMethod]
        public void Inputs_make_signal_to_sell_when_price_breaks_to_low_test()
        {
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { Symbol = "RTS-9.13_FT", Price = -1 });

            Assert.AreEqual(1, this.signalQueue.Count);

            Signal signal = this.signalQueue.Dequeue();

            Assert.AreEqual(0, signal.Stop);
            Assert.AreEqual(0, signal.Limit);
            Assert.AreEqual(0, signal.Price);
            Assert.AreEqual(OrderType.Market, signal.OrderType);
            Assert.AreEqual(TradeAction.Sell, signal.TradeAction);
            Assert.AreEqual(this.strategyHeader, signal.Strategy);
            Assert.AreEqual(this.strategyHeader.Id, signal.StrategyId);
            Assert.IsTrue(signal.Id > 0);
        }

        [TestMethod]
        public void Inputs_ignore_break_to_high_when_unfilled_market_buy_order_exists()
        {
            Signal openSignal =
                new Signal(this.strategyHeader,
                    BrokerDateTime.Make(DateTime.Now),
                    TradeAction.Buy,
                    OrderType.Market,
                    0,
                    0,
                    0);
            this.tradingData.AddSignalAndItsOrder(openSignal);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, BrokerDateTime.Make(DateTime.Now), 20, 1));
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_ignore_break_to_low_when_unfilled_market_buy_order_exists()
        {
            Signal openSignal =
                new Signal(this.strategyHeader,
                    BrokerDateTime.Make(DateTime.Now),
                    TradeAction.Buy,
                    OrderType.Market,
                    0,
                    0,
                    0);
            this.tradingData.AddSignalAndItsOrder(openSignal);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, BrokerDateTime.Make(DateTime.Now), -1, 1));
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_ignore_break_to_low_when_unfilled_market_sell_order_exists()
        {
            Signal openSignal =
                new Signal(this.strategyHeader,
                    BrokerDateTime.Make(DateTime.Now),
                    TradeAction.Sell,
                    OrderType.Market,
                    0,
                    0,
                    0);
            this.tradingData.AddSignalAndItsOrder(openSignal);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, BrokerDateTime.Make(DateTime.Now), -1, 1));
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_ignore_break_to_high_when_unfilled_market_sell_order_exists()
        {
            Signal openSignal =
                new Signal(this.strategyHeader,
                    BrokerDateTime.Make(DateTime.Now),
                    TradeAction.Sell,
                    OrderType.Market,
                    0,
                    0,
                    0);
            this.tradingData.AddSignalAndItsOrder(openSignal);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, BrokerDateTime.Make(DateTime.Now), 20, 1));
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_do_nothing_when_there_is_no_any_bars_in_trading_context_test()
        {
            this.tradingData.Get<ICollection<Bar>>().Clear();

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, BrokerDateTime.Make(DateTime.Now), 20, 1));
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_do_nothing_when_there_is_no_enough_bars_in_trading_context_test()
        {
            Bar last = this.tradingData.Get<ICollection<Bar>>().Last();

            this.tradingData.Get<ICollection<Bar>>().Remove(last);

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(this.strategyHeader.Symbol, BrokerDateTime.Make(DateTime.Now), 20, 1));
            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_do_nothing_on_break_to_high_when_partial_long_position_exists_for_strategy_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 0, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 151000, 3);
            trade.Order.Cancel(BrokerDateTime.Make(DateTime.Now), "Order is cancelled by broker");

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { Symbol = "RTS-9.13_FT", Price = 151000 });

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_do_nothing_on_break_to_high_when_partial_short_position_exists_for_strategy_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 0, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 151000, 5);
            trade.Order.Cancel(BrokerDateTime.Make(DateTime.Now), "Order is cancelled by broker");

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { Symbol = "RTS-9.13_FT", Price = 151000 });

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_do_nothing_on_break_to_low_when_partial_long_position_exists_for_strategy_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 0, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 150000, 6);
            trade.Order.Cancel(BrokerDateTime.Make(DateTime.Now), "Order is cancelled by broker");

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { Symbol = "RTS-9.13_FT", Price = -1 });

            Assert.AreEqual(0, this.signalQueue.Count);
        }

        [TestMethod]
        public void Inputs_do_nothing_on_break_to_low_when_partial_short_position_exists_for_strategy_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 0, 0, 0);
            Trade trade = this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 150000, 9);
            trade.Order.Cancel(BrokerDateTime.Make(DateTime.Now), "Order is cancelled by broker");

            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick { Symbol = "RTS-9.13_FT", Price = -1 });

            Assert.AreEqual(0, this.signalQueue.Count);
        }
    }
}
