using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Handlers;
using TRL.Logging;

namespace TRL.Common.Handlers
{
    public class UpdateOrderOnOrderDeliveryConfirmation:AddedItemHandler<OrderDeliveryConfirmation>
    {
        private ILogger logger;

        public UpdateOrderOnOrderDeliveryConfirmation(IDataContext tradingData, ILogger logger)
            : base(tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>())
        {
            this.logger = logger;
        }

        public override void OnItemAdded(OrderDeliveryConfirmation item)
        {
            if (item.Order.IsDelivered)
                return;

            item.Order.DeliveryDate = item.DateTime;
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, брокер подтвердил получение заявки {2}.", DateTime.Now, this.GetType().Name, item.Order.ToString()));
        }

    }
}
