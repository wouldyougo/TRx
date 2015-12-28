using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Handlers;
using TRL.Connect.Smartcom.Handlers;
using TRL.Connect.Smartcom.Data;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class AdapterHandlersTests
    {
        private IDataContext handlers;

        [TestInitialize]
        public void Setup()
        {
            this.handlers = new AdapterHandlers();
        }

        [TestMethod]
        public void AdapterHandlers_contains_RejectOrderOnOrderFailed_test()
        {
            Assert.IsNotNull(handlers.Get<RejectOrderOnOrderFailed>());
        }

        [TestMethod]
        public void AdapterHandlers_contains_RejectOrderOnUpdateOrder_test()
        {
            Assert.IsNotNull(handlers.Get<RejectOrderOnUpdateOrder>());
        }

        [TestMethod]
        public void AdapterHandlers_contains_CancelOrderOnUpdateOrder_test()
        {
            Assert.IsNotNull(handlers.Get<CancelOrderOnUpdateOrder>());
        }

        [TestMethod]
        public void AdapterHandlers_contains_MakeOrderDeliveryConfirmationOnOrderSucceeded_test()
        {
            Assert.IsNotNull(handlers.Get<MakeOrderDeliveryConfirmationOnOrderSucceeded>());
        }

        [TestMethod]
        public void AdapterHandlers_contains_UpdateOrderBookOnBidAsk_test()
        {
            Assert.IsNotNull(handlers.Get<UpdateOrderBookOnBidAsk>());
        }

        [TestMethod]
        public void AdapterHandlers_contains_MakeCookieToOrderNoAssociationOnUpdateOrder_test()
        {
            Assert.IsNotNull(handlers.Get<MakeCookieToOrderNoAssociationOnUpdateOrder>());
        }

        [TestMethod]
        public void AdapterHandlers_contains_MakePendingTradeInfoOnTradeInfo_test()
        {
            Assert.IsNotNull(handlers.Get<MakePendingTradeInfoOnTradeInfo>());
        }

        [TestMethod]
        public void AdapterHandlers_contains_MakeTradeOnCookieToOrderNoAssociation_test()
        {
            Assert.IsNotNull(handlers.Get<MakeTradeOnCookieToOrderNoAssociation>());
        }

        [TestMethod]
        public void AdapterHandlers_contains_MakeTradeOnPendingTradeInfo_test()
        {
            Assert.IsNotNull(handlers.Get<MakeTradeOnPendingTradeInfo>());
        }

        [TestMethod]
        public void AdapterHandlers_contains_MakeCookieToOrderNoAssociationOnUpdateOrderWithZeroCookie_test()
        {
            Assert.IsNotNull(handlers.Get<MakeCookieToOrderNoAssociationOnUpdateOrderWithZeroCookie>());
        }

        [TestMethod]
        public void AdapterHandlers_contains_CancelOrderOnUpdateOrderWithZeroCookie_test()
        {
            Assert.IsNotNull(handlers.Get<CancelOrderOnUpdateOrderWithWrongCookie>());
        }

        [TestMethod]
        public void AdapterHandlers_contains_ExpireOrderOnUpdateOrder_test()
        {
            Assert.IsNotNull(handlers.Get<ExpireOrderOnUpdateOrder>());
        }
    }
}
