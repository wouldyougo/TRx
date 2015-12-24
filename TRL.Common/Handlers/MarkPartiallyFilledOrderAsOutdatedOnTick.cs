using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
//using TRL.Common.Extensions;
using TRL.Common.Handlers;
using TRL.Common.Models;
using TRL.Logging;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Handlers
{
    public class MarkPartiallyFilledOrderAsOutdatedOnTick:AddedItemHandler<Tick>
    {
        private StrategyHeader strategyHeader;
        private int outdateSeconds;
        private ILogger logger;
        private IDataContext tradingData;

        public MarkPartiallyFilledOrderAsOutdatedOnTick(StrategyHeader strategyHeader, int outdateSeconds, IDataContext tradingData, ILogger logger)
            :base(tradingData.Get<ObservableCollection<Tick>>())
        {
            this.strategyHeader = strategyHeader;
            this.outdateSeconds = outdateSeconds;
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public override void OnItemAdded(Tick item)
        {
            if (strategyHeader.Symbol != item.Symbol)
                return;

            double amount = this.tradingData.GetAmount(this.strategyHeader);

            if (amount == 0)
                return;

            IEnumerable<Order> filledPartially = this.tradingData.GetFilledPartially(this.strategyHeader, OrderType.Limit);

            if (filledPartially == null)
                return;

            if (filledPartially.Count() == 0)
                return;

            Order o = filledPartially.First();

            if (o.TradeAction == TradeAction.Buy && amount < 0)
                return;

            if (o.TradeAction == TradeAction.Sell && amount > 0)
                return;

            if (CancelOrderRequestExists(o.Id))
            {
                if(!CancelOrderRequestIsOutdated(o.Id))
                    return;
            }

            if (o.DateTime.AddSeconds(this.outdateSeconds) > item.DateTime)
                return;

            string descr = String.Format("Отменить заявку {0}, потому что она не исполнилась полностью в течение {1} секунд", o.ToString(), this.outdateSeconds);
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, {2}", DateTime.Now, this.GetType().Name, descr));
            this.tradingData.Get<ICollection<OrderCancellationRequest>>().Add(new OrderCancellationRequest(o, descr));
        }

        private bool CancelOrderRequestExists(int orderId)
        {
            return this.tradingData.Get<IEnumerable<OrderCancellationRequest>>().Any(o => o.OrderId == orderId);
        }

        private bool CancelOrderRequestIsOutdated(int orderId)
        {
            return this.tradingData.Get<IEnumerable<OrderCancellationRequest>>().Any(o => o.OrderId == orderId
                && o.DateTime < o.DateTime.AddSeconds(this.outdateSeconds * 3));
        }

    }
}
