using TRL.Common.Data;
using TRL.Common.Handlers;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Common.TimeHelpers;
using SmartCOM3Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Handlers
{
    public class MakeCookieToOrderNoAssociationOnUpdateOrderWithZeroCookie : GenericCollectionObserver<UpdateOrder>
    {
        private ILogger logger;
        private UpdateOrder sourceUpdateOrder;

        public MakeCookieToOrderNoAssociationOnUpdateOrderWithZeroCookie() :
            this(RawTradingData.Instance,
             DefaultLogger.Instance) { }

        public MakeCookieToOrderNoAssociationOnUpdateOrderWithZeroCookie(BaseDataContext rawData, ILogger logger)
            :base(rawData)
        {
            this.logger = logger;
        }

        public override void Update(UpdateOrder item)
        {
            if (item.Cookie != 0)
                return;

            if (!item.CanContainFillingMark())
                return;

            UpdateSourceUpdateOrder(item);

            if (!HasSourceUpdateOrder())
                return;

            RegisterCookieToOrderNoAssociation(item);

            this.sourceUpdateOrder = null;
        }

        private bool HasSourceUpdateOrder()
        {
            return this.sourceUpdateOrder != null;
        }

        private void UpdateSourceUpdateOrder(UpdateOrder item)
        {
            this.sourceUpdateOrder = this.dataContext.GetData<UpdateOrder>().SingleOrDefault(i => i.OrderNo == item.OrderId);
        }

        private void RegisterCookieToOrderNoAssociation(UpdateOrder item)
        {
            CookieToOrderNoAssociation association = new CookieToOrderNoAssociation(this.sourceUpdateOrder.Cookie, item.OrderNo);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, {2}",
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name,
                association.ToString()));

            this.dataContext.GetData<CookieToOrderNoAssociation>().Add(association);
        }
    }
}
