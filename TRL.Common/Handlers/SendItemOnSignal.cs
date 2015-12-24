using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Events;
using TRL.Common.Models;
using TRL.Common.Extensions.Collections;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.TimeHelpers;
using TRL.Logging;

namespace TRL.Common.Handlers
{
    public class SendItemOnSignal:IObserver
    {
        //private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        //private ObservableQueue<Order> orderQueue;
        //private ITradingSchedule schedule;
        //private ILogger logger;

        /// <summary>
        /// сторонний обработчик
        /// </summary>
        private ItemAddedNotification<Signal> SignalHandler;


        public SendItemOnSignal(ObservableQueue<Signal> signalQueue)
        {
            this.signalQueue = signalQueue;
            this.signalQueue.RegisterObserver(this);
            //this.orderQueue = orderQueue;
            //this.tradingData = tradingData;
            //this.schedule = schedule;
            //this.logger = logger;
        }

        public void Update()
        {
            Signal signal = this.signalQueue.Peek();
            SignalHandler.Invoke(signal);

            //if (!this.schedule.ItIsTimeToTrade(BrokerDateTime.Make(DateTime.Now)))
            //{
            //    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал удален, потому что в настоящий момент торгов нет.", DateTime.Now, this.GetType().Name));
            //    return;
            //}

            //IEnumerable<Order> unfilledOrders = this.tradingData.Get<ICollection<Order>>().GetUnfilledOrderJustLikeASignal(signal);

            //if (unfilledOrders != null && unfilledOrders.Count() > 0)
            //{
            //    foreach(Order o in unfilledOrders)
            //        this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал удален, потому что существуют неисполненные заявки для стратегии {2}.", DateTime.Now, this.GetType().Name, o.ToString()));

            //    return;
            //}

            //this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, формирование заявки по сигналу {2}", DateTime.Now, this.GetType().Name, signal.ToString()));
            //Order order = new Order(signal);
        }

        /// <summary>
        /// добавить сторонний обработчик
        /// </summary>
        /// <param name="handler"></param>
        public void AddedItemHandler(ItemAddedNotification<Signal> signalHandler)
        {
            //this.notifier.OnItemAdded += new ItemAddedNotification<Bar>(OnItemAdded);
            //this.notifier.OnItemAdded += handler;
            this.SignalHandler = signalHandler;
        }
    }
}
