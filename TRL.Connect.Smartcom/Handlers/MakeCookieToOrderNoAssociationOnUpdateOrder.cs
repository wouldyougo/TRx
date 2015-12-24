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
    public class MakeCookieToOrderNoAssociationOnUpdateOrder : GenericCollectionObserver<UpdateOrder>
    {
        private ILogger logger;

        public MakeCookieToOrderNoAssociationOnUpdateOrder() :
            this(RawTradingData.Instance,
             DefaultLogger.Instance) { }

        public MakeCookieToOrderNoAssociationOnUpdateOrder(BaseDataContext rawData, ILogger logger)
            :base(rawData)
        {
            this.logger = logger;
        }

        public override void Update(UpdateOrder item)
        {
            if (item.Cookie == 0)
                return;

            if (item.OrderNo == "0")
                return;

            if(!item.CanContainFillingMark())
                return;

            if (DuplicateExistsFor(item))
                return;

            RegisterTradeInfoPending(item);
        }

        private void RegisterTradeInfoPending(UpdateOrder item)
        {
            CookieToOrderNoAssociation expectedTradeInfo = new CookieToOrderNoAssociation(item.Cookie, item.OrderNo);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, {2}",
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name,
                expectedTradeInfo.ToString()));

            this.dataContext.GetData<CookieToOrderNoAssociation>().Add(expectedTradeInfo);
        }

        private bool DuplicateExistsFor(UpdateOrder item)
        {
            try
            {
                return this.dataContext.GetData<UpdateOrder>().Where(i => i.Cookie == item.Cookie
                    && i.OrderId == item.OrderId
                    && i.OrderNo == item.OrderNo
                    && i.OrderUnfilled == item.OrderUnfilled).Count() > 1;
            }
            catch
            {
                return false;
            }
        }

    }
}
