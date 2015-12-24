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
    public class SendItemOnOrder:IObserver
    {
        //private IDataContext tradingData;
        private ObservableQueue<Order> orderQueue;
        //private IOrderManager manager;
        //private ILogger logger;

        //public OrderQueueProcessor(IOrderManager manager, IDataContext tradingData, ObservableQueue<Order> orderQueue, ILogger logger)
        //{
        //    this.manager = manager;
        //    this.tradingData = tradingData;
        //    this.orderQueue = orderQueue;
        //    this.logger = logger;

        //    this.orderQueue.RegisterObserver(this);
        //}

        /// <summary>
        /// сторонний обработчик
        /// </summary>
        private ItemAddedNotification<Order> OrderHandler;

        public SendItemOnOrder(ObservableQueue<Order> orderQueue)
        {
            this.orderQueue = orderQueue;
            this.orderQueue.RegisterObserver(this);
        }


        public void Update()
        {
            Order order = this.orderQueue.Dequeue();
            OrderHandler.Invoke(order);

            //if (OrderExists(order))
            //    return;

            //this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, отправка заявки {2}.", DateTime.Now, this.GetType().Name, order.ToString()));

            //if (order.Signal != null)
            //    CopyOrderAsCloseOrAsOpen(order);

            //this.manager.PlaceOrder(order);

            //this.tradingData.Get<ObservableHashSet<Order>>().Add(order);
        }
        /// <summary>
        /// добавить сторонний обработчик
        /// </summary>
        /// <param name="handler"></param>
        public void AddedItemHandler(ItemAddedNotification<Order> orderHandler)
        {
            //this.notifier.OnItemAdded += new ItemAddedNotification<Bar>(OnItemAdded);
            //this.notifier.OnItemAdded += handler;
            this.OrderHandler = orderHandler;
        }
    }
}
