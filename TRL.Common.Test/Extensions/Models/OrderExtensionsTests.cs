using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
//using TRL.Common.Extensions.Data;
using TRL.Common.Extensions.Models;

namespace TRL.Common.Extensions.Models.Test
{
    [TestClass]
    public class OrderExtensionsTests
    {
        private StrategyHeader strategyHeader;
        private Signal signalToBuy, signalToSell;

        [TestInitialize]
        public void Setup()
        {
            this.strategyHeader =
                new StrategyHeader(1, "Description", "BP12345-RF-01", "RTS-12.13_FT", 1);

            this.signalToBuy =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 150000, 0, 0);

            this.signalToSell =
                new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 150000, 0, 0);

        }

        [TestMethod]
        public void OrderExtensions_InverseAction_for_order_to_buy_test()
        {
            Order buyOrder = new Order(this.signalToBuy);

            Assert.AreEqual(TradeAction.Sell, buyOrder.InverseAction());
        }

        [TestMethod]
        public void OrderExtensions_InverseAction_for_order_to_sell_test()
        {
            Order sellOrder = new Order(this.signalToSell);

            Assert.AreEqual(TradeAction.Buy, sellOrder.InverseAction());
        }
    }
}
