using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Common.Models;
using TRL.Common.Handlers;
using TRL.Connect.Smartcom.Models;
using TRL.Common.TimeHelpers;
using TRL.Connect.Smartcom.Data;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Handlers
{
    public class RejectOrderOnOrderFailed:GenericCollectionObserver<OrderFailed>
    {
        private IDataContext tradingData;
        private ILogger logger;

        public RejectOrderOnOrderFailed()
            : this(TradingData.Instance, RawTradingData.Instance, DefaultLogger.Instance) { }

        public RejectOrderOnOrderFailed(IDataContext tradingData, BaseDataContext rawTradingData, ILogger logger)
            :base(rawTradingData)
        {
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public override void Update(OrderFailed item)
        {
            if (IsDuplicateOrderFailed(item))
                return;

            Order order = FindOrder(item.Cookie);

            if (order == null)
                return;

            order.Reject(BrokerDateTime.Make(DateTime.Now), item.Reason);
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, заявка отклонена {2}.", 
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name, 
                order.ToString()));
        }

        private bool IsDuplicateOrderFailed(OrderFailed item)
        {
            try
            {
                return this.dataContext.GetData<OrderFailed>().Where(u => u.OrderId == item.OrderId && u.Cookie == item.Cookie).Count() > 1;
            }
            catch
            {
                return false;
            }
        }

        private Order FindOrder(int id)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<Order>>().Single(o => o.Id == id);
            }
            catch
            {
                return null;
            }
        }
    }
}
