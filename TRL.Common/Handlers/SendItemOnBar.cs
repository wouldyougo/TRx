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
    public class SendItemOnBar : AddedItemHandler<Bar>
    {
        private IDataContext tradingData;
        private BarSettings barSettings;
        
        /// <summary>
        /// список сторонних обработчиков
        /// </summary>
        private IList<ItemAddedNotification<Bar>> Handlers;


        public SendItemOnBar(BarSettings barSettings, IDataContext tradingData)
            : base(tradingData.Get<ObservableCollection<Bar>>())
        {
            this.tradingData = tradingData;
            this.barSettings = barSettings;
            //подменяем тип бара на RangeBar
            if (this.barSettings.BarType != Enums.DataModelType.RangeBar)
                this.barSettings.BarType = Enums.DataModelType.RangeBar;
            this.Handlers = new List<ItemAddedNotification<Bar>>();
        }
        public override void OnItemAdded(Bar bar)
        {
            //кривая заглушка при получении минутных баров
            if (bar.Interval == 0) 
            {
                bar.Interval = barSettings.Interval;
            }

            foreach (var handler in Handlers)
            {
                handler.Invoke(bar);
            }
        }
        
        ///// <summary>
        ///// добавить сторонний обработчик
        ///// </summary>
        ///// <param name="handler"></param>
        //public void AddItemHandler(ItemAddedNotification<Bar> handler)
        //{
        //    //this.notifier.OnItemAdded += new ItemAddedNotification<Bar>(OnItemAdded);
        //    this.notifier.OnItemAdded += handler;
        //}

        /// <summary>
        /// добавить сторонний обработчик
        /// </summary>
        /// <param name="handler"></param>
        public void AddItemHandler(ItemAddedNotification<Bar> handler)
        {
            this.Handlers.Add(handler);
        }

    }
}
