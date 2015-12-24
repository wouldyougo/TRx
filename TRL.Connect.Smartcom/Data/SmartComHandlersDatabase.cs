using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using SmartCOM3Lib;
using System.Reflection;

namespace TRL.Connect.Smartcom.Data
{
    public class SmartComHandlersDatabase:IDatabase, IStrictDataContext
    {
        public HashSet<_IStClient_AddBarEventHandler> AddBarHandlers { get; private set; }
        public HashSet<_IStClient_AddPortfolioEventHandler> AddPortfolioHandlers { get; private set; }
        public HashSet<_IStClient_AddSymbolEventHandler> AddSymbolHandlers { get; private set; }
        public HashSet<_IStClient_AddTickEventHandler> AddTickHandlers { get; private set; }
        public HashSet<_IStClient_AddTickHistoryEventHandler> AddTickHistoryHandlers { get; private set; }
        public HashSet<_IStClient_AddTradeEventHandler> AddTradeHandlers { get; private set; }
        public HashSet<_IStClient_ConnectedEventHandler> ConnectedHandlers { get; private set; }
        public HashSet<_IStClient_DisconnectedEventHandler> DisconnectedHandlers { get; private set; }
        public HashSet<_IStClient_OrderCancelFailedEventHandler> OrderCancelFailedHandlers { get; private set; }
        public HashSet<_IStClient_OrderCancelSucceededEventHandler> OrderCancelSucceededHandlers { get; private set; }
        public HashSet<_IStClient_OrderFailedEventHandler> OrderFailedHandlers { get; private set; }
        public HashSet<_IStClient_OrderMoveFailedEventHandler> OrderMoveFailedHandlers { get; private set; }
        public HashSet<_IStClient_OrderMoveSucceededEventHandler> OrderMoveSucceededHandlers { get; private set; }
        public HashSet<_IStClient_OrderSucceededEventHandler> OrderSucceededHandlers { get; private set; }
        public HashSet<_IStClient_SetMyClosePosEventHandler> SetMyClosePosHandlers { get; private set; }
        public HashSet<_IStClient_SetMyOrderEventHandler> SetMyOrderHandlers { get; private set; }
        public HashSet<_IStClient_SetMyTradeEventHandler> SetMyTradeHandlers { get; private set; }
        public HashSet<_IStClient_SetPortfolioEventHandler> SetPortfolioHandlers { get; private set; }
        public HashSet<_IStClient_SetSubscribtionCheckReultEventHandler> SetSubscriptionHandlers { get; private set; }
        public HashSet<_IStClient_UpdateBidAskEventHandler> UpdateBidAskHandlers { get; private set; }
        public HashSet<_IStClient_UpdateOrderEventHandler> UpdateOrderHandlers { get; private set; }
        public HashSet<_IStClient_UpdatePositionEventHandler> UpdatePositionHandlers { get; private set; }
        public HashSet<_IStClient_UpdateQuoteEventHandler> UpdateQuoteHandlers { get; private set; }


        public SmartComHandlersDatabase()
        {
            this.AddBarHandlers = new HashSet<_IStClient_AddBarEventHandler>();
            this.AddPortfolioHandlers = new HashSet<_IStClient_AddPortfolioEventHandler>();
            this.AddSymbolHandlers = new HashSet<_IStClient_AddSymbolEventHandler>();
            this.AddTickHandlers = new HashSet<_IStClient_AddTickEventHandler>();
            this.AddTickHistoryHandlers = new HashSet<_IStClient_AddTickHistoryEventHandler>();
            this.AddTradeHandlers = new HashSet<_IStClient_AddTradeEventHandler>();
            this.ConnectedHandlers = new HashSet<_IStClient_ConnectedEventHandler>();
            this.DisconnectedHandlers = new HashSet<_IStClient_DisconnectedEventHandler>();
            this.OrderCancelFailedHandlers = new HashSet<_IStClient_OrderCancelFailedEventHandler>();
            this.OrderCancelSucceededHandlers = new HashSet<_IStClient_OrderCancelSucceededEventHandler>();
            this.OrderFailedHandlers = new HashSet<_IStClient_OrderFailedEventHandler>();
            this.OrderMoveFailedHandlers = new HashSet<_IStClient_OrderMoveFailedEventHandler>();
            this.OrderMoveSucceededHandlers = new HashSet<_IStClient_OrderMoveSucceededEventHandler>();
            this.OrderSucceededHandlers = new HashSet<_IStClient_OrderSucceededEventHandler>();
            this.SetMyClosePosHandlers = new HashSet<_IStClient_SetMyClosePosEventHandler>();
            this.SetMyOrderHandlers = new HashSet<_IStClient_SetMyOrderEventHandler>();
            this.SetMyTradeHandlers = new HashSet<_IStClient_SetMyTradeEventHandler>();
            this.SetPortfolioHandlers = new HashSet<_IStClient_SetPortfolioEventHandler>();
            this.SetSubscriptionHandlers = new HashSet<_IStClient_SetSubscribtionCheckReultEventHandler>();
            this.UpdateBidAskHandlers = new HashSet<_IStClient_UpdateBidAskEventHandler>();
            this.UpdateOrderHandlers = new HashSet<_IStClient_UpdateOrderEventHandler>();
            this.UpdatePositionHandlers = new HashSet<_IStClient_UpdatePositionEventHandler>();
            this.UpdateQuoteHandlers = new HashSet<_IStClient_UpdateQuoteEventHandler>();

        }

        public HashSet<T> GetData<T>() where T : class
        {
            PropertyInfo[] properties = this.GetType().GetProperties();

            foreach (PropertyInfo item in properties)
            {
                if (item.PropertyType.FullName.Equals(typeof(HashSet<T>).FullName))
                    return (HashSet<T>)item.GetValue(this, null);
            }

            return null;
        }

        public void Add<T>(T method)
            where T : class
        {
            if (!SmartComEventsTypes.Collection.Contains(typeof(T)))
                return;

            if (!this.GetData<T>().Contains(method))
            {
                this.GetData<T>().Add(method);
                this.handlerCounter++;
            }
        }

        public void Remove<T>(T method)
            where T : class
        {
            if (!SmartComEventsTypes.Collection.Contains(typeof(T)))
                return;

            if (this.GetData<T>().Contains(method))
            {
                this.GetData<T>().Remove(method);

                this.handlerCounter--;
            }
        }

        private int handlerCounter;
        public int HandlerCounter
        {
            get
            {
                return this.handlerCounter;
            }
        }

    }
}
