using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Common.Extensions;
using TRL.Common.Data;
using TRL.Logging;

namespace TRL.Common.Handlers
{
    public class CancelOrderOnCancellationConfirmation:AddedItemHandler<OrderCancellationConfirmation>
    {
        private IDataContext tradingData;
        private ILogger logger;

        public CancelOrderOnCancellationConfirmation(IDataContext tradingData, ILogger logger)
            : base(tradingData.Get<ObservableHashSet<OrderCancellationConfirmation>>()) 
        {
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public override void OnItemAdded(OrderCancellationConfirmation item)
        {
            Order order = this.tradingData.Get<IEnumerable<Order>>().SingleOrDefault(o => o.Id == item.OrderId);

            if (order == null)
                return;

            order.Cancel(item.DateTime, item.Description);
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, заявка отменена {2}.", DateTime.Now, this.GetType().Name, order.ToString()));
        }
    }
}
