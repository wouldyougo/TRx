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
    public class RejectOrderOnUpdateOrder:GenericCollectionObserver<UpdateOrder>
    {
        private IDataContext tradingData;
        private ILogger logger;

        public RejectOrderOnUpdateOrder()
            : this(TradingData.Instance, RawTradingData.Instance, DefaultLogger.Instance) { }

        public RejectOrderOnUpdateOrder(IDataContext strategyData, RawTradingDataContext rawData, ILogger logger)
            :base(rawData)
        {
            this.tradingData = strategyData;
            this.logger = logger;
        }

        public override void Update(UpdateOrder item)
        {
            if (!IsRejectUpdate(item))
                return;

            Order order = FindOrder(item);

            if (order == null)
                return;

            order.Reject(item.Datetime, item.State.ToString());

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, заявка отклонена {2}", 
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name, 
                order.ToString()));
        }

        private Order FindOrder(UpdateOrder update)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<Order>>().Single(o => o.Id == update.Cookie
                    && !o.IsFilled
                    && !o.IsRejected
                    && !o.IsExpired);
            }
            catch
            {
                return null;
            }
        }

        private bool IsRejectUpdate(UpdateOrder update)
        {
            return update.State == StOrder_State.StOrder_State_ContragentReject
                || update.State == StOrder_State.StOrder_State_SystemReject;
        }
    }
}
