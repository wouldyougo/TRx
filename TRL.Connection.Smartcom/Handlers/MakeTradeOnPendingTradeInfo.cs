using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Handlers;
using TRL.Common.Models;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Common.TimeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Handlers
{
    public class MakeTradeOnPendingTradeInfo : GenericCollectionObserver<PendingTradeInfo>
    {
        private IDataContext tradingData;
        private ILogger logger;
        private CookieToOrderNoAssociation cookieAndOrderNoAssociation;
        private Order order;

        public MakeTradeOnPendingTradeInfo()
            : this(RawTradingData.Instance, TradingData.Instance, DefaultLogger.Instance) { }

        public MakeTradeOnPendingTradeInfo(BaseDataContext rawData,
            IDataContext tradingData,
            ILogger logger)
            :base(rawData)
        {
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public override void Update(PendingTradeInfo item)
        {
            UpdateCookieAndOrderNoAssociationFor(item);

            if (!HasCookieAndOrderNoAssociation())
                return;

            UpdateOrder();

            if (!HasOrder())
            {
                this.cookieAndOrderNoAssociation = null;
                return;
            }

            if (this.order.IsFilled)
            {
                this.order = null;
                return;
            }

            Trade trade = new Trade(this.order, item.Portfolio, item.Symbol, item.Price, item.Amount, item.DateTime);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, исполнена сделка {2}",
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name,
                trade.ToString()));

            this.dataContext.GetData<PendingTradeInfo>().Remove(item);
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(trade);
            
            this.cookieAndOrderNoAssociation = null;
            this.order = null;
        }

        private bool HasOrder()
        {
            return this.order != null;
        }

        private void UpdateOrder()
        {
            try { 
                this.order = 
                    this.tradingData.Get<IEnumerable<Order>>().SingleOrDefault(i => i.Id == this.cookieAndOrderNoAssociation.Cookie);
            }
            catch
            {
                this.order = null;
            }
        }

        private bool HasCookieAndOrderNoAssociation()
        {
            return this.cookieAndOrderNoAssociation != null;
        }

        private void UpdateCookieAndOrderNoAssociationFor(PendingTradeInfo item)
        {
            try { 
                this.cookieAndOrderNoAssociation = 
                    this.dataContext.GetData<CookieToOrderNoAssociation>().Where(i => i.OrderNo == item.OrderNo).Last();
            }
            catch
            {
                this.cookieAndOrderNoAssociation = null;
            }
        }


    }
}
