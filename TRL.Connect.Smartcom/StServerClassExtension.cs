using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;
using System.Runtime.InteropServices;

namespace TRL.Connect.Smartcom
{
    public static class SmartComEventsExtensions
    {
        public static bool IsSmartComEvent<T>()
        {
            return SmartComEventsTypes.Collection.Contains(typeof(T));
        }

        public static void Bind<T>(this StServer stServer, T method)
            where T : class
        {
            if (!IsSmartComEvent<T>())
                return;

            if (method is _IStClient_AddBarEventHandler)
                stServer.AddBar += method as _IStClient_AddBarEventHandler;

            if (method is _IStClient_AddPortfolioEventHandler)
                stServer.AddPortfolio += method as _IStClient_AddPortfolioEventHandler;

            if (method is _IStClient_AddSymbolEventHandler)
                stServer.AddSymbol += method as _IStClient_AddSymbolEventHandler;

            if (method is _IStClient_AddTickEventHandler)
                stServer.AddTick += method as _IStClient_AddTickEventHandler;

            if (method is _IStClient_AddTickHistoryEventHandler)
                stServer.AddTickHistory += method as _IStClient_AddTickHistoryEventHandler;

            if (method is _IStClient_AddTradeEventHandler)
                stServer.AddTrade += method as _IStClient_AddTradeEventHandler;

            if (method is _IStClient_ConnectedEventHandler)
              stServer.Connected += method as _IStClient_ConnectedEventHandler;

            if (method is _IStClient_DisconnectedEventHandler)
                stServer.Disconnected += method as _IStClient_DisconnectedEventHandler;

            if (method is _IStClient_OrderCancelFailedEventHandler)
                stServer.OrderCancelFailed += method as _IStClient_OrderCancelFailedEventHandler;

            if (method is _IStClient_OrderCancelSucceededEventHandler)
                stServer.OrderCancelSucceeded += method as _IStClient_OrderCancelSucceededEventHandler;

            if (method is _IStClient_OrderFailedEventHandler)
                stServer.OrderFailed += method as _IStClient_OrderFailedEventHandler;

            if (method is _IStClient_OrderMoveFailedEventHandler)
                stServer.OrderMoveFailed += method as _IStClient_OrderMoveFailedEventHandler;

            if (method is _IStClient_OrderMoveSucceededEventHandler)
                stServer.OrderMoveSucceeded += method as _IStClient_OrderMoveSucceededEventHandler;

            if (method is _IStClient_OrderSucceededEventHandler)
                stServer.OrderSucceeded += method as _IStClient_OrderSucceededEventHandler;

            if (method is _IStClient_SetMyClosePosEventHandler)
                stServer.SetMyClosePos += method as _IStClient_SetMyClosePosEventHandler;

            if (method is _IStClient_SetMyOrderEventHandler)
                stServer.SetMyOrder += method as _IStClient_SetMyOrderEventHandler;

            if (method is _IStClient_SetMyTradeEventHandler)
                stServer.SetMyTrade += method as _IStClient_SetMyTradeEventHandler;

            if (method is _IStClient_SetPortfolioEventHandler)
                stServer.SetPortfolio += method as _IStClient_SetPortfolioEventHandler;

            if (method is _IStClient_SetSubscribtionCheckReultEventHandler)
                stServer.SetSubscribtionCheckReult += method as _IStClient_SetSubscribtionCheckReultEventHandler;

            if (method is _IStClient_UpdateBidAskEventHandler)
                stServer.UpdateBidAsk += method as _IStClient_UpdateBidAskEventHandler;

            if (method is _IStClient_UpdateOrderEventHandler)
                stServer.UpdateOrder += method as _IStClient_UpdateOrderEventHandler;

            if (method is _IStClient_UpdatePositionEventHandler)
                stServer.UpdatePosition += method as _IStClient_UpdatePositionEventHandler;

            if (method is _IStClient_UpdateQuoteEventHandler)
                stServer.UpdateQuote += method as _IStClient_UpdateQuoteEventHandler;

        }


