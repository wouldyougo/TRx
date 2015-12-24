using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Extensions;
using TRL.Common.Collections;
using TRL.Logging;

namespace TRL.Common.Handlers
{
    public class CancelOrderOnCancellationRequest:AddedItemHandler<OrderCancellationRequest>
    {
        private IOrderManager manager;
        private ILogger logger;

        public CancelOrderOnCancellationRequest(IOrderManager manager, IDataContext tradingData, ILogger logger)
            :base(tradingData.Get<ObservableHashSet<OrderCancellationRequest>>())
        {
            this.manager = manager;
            this.logger = logger;
        }

        public override void OnItemAdded(OrderCancellationRequest item)
        {
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, отправляется запрос на отмену заявки {2}.", DateTime.Now, this.GetType().Name, item.Order.ToString()));
            this.manager.CancelOrder(item.Order);
        }
    }
}
