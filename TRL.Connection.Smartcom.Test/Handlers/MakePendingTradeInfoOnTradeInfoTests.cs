using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Connect.Smartcom.Handlers;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Test.Handlers
{
    [TestClass]
    public class MakePendingTradeInfoOnTradeInfoTests
    {
        private BaseDataContext rawData;
        private TradeInfo tradeInfo;

        [TestInitialize]
        public void Setup()
        {
            this.rawData = new RawTradingDataContext();
            this.tradeInfo =
                new TradeInfo("ST12345-RF-01",
                    "RTS-9.14_FT",
                    "355155",
                    123000,
                    -1,
                    DateTime.Now,
                    "899320");

            MakePendingTradeInfoOnTradeInfo handler =
                new MakePendingTradeInfoOnTradeInfo(this.rawData, new NullLogger());

            Assert.AreEqual(0, this.rawData.GetData<TradeInfo>().Count);
            Assert.AreEqual(0, this.rawData.GetData<PendingTradeInfo>().Count);
        }

        [TestMethod]
        public void make_ExpectedUpdateOrder_test()
        {
            this.rawData.GetData<TradeInfo>().Add(this.tradeInfo);

            Assert.AreEqual(1, this.rawData.GetData<TradeInfo>().Count);
            Assert.AreEqual(1, this.rawData.GetData<PendingTradeInfo>().Count);

            PendingTradeInfo item = this.rawData.GetData<PendingTradeInfo>().Last();

            Assert.AreEqual(this.tradeInfo.Portfolio, item.Portfolio);
            Assert.AreEqual(this.tradeInfo.Symbol, item.Symbol);
            Assert.AreEqual(this.tradeInfo.OrderNo, item.OrderNo);
            Assert.AreEqual(this.tradeInfo.Price, item.Price);
            Assert.AreEqual(this.tradeInfo.Amount, item.Amount);
            Assert.AreEqual(this.tradeInfo.DateTime, item.DateTime);
            Assert.AreEqual(this.tradeInfo.TradeNo, item.TradeNo);
        }

        [TestMethod]
        public void ignore_TradeInfo_duplicates_test()
        {
            this.rawData.GetData<TradeInfo>().Add(this.tradeInfo);

            Assert.AreEqual(1, this.rawData.GetData<TradeInfo>().Count);
            Assert.AreEqual(1, this.rawData.GetData<PendingTradeInfo>().Count);

            this.rawData.GetData<TradeInfo>().Add(this.tradeInfo);

            Assert.AreEqual(2, this.rawData.GetData<TradeInfo>().Count);
            Assert.AreEqual(1, this.rawData.GetData<PendingTradeInfo>().Count);
        }
    }
}
