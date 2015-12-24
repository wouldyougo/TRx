using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SmartCOM3Lib;

namespace TRL.Connect.Smartcom
{
    public class SmartComEventsTypes
    {
        private static List<Type> theCollection = new List<Type>();

        public static IEnumerable<Type> Collection
        {
            get
            {
                if (theCollection.Count == 0)
                    Initialize();

                return theCollection;
            }
        }

        private static void Initialize()
        {
            theCollection.Add(typeof(_IStClient_ConnectedEventHandler));
            theCollection.Add(typeof(_IStClient_AddBarEventHandler));
            theCollection.Add(typeof(_IStClient_AddPortfolioEventHandler));
            theCollection.Add(typeof(_IStClient_AddSymbolEventHandler));
            theCollection.Add(typeof(_IStClient_AddTickEventHandler));
            theCollection.Add(typeof(_IStClient_AddTickHistoryEventHandler));
            theCollection.Add(typeof(_IStClient_AddTradeEventHandler));
            theCollection.Add(typeof(_IStClient_DisconnectedEventHandler));
            theCollection.Add(typeof(_IStClient_OrderCancelFailedEventHandler));
            theCollection.Add(typeof(_IStClient_OrderCancelSucceededEventHandler));
            theCollection.Add(typeof(_IStClient_OrderFailedEventHandler));
            theCollection.Add(typeof(_IStClient_OrderMoveFailedEventHandler));
            theCollection.Add(typeof(_IStClient_OrderMoveSucceededEventHandler));
            theCollection.Add(typeof(_IStClient_OrderSucceededEventHandler));
            theCollection.Add(typeof(_IStClient_SetMyClosePosEventHandler));
            theCollection.Add(typeof(_IStClient_SetMyOrderEventHandler));
            theCollection.Add(typeof(_IStClient_SetMyTradeEventHandler));
            theCollection.Add(typeof(_IStClient_SetPortfolioEventHandler));
            theCollection.Add(typeof(_IStClient_SetSubscribtionCheckReultEventHandler));
            theCollection.Add(typeof(_IStClient_UpdateBidAskEventHandler));
            theCollection.Add(typeof(_IStClient_UpdateOrderEventHandler));
            theCollection.Add(typeof(_IStClient_UpdatePositionEventHandler));
            theCollection.Add(typeof(_IStClient_UpdateQuoteEventHandler));
        }
    }
}
