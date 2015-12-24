using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Connect.Smartcom.Data;
using TRL.Common.TimeHelpers;
using TRL.Common.Models;
using System.Collections.Generic;
using TRL.Emulation;
using TRL.Connect.Smartcom.Handlers;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Test.Handlers
{
    [TestClass]
    public class MakeTradeOnCookieToOrderNoAssociationTests
    {
        private BaseDataContext rawData;
        private IDataContext tradingData;

        private StrategyHeader strategyHeader;
        private Signal signal;
        private Order order;
        private TradeInfo tradeInfo;
        private CookieToOrderNoAssociation cookieToOrderNoAssociation;
        private PendingTradeInfo pendingTradeInfo;

        private MakeTradeOnCookieToOrderNoAssociation handler;

        [TestInitialize]
        public void Setup()
        {
            this.rawData = new RawTradingDataContext();
            this.tradingData = new TradingDataContext();

            this.strategyHeader = new StrategyHeader(1, 
                "Strategy description",
                "ST12345-RF-01",
                "RTS-9.14_FT",
                10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            this.signal = new Signal(this.strategyHeader,
                BrokerDateTime.Make(DateTime.Now),
                TradeAction.Sell,
                OrderType.Market,
                125000,
                0,
                0);
            
            this.order = this.tradingData.AddSignalAndItsOrder(this.signal);
            
            this.tradeInfo = 
                new TradeInfo("ST12345-RF-01",
                    "RTS-9.14_FT",
                    "12345",
                    this.order.Price,
                    -1,
                    BrokerDateTime.Make(DateTime.Now),
                    "54321");

            this.cookieToOrderNoAssociation =
                new CookieToOrderNoAssociation(this.order.Id, this.tradeInfo.OrderNo);

            this.pendingTradeInfo =
                new PendingTradeInfo(this.tradeInfo);

            this.handler =
                new MakeTradeOnCookieToOrderNoAssociation(this.rawData, this.tradingData, new NullLogger());

            Assert.AreEqual(0, this.rawData.GetData<CookieToOrderNoAssociation>().Count);
            Assert.AreEqual(0, this.rawData.GetData<PendingTradeInfo>().Count);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
        }

        [TestMethod]
        public void make_Trade_when_single_PendingTradeInfo_exists_test()
        {
            this.rawData.GetData<PendingTradeInfo>().Add(this.pendingTradeInfo);
            this.rawData.GetData<CookieToOrderNoAssociation>().Add(this.cookieToOrderNoAssociation);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count);
            Assert.AreEqual(0, this.rawData.GetData<PendingTradeInfo>().Count);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Trade>>().Count());

            Trade trade = this.tradingData.Get<IEnumerable<Trade>>().Last();
            Assert.AreEqual(this.tradeInfo.Amount, trade.Amount);
            Assert.AreEqual(this.tradeInfo.DateTime, trade.DateTime);
            Assert.AreEqual(this.tradeInfo.Portfolio, trade.Portfolio);
            Assert.AreEqual(this.tradeInfo.Price, trade.Price);
            Assert.AreEqual(this.order.Id, trade.OrderId);
            Assert.AreEqual(this.order, trade.Order);
        }

        [TestMethod]
        public void make_Trade_when_multiple_PendingTradeInfo_exists_test()
        {
            this.tradeInfo =
                new TradeInfo("ST12345-RF-01",
                    "RTS-9.14_FT",
                    "12345",
                    this.order.Price,
                    -5,
                    BrokerDateTime.Make(DateTime.Now),
                    "54321");
            this.pendingTradeInfo =
                new PendingTradeInfo(this.tradeInfo);
            this.rawData.GetData<PendingTradeInfo>().Add(this.pendingTradeInfo);

            this.tradeInfo =
                new TradeInfo("ST12345-RF-01",
                    "RTS-9.14_FT",
                    "12345",
                    this.order.Price,
                    -5,
                    BrokerDateTime.Make(DateTime.Now),
                    "54325");
            this.pendingTradeInfo =
                new PendingTradeInfo(this.tradeInfo);
            this.rawData.GetData<PendingTradeInfo>().Add(this.pendingTradeInfo);

            this.cookieToOrderNoAssociation =
                new CookieToOrderNoAssociation(this.order.Id, this.tradeInfo.OrderNo);
            this.rawData.GetData<CookieToOrderNoAssociation>().Add(this.cookieToOrderNoAssociation);


            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count);
            Assert.AreEqual(0, this.rawData.GetData<PendingTradeInfo>().Count);
            Assert.AreEqual(2, this.tradingData.Get<IEnumerable<Trade>>().Count());

            Trade trade = this.tradingData.Get<IEnumerable<Trade>>().Last();
            Assert.AreEqual(this.tradeInfo.Amount, trade.Amount);
            Assert.AreEqual(this.tradeInfo.DateTime, trade.DateTime);
            Assert.AreEqual(this.tradeInfo.Portfolio, trade.Portfolio);
            Assert.AreEqual(this.tradeInfo.Price, trade.Price);
            Assert.AreEqual(this.order.Id, trade.OrderId);
            Assert.AreEqual(this.order, trade.Order);
        }

        [TestMethod]
        public void ignore_CookieToOrderNoAssociation_when_no_PendingTradeInfo_exists_test()
        {
            this.rawData.GetData<CookieToOrderNoAssociation>().Add(this.cookieToOrderNoAssociation);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count);
            Assert.AreEqual(0, this.rawData.GetData<PendingTradeInfo>().Count);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
        }

        [TestMethod]
        public void ignore_CookieToOrderNoAssociation_when_no_Order_exists_test()
        {
            this.cookieToOrderNoAssociation =
                new CookieToOrderNoAssociation(1, this.tradeInfo.OrderNo);

            this.rawData.GetData<PendingTradeInfo>().Add(this.pendingTradeInfo);
            this.rawData.GetData<CookieToOrderNoAssociation>().Add(this.cookieToOrderNoAssociation);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count);
            Assert.AreEqual(1, this.rawData.GetData<PendingTradeInfo>().Count);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
        }

        [TestMethod]
        public void ignore_CookieToOrderNoAssociation_when_order_IsFilled_test()
        {
            this.order.FilledAmount = this.order.Amount;
            Assert.IsTrue(this.order.IsFilled);

            this.rawData.GetData<PendingTradeInfo>().Add(this.pendingTradeInfo);
            this.rawData.GetData<CookieToOrderNoAssociation>().Add(this.cookieToOrderNoAssociation);
            Assert.AreEqual(1, this.rawData.GetData<CookieToOrderNoAssociation>().Count);
            Assert.AreEqual(1, this.rawData.GetData<PendingTradeInfo>().Count);
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Trade>>().Count());
        }

    }
}
