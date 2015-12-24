using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Connect.Smartcom.Models;
using TRL.Common.Handlers;
using TRL.Configuration;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Models;
using SmartCOM3Lib;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Handlers
{
    public class CancelOrderOnUpdateOrderWithWrongCookie:GenericCollectionObserver<UpdateOrder>
    {
        private IDataContext tradingData;
        private ILogger logger;

        public CancelOrderOnUpdateOrderWithWrongCookie()
            : this(TradingData.Instance, RawTradingData.Instance, DefaultLogger.Instance) { }

        public CancelOrderOnUpdateOrderWithWrongCookie(IDataContext tradingData, RawTradingDataContext rawData, ILogger logger)
            :base(rawData)
        {
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public override void Update(UpdateOrder item)
        {
            if (!IsCancelUpdate(item))
                return;

            CookieToOrderNoAssociation association = FindCookieToOrderNoAssociation(item.OrderNo);

            if (association == null)
                association = FindCookieToOrderNoAssociation(item.OrderId);

            if (association == null)
                return;

            Order order = FindOrder(association.Cookie);

            if (order == null)
                return;

            order.Cancel(item.Datetime, item.State.ToString());

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, заявка отменена {2}", 
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name, 
                order.ToString()));
        }

        private CookieToOrderNoAssociation FindCookieToOrderNoAssociation(string orderNo)
        {
            try
            {
                return this.dataContext.GetData<CookieToOrderNoAssociation>().SingleOrDefault(a => a.OrderNo == orderNo);
            }
            catch
            {
                return null;
            }
        }

        private Order FindOrder(int id)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<Order>>().SingleOrDefault(o => o.Id == id
                    && !o.IsFilled
                    && !o.IsRejected
                    && !o.IsExpired
                    && !o.IsCanceled);
            }
            catch
            {
                return null;
            }
        }

        private bool IsCancelUpdate(UpdateOrder update)
        {
            return update.State == StOrder_State.StOrder_State_Cancel ||
                update.State == StOrder_State.StOrder_State_SystemCancel ||
                update.State == StOrder_State.StOrder_State_ContragentCancel;
        }
    }
}
