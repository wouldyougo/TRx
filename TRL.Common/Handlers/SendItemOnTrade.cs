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
//using TRL.Common.Extensions.Models;
using TRL.Common.Extensions.Collections;
using TRL.Logging;
using TRL.Common.Events;

namespace TRL.Common.Handlers
{
    public class SendItemOnTrade:AddedItemHandler<Trade>
    {
        private IDataContext tradingData;
        private ILogger logger;

        /// <summary>
        /// список сторонних обработчиков
        /// </summary>
        private IList<ItemAddedNotification<Trade>> Handlers;


        public SendItemOnTrade(IDataContext tradingData, ILogger logger)
            :base(tradingData.Get<ObservableHashSet<Trade>>())
        {
            this.tradingData = tradingData;
            this.logger = logger;
            this.Handlers = new List<ItemAddedNotification<Trade>>();
        }

        public override void OnItemAdded(Trade item)
        {
            //Strategy strategyHeader = item.Order.Signal.Strategy;

            //if (strategyHeader == null)
            //    return;

            //IEnumerable<Order> unfilled = this.tradingData.Get<ICollection<Order>>().GetUnfilled(strategyHeader);

            //if (unfilled == null || unfilled.Count() == 0)
            //    return;

            //foreach (Order o in unfilled)
            //{
            //    if (this.tradingData.GetAmount(strategyHeader) == 0)
            //    {
            //        if (!CancelOrderRequestExists(o.Id))
            //        {

            //            string descr = String.Format("Отменить заявку {0}, потому что позиция была закрыта заявкой {1}", o.ToString(), item.Order.ToString());
            //            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, {2}", DateTime.Now, this.GetType().Name, descr));
            //            this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Add(new OrderCancellationRequest(o, descr));
            //        }
            //    }
            //}
            foreach (var handler in Handlers)
            {
                handler.Invoke(item);
            }                
        }
        
        /// <summary>
        /// добавить сторонний обработчик
        /// </summary>
        /// <param name="handler"></param>
        public void AddItemHandler(ItemAddedNotification<Trade> handler)
        {
            //this.notifier.OnItemAdded += new ItemAddedNotification<Bar>(OnItemAdded);
            //this.notifier.OnItemAdded += handler;
            this.Handlers.Add(handler);
        }
    }
}
