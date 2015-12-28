using TRL.Common.Data;
using TRL.Common.Handlers;
using TRL.Common.Models;
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
    public class ExpireOrderOnUpdateOrder:GenericCollectionObserver<UpdateOrder>
    {
        private IDataContext tradingData;
        private ILogger logger;

        private Order order;

        public ExpireOrderOnUpdateOrder()
            : this(TradingData.Instance, RawTradingData.Instance, DefaultLogger.Instance) { }

        public ExpireOrderOnUpdateOrder(IDataContext tradingData, BaseDataContext rawData, ILogger logger)
            :base(rawData)
        {
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public override void Update(UpdateOrder item)
        {
            if (item.State != StOrder_State.StOrder_State_Expired)
                return;

            if (item.Cookie == 0)
                return;

            SetOrder(item.Cookie);

            if (this.order == null)
                return;

            this.order.ExpirationDate = item.Datetime;

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, срок действия заявки истек {2}.",
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name,
                order.ToString()));

            this.order = null;
        }

        private void SetOrder(int id)
        {
            try
            {
                this.order = this.tradingData.Get<IEnumerable<Order>>().SingleOrDefault(o => o.Id == id);
            }
            catch
            {
                this.order = null;
            }
        }
    }
}
