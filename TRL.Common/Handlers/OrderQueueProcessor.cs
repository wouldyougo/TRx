using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Events;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Logging;
//using TRL.Common.Extensions;

namespace TRL.Common.Handlers
{
    public class OrderQueueProcessor:IObserver
    {
        private IDataContext tradingData;
        private ObservableQueue<Order> orderQueue;
        private IOrderManager manager;
        private ILogger logger;

        public OrderQueueProcessor(IOrderManager manager, IDataContext tradingData, ObservableQueue<Order> orderQueue, ILogger logger)
        {
            this.manager = manager;
            this.tradingData = tradingData;
            this.orderQueue = orderQueue;
            this.logger = logger;

            this.orderQueue.RegisterObserver(this);
        }

        public void Update()
        {
            Order order = this.orderQueue.Dequeue();

            if (OrderExists(order))
                return;

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, отправка заявки {2}.", DateTime.Now, this.GetType().Name, order.ToString()));

            if (order.Signal != null)
                CopyOrderAsCloseOrAsOpen(order);

            this.manager.PlaceOrder(order);

            this.tradingData.Get<ObservableHashSet<Order>>().Add(order);
        }

        private void CopyOrderAsCloseOrAsOpen(Order order)
        {
            double amount = this.tradingData.GetAmount(order.Signal.Strategy);

            if (amount == 0)
                this.tradingData.Get<ObservableHashSet<OpenOrder>>().Add(new OpenOrder(order));

            if (amount > 0 && order.TradeAction == TradeAction.Buy)
                this.tradingData.Get<ObservableHashSet<OpenOrder>>().Add(new OpenOrder(order));

            if (amount > 0 && order.TradeAction == TradeAction.Sell)
                this.tradingData.Get<ObservableHashSet<CloseOrder>>().Add(new CloseOrder(order));

            if (amount < 0 && order.TradeAction == TradeAction.Sell)
                this.tradingData.Get<ObservableHashSet<OpenOrder>>().Add(new OpenOrder(order));

            if (amount < 0 && order.TradeAction == TradeAction.Buy)
                this.tradingData.Get<ObservableHashSet<CloseOrder>>().Add(new CloseOrder(order));
        }

        private bool OrderExists(Order order)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<Order>>().Any(o => o.Id == order.Id);
            }
            catch
            {
                return false;
            }
        }
    }
}
