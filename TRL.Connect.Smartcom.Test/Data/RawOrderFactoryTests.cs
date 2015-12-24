using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Connect.Smartcom.Models;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Data;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class RawOrderFactoryTests
    {
        private RawOrderFactory factory;

        [TestInitialize]
        public void Setup()
        {
            factory = new RawOrderFactory(new FortsTradingSchedule());
        }

        [TestMethod]
        public void RawOrderFactory_Make_SellAtMarket_Order()
        {
            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "PRTFL", "SMBL", TradeAction.Sell, OrderType.Market, 1, 0, 0);

            RawOrder rawOrder = factory.Make(order);

            Assert.AreEqual(order.Id, rawOrder.Cookie);
            Assert.AreEqual(order.Amount, rawOrder.Amount);
            Assert.AreEqual(StOrder_Action.StOrder_Action_Sell, rawOrder.Action);
            Assert.AreEqual(StOrder_Type.StOrder_Type_Market, rawOrder.Type);
            Assert.AreEqual(StOrder_Validity.StOrder_Validity_Day, rawOrder.Validity);
            Assert.AreEqual("SMBL", rawOrder.Symbol);
            Assert.AreEqual("PRTFL", rawOrder.Portfolio);
            Assert.AreEqual(0, rawOrder.Price);
        }

        [TestMethod]
        public void RawOrderFactory_Make_BuyAtMarket_Order()
        {
            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "PRTFL", "SMBL", TradeAction.Buy, OrderType.Market, 1, 0, 0);

            RawOrder rawOrder = factory.Make(order);

            Assert.AreEqual(order.Id, rawOrder.Cookie);
            Assert.AreEqual(order.Amount, rawOrder.Amount);
            Assert.AreEqual(StOrder_Action.StOrder_Action_Buy, rawOrder.Action);
            Assert.AreEqual(StOrder_Type.StOrder_Type_Market, rawOrder.Type);
            Assert.AreEqual(StOrder_Validity.StOrder_Validity_Day, rawOrder.Validity);
            Assert.AreEqual("SMBL", rawOrder.Symbol);
            Assert.AreEqual("PRTFL", rawOrder.Portfolio);
            Assert.AreEqual(0, rawOrder.Price);
        }

        [TestMethod]
        public void RawOrderFactory_make_limit_order_to_buy()
        {
            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "Portfolio", "Symbol", TradeAction.Buy, OrderType.Limit, 1, 149000, 0);

            RawOrder rawOrder = factory.Make(order);

            Assert.AreEqual(order.Id, rawOrder.Cookie);
            Assert.AreEqual(order.Amount, rawOrder.Amount);
            Assert.AreEqual(StOrder_Action.StOrder_Action_Buy, rawOrder.Action);
            Assert.AreEqual(StOrder_Type.StOrder_Type_Limit, rawOrder.Type);
            Assert.AreEqual(StOrder_Validity.StOrder_Validity_Day, rawOrder.Validity);
            Assert.AreEqual("Symbol", rawOrder.Symbol);
            Assert.AreEqual("Portfolio", rawOrder.Portfolio);
            Assert.AreEqual(149000, rawOrder.Price);
        }

        [TestMethod]
        public void RawOrderFactory_make_limit_order_to_sell()
        {
            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "Portfolio", "Symbol", TradeAction.Sell, OrderType.Limit, 1, 125000, 0);

            RawOrder rawOrder = factory.Make(order);

            Assert.AreEqual(order.Id, rawOrder.Cookie);
            Assert.AreEqual(order.Amount, rawOrder.Amount);
            Assert.AreEqual(StOrder_Action.StOrder_Action_Sell, rawOrder.Action);
            Assert.AreEqual(StOrder_Type.StOrder_Type_Limit, rawOrder.Type);
            Assert.AreEqual(StOrder_Validity.StOrder_Validity_Day, rawOrder.Validity);
            Assert.AreEqual("Symbol", rawOrder.Symbol);
            Assert.AreEqual("Portfolio", rawOrder.Portfolio);
            Assert.AreEqual(125000, rawOrder.Price);

        }

        [TestMethod]
        public void RawOrderFactory_make_stop_order_to_buy()
        {
            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "Portfolio", "Symbol", TradeAction.Buy, OrderType.Stop, 1, 0, 130000);

            RawOrder rawOrder = factory.Make(order);

            Assert.AreEqual(order.Id, rawOrder.Cookie);
            Assert.AreEqual(order.Amount, rawOrder.Amount);
            Assert.AreEqual(StOrder_Action.StOrder_Action_Buy, rawOrder.Action);
            Assert.AreEqual(StOrder_Type.StOrder_Type_Stop, rawOrder.Type);
            Assert.AreEqual(StOrder_Validity.StOrder_Validity_Day, rawOrder.Validity);
            Assert.AreEqual("Symbol", rawOrder.Symbol);
            Assert.AreEqual("Portfolio", rawOrder.Portfolio);
            Assert.AreEqual(0, rawOrder.Price);
            Assert.AreEqual(130000, rawOrder.Stop);
        }

        [TestMethod]
        public void RawOrderFactory_make_stop_order_to_sell()
        {
            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "Portfolio", "Symbol", TradeAction.Sell, OrderType.Stop, 1, 0, 125000);

            RawOrder rawOrder = factory.Make(order);

            Assert.AreEqual(order.Id, rawOrder.Cookie);
            Assert.AreEqual(order.Amount, rawOrder.Amount);
            Assert.AreEqual(StOrder_Action.StOrder_Action_Sell, rawOrder.Action);
            Assert.AreEqual(StOrder_Type.StOrder_Type_Stop, rawOrder.Type);
            Assert.AreEqual(StOrder_Validity.StOrder_Validity_Day, rawOrder.Validity);
            Assert.AreEqual("Symbol", rawOrder.Symbol);
            Assert.AreEqual("Portfolio", rawOrder.Portfolio);
            Assert.AreEqual(0, rawOrder.Price);
            Assert.AreEqual(125000, rawOrder.Stop);
        }

        [TestMethod]
        public void RawOrderFactory_validity_day_for_order_with_expiration_date_before_SessionEnd()
        {
            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "PRTFL", "SMBL", TradeAction.Sell, OrderType.Market, 1, 0, 0);

            Assert.AreEqual(new FortsTradingSchedule().SessionEnd, order.ExpirationDate);

            RawOrder rawOrder = factory.Make(order);

            Assert.AreEqual(StOrder_Validity.StOrder_Validity_Day, rawOrder.Validity);
        }

        [TestMethod]
        public void RawOrderFactory_validity_gtc_for_order_with_expiration_date_greater_than_SessionEnd()
        {
            Order order = new Order(1, BrokerDateTime.Make(DateTime.Now), "PRTFL", "SMBL", TradeAction.Sell, OrderType.Market, 1, 0, 0);
            order.ExpirationDate = new FortsTradingSchedule().SessionEnd.AddMilliseconds(1);

            Assert.IsTrue(new FortsTradingSchedule().SessionEnd < order.ExpirationDate);

            RawOrder rawOrder = factory.Make(order);

            Assert.AreEqual(StOrder_Validity.StOrder_Validity_Gtc, rawOrder.Validity);
        }

    }
}
