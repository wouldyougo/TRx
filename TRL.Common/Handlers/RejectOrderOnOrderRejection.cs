using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.Extensions;
using TRL.Common.Collections;
using TRL.Logging;

namespace TRL.Common.Handlers
{
    public class RejectOrderOnOrderRejection:AddedItemHandler<OrderRejection>
    {
        private IDataContext tradingData;
        private ILogger logger;

        public RejectOrderOnOrderRejection(IDataContext tradingData, ILogger logger)
            : base(tradingData.Get<ObservableHashSet<OrderRejection>>())
        {
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public override void OnItemAdded(OrderRejection item)
        {
            if (!OrderExists(item.Order))
                return;

            Order order = this.tradingData.Get<IEnumerable<Order>>().Single(o => o.Id == item.OrderId);
            
            order.Reject(item.DateTime, item.Description);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, заявка отклонена {2}.", DateTime.Now, this.GetType().Name, order.ToString()));
        }

        private bool OrderExists(Order order)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<Order>>().Contains(order);
            }
            catch
            {
                return false;
            }
        }

    }
}
