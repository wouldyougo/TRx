using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.TimeHelpers;
using TRL.Common.Models;
using System.Globalization;
//using TRL.Logging;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class SignalTests
    {
        [TestMethod]
        public void Signal_one_more_constructor_test()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-6.13_FT", 10);

            DateTime signalDate = BrokerDateTime.Make(DateTime.Now);

            Signal signal = new Signal(strategyHeader, signalDate, TradeAction.Buy, OrderType.Stop, 150000, 149000, 149500);

            Assert.IsTrue(signal is IIdentified);
            Assert.IsTrue(signal is IDateTime);

            Assert.IsTrue(signal.Id > 0);
            Assert.AreEqual(signalDate, signal.DateTime);
            Assert.AreEqual(TradeAction.Buy, signal.TradeAction);
            Assert.AreEqual(OrderType.Stop, signal.OrderType);
            Assert.AreEqual(150000D, signal.Price);
            Assert.AreEqual(149000D, signal.Stop);
            Assert.AreEqual(149500D, signal.Limit);
            Assert.AreEqual(1, signal.StrategyId);
            Assert.AreEqual(strategyHeader, signal.Strategy);
        }

        [TestMethod]
        public void TradeSignal_ToString()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-6.13_FT", 10);

            CultureInfo ci = CultureInfo.InvariantCulture;

            DateTime signalDate = BrokerDateTime.Make(DateTime.Now);

            Signal ts = new Signal(strategyHeader, signalDate, TradeAction.Buy, OrderType.Stop, 150000, 149000, 149500);

            string result = String.Format("Signal Id: {0}, DateTime: {1}, TradeAction: {2}, OrderType: {3}, Price: {4}, Stop: {5}, Limit: {6}, Amount: {7}, StrategyId: {8}",
                ts.Id, ts.DateTime.ToString(ci), ts.TradeAction, ts.OrderType, ts.Price.ToString("0.0000", ci), ts.Stop.ToString("0.0000", ci), ts.Limit.ToString("0.0000", ci), ts.Strategy.Amount.ToString("0.0000", ci), ts.Strategy.Id);

            Assert.AreEqual(result, ts.ToString());
        }
        
        [TestMethod]
        public void TradeSignal_ToImportString()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy", "BP12345-RF-01", "RTS-6.13_FT", 10);

            CultureInfo ci = CultureInfo.InvariantCulture;

            DateTime signalDate = BrokerDateTime.Make(DateTime.Now);

            Signal ts = new Signal(strategyHeader, signalDate, TradeAction.Buy, OrderType.Stop, 150000, 149000, 149500);

            string result = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                ts.Id, ts.DateTime.ToString(ci), ts.TradeAction, ts.OrderType, ts.Price.ToString("0.0000", ci), ts.Stop.ToString("0.0000", ci), ts.Limit.ToString("0.0000", ci), ts.Strategy.Amount.ToString("0.0000", ci), ts.Strategy.Id);

            Assert.AreEqual(result, ts.ToImportString());
        }

        [TestMethod]
        public void TradeSignal_Parse_test()
        {
            string signalString = "1, 01/01/2013 12:24:40, Buy, Limit, 150000.0000, 149000.0000, 155000.0000, 8.0000, 1";

            Signal signal = Signal.Parse(signalString);

            Assert.AreEqual(1, signal.Id);
            Assert.AreEqual(new DateTime(2013, 1, 1, 12, 24, 40), signal.DateTime);
            Assert.AreEqual(TradeAction.Buy, signal.TradeAction);
            Assert.AreEqual(OrderType.Limit, signal.OrderType);
            Assert.AreEqual(150000D, signal.Price);
            Assert.AreEqual(149000D, signal.Stop);
            Assert.AreEqual(155000D, signal.Limit);
            Assert.AreEqual(1, signal.StrategyId);
            Assert.IsNull(signal.Strategy);
        }
    }
}
