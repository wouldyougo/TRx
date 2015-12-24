using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Common.Events;

namespace TRL.Common.Handlers
{
    public class SendItemOnTick : AddedItemHandler<Tick>
    {
        private IDataContext tradingData;
        
        /// <summary>
        /// список сторонних обработчиков
        /// </summary>
        private IList<ItemAddedNotification<Tick>> Handlers;


        public SendItemOnTick(IDataContext tradingData)
            : base(tradingData.Get<ObservableCollection<Tick>>())
        {
            this.tradingData = tradingData;
            this.Handlers = new List<ItemAddedNotification<Tick>>();
        }
        public override void OnItemAdded(Tick Tick)
        {
            foreach (var handler in Handlers)
            {
                handler.Invoke(Tick);
            }
        }
        
        /// <summary>
        /// добавить сторонний обработчик
        /// </summary>
        /// <param name="handler"></param>
        public void AddItemHandler(ItemAddedNotification<Tick> handler)
        {
            this.Handlers.Add(handler);
        }

    }
}
