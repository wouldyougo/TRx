using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Common.TimeHelpers;
using TRL.Extensions.Inputs;
using TRL.Logging;
using TRL.Handlers.Inputs;

namespace TRL.Extensions.Inputs.Test
{
    [TestClass]
    public class BuyOnTickTests
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Inputs_Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();

            this.strategyHeader = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            BuyOnTick handler = new BuyOnTick(this.strategyHeader, this.tradingData, this.signalQueue, new NullLogger());
        }

        [TestMethod]
        public void Inputs_open_long_position()
        {

            BarSettings barSettings = new BarSettings(this.strategyHeader, "RTS-9.13_FT", 60, 10);
            this.tradingData.Get<ICollection<BarSettings>>().Add(barSettings);

            SMASettings smaSettings = new SMASettings(this.strategyHeader, 5, 10);
            this.tradingData.Get<ICollection<SMASettings>>().Add(smaSettings);

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 0, 0),  120, 125, 119, 124, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 5, 0),  121, 126, 120, 125, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 10, 0), 122, 127, 121, 126, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 15, 0), 123, 128, 122, 127, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 20, 0), 124, 129, 123, 128, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 25, 0), 125, 130, 124, 129, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 30, 0), 126, 131, 125, 130, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 35, 0), 127, 132, 126, 131, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 40, 0), 128, 133, 127, 132, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 45, 0), 129, 134, 128, 133, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 50, 0), 130, 135, 129, 134, 10));
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick("RTS-9.13_FT", new DateTime(2013, 8, 8, 10, 50, 0), 134, 10, TradeAction.Sell));


            Assert.AreEqual(1, this.signalQueue.Count);
            Signal signal = this.signalQueue.Dequeue();

            Assert.AreEqual(this.strategyHeader, signal.Strategy);
            Assert.AreEqual(this.strategyHeader.Id, signal.StrategyId);
            Assert.AreEqual(TradeAction.Buy, signal.TradeAction);
            Assert.AreEqual(OrderType.Limit, signal.OrderType);
            Assert.AreEqual(134, signal.Price);
            Assert.AreEqual(134, signal.Limit);
            Assert.AreEqual(0, signal.Stop);
        }

        [TestMethod]
        public void Inputs_open_short_position()
        {

            BarSettings barSettings = new BarSettings(this.strategyHeader, "RTS-9.13_FT", 60, 10);
            this.tradingData.Get<ICollection<BarSettings>>().Add(barSettings);

            SMASettings smaSettings = new SMASettings(this.strategyHeader, 5, 10);
            this.tradingData.Get<ICollection<SMASettings>>().Add(smaSettings);

            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 0, 0),  120, 125, 119, 124, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 5, 0),  119, 124, 118, 123, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 10, 0), 118, 123, 117, 122, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 15, 0), 117, 122, 116, 121, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 20, 0), 116, 121, 115, 120, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 25, 0), 115, 110, 114, 119, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 30, 0), 114, 119, 113, 118, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 35, 0), 113, 118, 112, 117, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 40, 0), 112, 117, 111, 116, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 45, 0), 111, 116, 110, 115, 10));
            this.tradingData.Get<ObservableCollection<Bar>>().Add(new Bar("RTS-9.13_FT", barSettings.Interval, new DateTime(2013, 8, 8, 10, 50, 0), 110, 115, 109, 114, 10));
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick("RTS-9.13_FT", new DateTime(2013, 8, 8, 10, 50, 0), 114, 10, TradeAction.Sell));

            Assert.AreEqual(1, this.signalQueue.Count);
            Signal signal = this.signalQueue.Dequeue();

            Assert.AreEqual(this.strategyHeader, signal.Strategy);
            Assert.AreEqual(this.strategyHeader.Id, signal.StrategyId);
            Assert.AreEqual(TradeAction.Sell, signal.TradeAction);
            Assert.AreEqual(OrderType.Limit, signal.OrderType);
            Assert.AreEqual(114, signal.Price);
            Assert.AreEqual(114, signal.Limit);
            Assert.AreEqual(0, signal.Stop);
        }

    }
}
