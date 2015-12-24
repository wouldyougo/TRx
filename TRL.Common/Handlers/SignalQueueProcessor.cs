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
    public class SignalQueueProcessor:IObserver
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ObservableQueue<Order> orderQueue;
        private ITradingSchedule schedule;
        private ILogger logger;

        public SignalQueueProcessor() :
            this(SignalQueue.Instance,
             OrderQueue.Instance,
             TradingData.Instance,
             new FortsTradingSchedule(),
             DefaultLogger.Instance) { }

        public SignalQueueProcessor(ObservableQueue<Signal> signalQueue, 
            ObservableQueue<Order> orderQueue, 
            IDataContext tradingData, 
            ITradingSchedule schedule,
            ILogger logger)
        {
            this.signalQueue = signalQueue;
            this.orderQueue = orderQueue;
            this.tradingData = tradingData;
            this.schedule = schedule;
            this.logger = logger;

            this.signalQueue.RegisterObserver(this);
        }

        public void Update()
        {
            Signal signal = this.signalQueue.Dequeue();

            // переделать
            //if (!this.schedule.ItIsTimeToTrade(BrokerDateTime.Make(DateTime.Now)))
            //{
            //    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал удален, потому что в настоящий момент торгов нет.", DateTime.Now, this.GetType().Name));
            //    return;
            //}

            IEnumerable<Order> unfilledOrders = this.tradingData.Get<ICollection<Order>>().GetUnfilledOrderJustLikeASignal(signal);

            if (unfilledOrders != null && unfilledOrders.Count() > 0)
            {
                foreach(Order o in unfilledOrders)
                    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал удален, потому что существуют неисполненные заявки для стратегии {2}.", DateTime.Now, this.GetType().Name, o.ToString()));

                return;
            }

            SaveSignal(signal);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, формирование заявки по сигналу {2}", DateTime.Now, this.GetType().Name, signal.ToString()));

            Order order = new Order(signal);

            this.orderQueue.Enqueue(order);
        }

        private void SaveSignal(Signal signal)
        {
            this.tradingData.Get<ObservableHashSet<Signal>>().Add(signal);
        }
    }
}
