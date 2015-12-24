using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Data;
using TRL.Common.Handlers;
using TRL.Connect.Smartcom.Handlers;

namespace TRL.Connect.Smartcom.Data
{
    public class AdapterHandlers:RawBaseDataContext
    {
        public RejectOrderOnOrderFailed RejectOrderOnOrderFailedHandler { get; private set; }
        public RejectOrderOnUpdateOrder RejectOrderOnUpdateOrderHandler { get; private set; }
        public CancelOrderOnUpdateOrder CancelOrderOnUpdateOrderHandler { get; private set; }
        public CancelOrderOnUpdateOrderWithWrongCookie CancelOrderOnUpdateOrderWithWrongCookie { get; private set; }
        public MakeOrderDeliveryConfirmationOnOrderSucceeded MakeOrderDeliveryConfirmationOnOrderSucceededHandler { get; private set; }
        public UpdateOrderBookOnBidAsk UpdateOrderBookOnBidAskHandler { get; private set; }
        public MakeCookieToOrderNoAssociationOnUpdateOrder MakeExpectedTradeInfoOnUpdateOrderHandler { get; private set; }
        public MakePendingTradeInfoOnTradeInfo MakeExpectedUpdateOrderOnTradeInfo { get; private set; }
        public MakeCookieToOrderNoAssociationOnUpdateOrderWithZeroCookie MakeCookieToOrderNoAssociationOnUpdateOrderWithZeroCookie { get; private set; }
        public MakeTradeOnCookieToOrderNoAssociation MakeTradeOnExpectedTradeInfo { get; private set; }
        public MakeTradeOnPendingTradeInfo MakeTradeOnExpectedUpdateOrder { get; private set; }
        public ExpireOrderOnUpdateOrder ExpireOrderOnUpdateOrder { get; private set; }

        public AdapterHandlers()
        {
            this.RejectOrderOnOrderFailedHandler = 
                new RejectOrderOnOrderFailed();
            
            this.RejectOrderOnUpdateOrderHandler = 
                new RejectOrderOnUpdateOrder();
            
            this.CancelOrderOnUpdateOrderHandler = 
                new CancelOrderOnUpdateOrder();

            this.CancelOrderOnUpdateOrderWithWrongCookie =
                new CancelOrderOnUpdateOrderWithWrongCookie();
            
            this.MakeOrderDeliveryConfirmationOnOrderSucceededHandler = 
                new MakeOrderDeliveryConfirmationOnOrderSucceeded();

            this.UpdateOrderBookOnBidAskHandler =
                new UpdateOrderBookOnBidAsk();

            this.MakeExpectedTradeInfoOnUpdateOrderHandler =
                new MakeCookieToOrderNoAssociationOnUpdateOrder();

            this.MakeExpectedUpdateOrderOnTradeInfo =
                new MakePendingTradeInfoOnTradeInfo();

            this.MakeCookieToOrderNoAssociationOnUpdateOrderWithZeroCookie =
                new MakeCookieToOrderNoAssociationOnUpdateOrderWithZeroCookie();

            this.MakeTradeOnExpectedTradeInfo =
                new MakeTradeOnCookieToOrderNoAssociation();

            this.MakeTradeOnExpectedUpdateOrder =
                new MakeTradeOnPendingTradeInfo();

            this.ExpireOrderOnUpdateOrder =
                new ExpireOrderOnUpdateOrder();
        }

    }
}
