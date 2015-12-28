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
    public class MakeTradeOnCookieToOrderNoAssociation : GenericCollectionObserver<CookieToOrderNoAssociation>
    {
        private IDataContext tradingData;
        private ILogger logger;
        private List<PendingTradeInfo> pendingTradeInfoCollection;
        private Order order;

        public MakeTradeOnCookieToOrderNoAssociation()
            : this(RawTradingData.Instance, TradingData.Instance, DefaultLogger.Instance) { }

        public MakeTradeOnCookieToOrderNoAssociation(BaseDataContext rawData,
            IDataContext tradingData,
            ILogger logger)
            :base(rawData)
        {
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public override void Update(CookieToOrderNoAssociation item)
        {
            MakePendingTradeInfoCollection(item);

            if (IsEmptyPendingTradeInfoCollection())
                return;

            UpdateOrder(item);

            if (!HasOrder())
            {
                this.pendingTradeInfoCollection = null;
                return;
            }

            if (this.order.IsFilled) 
            {
                this.order = null;
                return;
            }

            MakeTrades();
            
            this.pendingTradeInfoCollection = null;
            this.order = null;
        }

        private void MakeTrades()
        {
            foreach (PendingTradeInfo item in this.pendingTradeInfoCollection)
            {

                Trade trade =
                    new Trade(this.order,
                        item.Portfolio,
                        item.Symbol,
                        item.Price,
                        item.Amount,
                        item.DateTime);

                this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, исполнена сделка {2}",
                    BrokerDateTime.Make(DateTime.Now),
                    this.GetType().Name,
                    trade.ToString()));

                this.dataContext.GetData<PendingTradeInfo>().Remove(item);
                this.tradingData.Get<ObservableHashSet<Trade>>().Add(trade);
            }
        }

        private bool HasOrder()
        {
            return this.order != null;
        }

        private void UpdateOrder(CookieToOrderNoAssociation item)
        {
            try
            {
                this.order = this.tradingData.Get<IEnumerable<Order>>().SingleOrDefault(i => i.Id == item.Cookie);
            }
            catch
            {
                this.order = null;
            }
        }

        private bool IsEmptyPendingTradeInfoCollection()
        {
            if (this.pendingTradeInfoCollection == null)
                return true;

            return this.pendingTradeInfoCollection.Count() == 0;
        }

        private void MakePendingTradeInfoCollection(CookieToOrderNoAssociation item)
        {
            try
            {
                this.pendingTradeInfoCollection = new List<PendingTradeInfo>(
                    this.dataContext.GetData<PendingTradeInfo>().Where(i => i.OrderNo == item.OrderNo));
            }
            catch
            {
                this.pendingTradeInfoCollection = null;
            }
        }

    }
}
