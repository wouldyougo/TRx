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
    public class CancelOrderOnUpdateOrder:GenericCollectionObserver<UpdateOrder>
    {
        private IDataContext tradingData;
        private ILogger logger;

        public CancelOrderOnUpdateOrder()
            : this(TradingData.Instance, RawTradingData.Instance, DefaultLogger.Instance) { }

        public CancelOrderOnUpdateOrder(IDataContext tradingData, RawTradingDataContext rawData, ILogger logger)
            :base(rawData)
        {
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public override void Update(UpdateOrder item)
        {
            if (!IsCancelUpdate(item))
                return;

            Order order = FindOrder(item);

            if (order == null)
                return;

            order.Cancel(item.Datetime, item.State.ToString());

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, заявка отменена {2}", 
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name, 
                order.ToString()));
        }

        private Order FindOrder(UpdateOrder update)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<Order>>().SingleOrDefault(o => o.Id == update.Cookie
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
