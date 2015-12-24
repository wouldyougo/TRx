using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Extensions.Models;
using TRL.Common.TimeHelpers;
//using TRL.Common.Extensions.Data;

namespace TRL.Common.Extensions.Models.Test
{
    [TestClass]
    public class SignalExtensionsTests
    {
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader = new StrategyHeader(1, "description", "BP12345-RF-01", "RTS-9.13_FT", 10);
        }

        [TestMethod]
        public void Signal_to_buy_GetSignedAmount_test()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);

            Assert.AreEqual(10, signal.GetSignedAmount());
        }

        [TestMethod]
        public void Signal_to_sell_GetSignedAmount_test()
        {
            Signal signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, 150000, 0, 0);

            Assert.AreEqual(-10, signal.GetSignedAmount());
        }
    }
}