        public static void Unbind<T>(this StServer stServer, T method)
            where T : class
        {
            if (!IsSmartComEvent<T>())
                return;

            if (method is _IStClient_AddBarEventHandler)
                stServer.AddBar -= method as _IStClient_AddBarEventHandler;

            if (method is _IStClient_AddPortfolioEventHandler)
                stServer.AddPortfolio -= method as _IStClient_AddPortfolioEventHandler;

            if (method is _IStClient_AddSymbolEventHandler)
                stServer.AddSymbol -= method as _IStClient_AddSymbolEventHandler;

            if (method is _IStClient_AddTickEventHandler)
                stServer.AddTick -= method as _IStClient_AddTickEventHandler;

            if (method is _IStClient_AddTickHistoryEventHandler)
                stServer.AddTickHistory -= method as _IStClient_AddTickHistoryEventHandler;

            if (method is _IStClient_AddTradeEventHandler)
                stServer.AddTrade -= method as _IStClient_AddTradeEventHandler;

            if (method is _IStClient_ConnectedEventHandler)
                stServer.Connected -= method as _IStClient_ConnectedEventHandler;

            if (method is _IStClient_DisconnectedEventHandler)
                stServer.Disconnected -= method as _IStClient_DisconnectedEventHandler;

            if (method is _IStClient_OrderCancelFailedEventHandler)
                stServer.OrderCancelFailed -= method as _IStClient_OrderCancelFailedEventHandler;

            if (method is _IStClient_OrderCancelSucceededEventHandler)
                stServer.OrderCancelSucceeded -= method as _IStClient_OrderCancelSucceededEventHandler;

            if (method is _IStClient_OrderFailedEventHandler)
                stServer.OrderFailed -= method as _IStClient_OrderFailedEventHandler;

            if (method is _IStClient_OrderMoveFailedEventHandler)
                stServer.OrderMoveFailed -= method as _IStClient_OrderMoveFailedEventHandler;

            if (method is _IStClient_OrderMoveSucceededEventHandler)
                stServer.OrderMoveSucceeded -= method as _IStClient_OrderMoveSucceededEventHandler;

            if (method is _IStClient_OrderSucceededEventHandler)
                stServer.OrderSucceeded -= method as _IStClient_OrderSucceededEventHandler;

            if (method is _IStClient_SetMyClosePosEventHandler)
                stServer.SetMyClosePos -= method as _IStClient_SetMyClosePosEventHandler;

            if (method is _IStClient_SetMyOrderEventHandler)
                stServer.SetMyOrder -= method as _IStClient_SetMyOrderEventHandler;

            if (method is _IStClient_SetMyTradeEventHandler)
                stServer.SetMyTrade -= method as _IStClient_SetMyTradeEventHandler;

            if (method is _IStClient_SetPortfolioEventHandler)
                stServer.SetPortfolio -= method as _IStClient_SetPortfolioEventHandler;

            if (method is _IStClient_SetSubscribtionCheckReultEventHandler)
                stServer.SetSubscribtionCheckReult -= method as _IStClient_SetSubscribtionCheckReultEventHandler;

            if (method is _IStClient_UpdateBidAskEventHandler)
                stServer.UpdateBidAsk -= method as _IStClient_UpdateBidAskEventHandler;

            if (method is _IStClient_UpdateOrderEventHandler)
                stServer.UpdateOrder -= method as _IStClient_UpdateOrderEventHandler;

            if (method is _IStClient_UpdatePositionEventHandler)
                stServer.UpdatePosition -= method as _IStClient_UpdatePositionEventHandler;

            if (method is _IStClient_UpdateQuoteEventHandler)
                stServer.UpdateQuote -= method as _IStClient_UpdateQuoteEventHandler;
        }

        public static void Reset(this StServer stServer)
        {
            if (stServer == null)
                return;

            if(stServer is StServerClass)
                Marshal.ReleaseComObject(stServer);

            stServer = null;
        }
    }
}
