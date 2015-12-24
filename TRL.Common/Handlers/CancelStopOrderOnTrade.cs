using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Data;
//using TRL.Common.Extensions;
using TRL.Common.Handlers;
using TRL.Common.Models;
//using TRL.Common.Extensions.Models;
using TRL.Common.Extensions.Collections;
using TRL.Logging;

namespace TRL.Common.Handlers
{
    public class CancelStopOrderOnTrade:AddedItemHandler<Trade>
    {
        private IDataContext tradingData;
        private ILogger logger;

        public CancelStopOrderOnTrade(IDataContext tradingData, ILogger logger)
            :base(tradingData.Get<ObservableHashSet<Trade>>())
        {
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public override void OnItemAdded(Trade item)
        {
            if (item.Order.OrderType == OrderType.Stop)
                return;

            if (item.Order.IsFilled)
                return;

            if (item.Order.Signal == null)
                return;

            if (item.Order.Signal.Strategy == null)
                return;

            Order stopOrder = FindStopOrder(item.Order.Signal.Strategy);

            if (stopOrder == null)
                return;

            if (CancelOrderRequestExists(stopOrder.Id))
                return;

            string descr = String.Format("Отменить стоп заявку {0}, потому что лимитная заявка {1} исполнилась лишь частично.", stopOrder.ToString(), item.Order.ToString());
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сформирован запрос на отмену стоп заявки {2}", DateTime.Now, this.GetType().Name, descr));
            this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Add(new OrderCancellationRequest(stopOrder, descr));
        }

        private bool CancelOrderRequestExists(int orderId)
        {
            return this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Any(o => o.OrderId == orderId);
        }

        private Order FindStopOrder(StrategyHeader strategyHeader)
        {
            try
            {
                return this.tradingData.Get<ICollection<Order>>().GetUnfilled(strategyHeader).Single(o => o.OrderType == OrderType.Stop);
            }
            catch
            {
                return null;
            }
        }
    }
}
