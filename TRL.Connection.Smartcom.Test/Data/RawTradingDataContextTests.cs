using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Common.Data;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class RawTradingDataContextTests
    {
        [TestMethod]
        public void RawTradingDataContext_Contains_Collections()
        {
            RawTradingDataContext rawData = new RawTradingDataContext();

            Assert.IsTrue(rawData is BaseDataContext);

            Assert.IsNotNull(rawData.GetData<OrderFailed>());
            Assert.IsNotNull(rawData.GetData<OrderSucceeded>());
            Assert.IsNotNull(rawData.GetData<UpdateOrder>());
            Assert.IsNotNull(rawData.GetData<SetPortfolio>());
            Assert.IsNotNull(rawData.GetData<TradeInfo>());
            Assert.IsNotNull(rawData.GetData<RawSymbol>());
            Assert.IsNotNull(rawData.GetData<OrderMoveSucceeded>());
            Assert.IsNotNull(rawData.GetData<OrderMoveFailed>());
            Assert.IsNotNull(rawData.GetData<CookieToOrderNoAssociation>());
            Assert.IsNotNull(rawData.GetData<PendingTradeInfo>());
        }
    }
}
