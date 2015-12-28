using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;
using TRL.Common.Data;
using TRL.Common.Models;

namespace TRL.Connect.Smartcom.Test.Mocks
{
    public delegate void MockEvent();

    public class StServerClassMock:StServer
    {
        private bool getSymbolsExecuted;
        public bool GetSymbolsExecuted
        {
            get
            {
                return this.getSymbolsExecuted;
            }
        }

        public void ReadTicksFrom(string file)
        {
            this.AddTick("RTS-6.16_FT", DateTime.Now, 150000, 345, "237842", StOrder_Action.StOrder_Action_Buy);
        }

        public void EmulateSymbolArrival(int row, int nrows, string symbol, string short_name, string long_name, string type, int decimals, int lot_size, double punkt, double step, string sec_ext_id, string sec_exch_name, DateTime expiry_date, double days_before_expiry, double strike)
        {
            this.AddSymbol(row, nrows, symbol, short_name, long_name, type, decimals, lot_size, punkt, step, sec_ext_id, sec_exch_name, expiry_date, days_before_expiry, strike);
        }

        public void EmulateTickArrival(string symbol, DateTime date, double price, double volume, StOrder_Action action)
        {
            this.AddTick(symbol, date, price, volume, "8888888", action);
        }

        public void EmulateBidAskArrival(string symbol, int row, int nrows, double bid, double bidsize, double ask, double asksize)
        {
            this.UpdateBidAsk(symbol, row, nrows, bid, bidsize, ask, asksize);
        }

        public void EmulateBarArrival(string symbol, StBarInterval interval, DateTime datetime, double open, double high, double low, double close, double volume, double open_int)
        {
            this.AddBar(0, 1, symbol, interval, datetime, open, high, low, close, volume, open_int);
        }

        public void EmulateOrderFailedArrival(int cookie, string orderId, string reason)
        {
            this.OrderFailed(cookie, orderId, reason);
        }

        public void EmulateOrderSucceededArrival(int cookie, string orderId)
        {
            this.OrderSucceeded(cookie, orderId);
        }

        public void EmulateTradeArrival(string portfolio, string symbol, string orderno, double price, double amount, DateTime date, string tradeno)
        {
            this.AddTrade(portfolio, symbol, orderno, price, amount, date, tradeno);
        }

        public void EmulateUpdateOrderArrival(string portfolio, string symbol, SmartCOM3Lib.StOrder_State state,
            SmartCOM3Lib.StOrder_Action action, SmartCOM3Lib.StOrder_Type type, SmartCOM3Lib.StOrder_Validity validity,
            double price, double amount, double stop, double filled, System.DateTime datetime, string orderid, string orderno,
            int status_mask, int cookie)
        {
            this.UpdateOrder(portfolio, symbol, state, action, type, validity, price, amount, stop, filled, datetime, orderid, orderno, status_mask, cookie);
        }

        public void EmulateSetPortfolioArrival(string portfolio, double cash, double leverage, double commision, double saldo)
        {
            this.SetPortfolio(portfolio, cash, leverage, commision, saldo);
        }

        public void EmulateSymbolSettingsArrival(int row, int nrows, string symbol, string short_name, string long_name, string type, int decimals, int lot_size, double punkt, double step, string sec_ext_id, string sec_exch_name, DateTime expiryDate, double days_before_expiry, double strike)
        {
            this.AddSymbol(row, nrows, symbol, short_name, long_name, type, decimals, lot_size, punkt, step, sec_ext_id, sec_exch_name, expiryDate, days_before_expiry, strike);
        }

        public void EmulateUpdateQuoteArrival(string symbol, DateTime datetime, double open, double high, double low, double close, double last, double volume, double size, double bid, double ask, double bidsize, double asksize, double open_int, double go_buy, double go_sell, double go_base, double go_base_backed, double high_limit, double low_limit, int trading_status, double volat, double theor_price)
        {
            this.UpdateQuote(symbol, datetime, open, high, low, close, last, volume, size, bid, ask, bidsize, asksize, open_int, go_buy, go_sell, go_base, go_base_backed, high_limit, low_limit, trading_status , volat, theor_price);
        }

        private bool isConnected;

        public StServerClassMock()
        {
        }

        public void EmulateDisconnect()
        {
            this.isConnected = false;

            if(this.Disconnected != null)
                this.Disconnected("connection lost");
        }

        public void CancelAllOrders()
        {
        }

        public void CancelBidAsks(string symbol)
        {
        }

        public void CancelOrder(string portfolio, string symbol, string orderid)
        {
            this.ordersCanceled++;
        }

        public void CancelPortfolio(string portfolio)
        {
        }

        public void CancelQuotes(string symbol)
        {
        }

        public void CancelTicks(string symbol)
        {
        }

        public void CheckSubscribtion(string subscribtionId)
        {
        }

        public void ConfigureClient(string paramsSet)
        {
        }

        public void ConfigureServer(string paramsSet)
        {
        }

        public void GetBars(string symbol, StBarInterval interval, DateTime since, int count)
        {
            this.barsReceived = count;
        }

        public void GetMyClosePos(string portfolio)
        {
        }

        public void GetMyOrders(int onlyActive, string portfolio)
        {

        }

        public void GetMyTrades(string portfolio)
        {

        }

        public void GetPrortfolioList()
        {

        }

        public void GetSymbols()
        {
            this.getSymbolsExecuted = true;
        }

        public void GetTrades(string symbol, DateTime from, int count)
        {

        }

        public bool IsConnected()
        {
            return this.isConnected;
        }

        public void ListenBidAsks(string symbol)
        {
            //this.UpdateBidAsk("RTS-12.12_FT", 0, 10, 1, 1, 1, 1);
        }

        public void ListenPortfolio(string portfolio)
        {
            //this.AddTrade("PRTFL", "RTS-12.12_FT", "10", 141000, 10, DateTime.Now, "11");
            //this.AddBar(0, 1, "RTS-12.12_FT", StBarInterval.StBarInterval_1Min, DateTime.Now, 140000, 141000, 139000, 139500, 1000, 0);
        }

        public void ListenQuotes(string symbol)
        {
            /*this.UpdateQuote("RTS-12.12_FT", DateTime.Now, 1, 1,
            1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1,
            1, 1, 1, 1, 1,
            1, 1, 1);*/
        }

        public void ListenTicks(string symbol)
        {
            //this.AddTick("RTS-12.12_FT", DateTime.Now, 1, 1, "number", StOrder_Action.StOrder_Action_Buy);
        }

        public void MoveOrder(string portfolio, string orderid, double targetPrice)
        {

        }

        private int barsReceived;
        public int BarsReceived
        {
            get
            {
                return this.barsReceived;
            }
        }

        private int ordersPlaced;
        public int OrdersPlaced
        {
            get
            {
                return this.ordersPlaced;
            }
        }

        private int ordersCanceled;
        public int OrdersCanceled
        {
            get
            {
                return this.ordersCanceled;
            }
        }

        public void PlaceOrder(string portfolio, string symbol, StOrder_Action action, StOrder_Type type, StOrder_Validity validity, double price, double amount, double stop, int cookie)
        {
            this.ordersPlaced++;
        }

        public void connect(string ip, ushort port, string login, string password)
        {
            if (this.isConnectionProhibited)
                return;

            this.isConnected = true;

            if(this.Connected != null)
                this.Connected();
        }

        public void disconnect()
        {
            this.isConnected = false;

            if(this.Disconnected != null)
                this.Disconnected("disconnected by user");
        }

        private bool isConnectionProhibited;

        public void ProhibitConnection()
        {
            this.isConnectionProhibited = true;
        }

        public event _IStClient_AddBarEventHandler AddBar;

        public event _IStClient_AddPortfolioEventHandler AddPortfolio;

        public event _IStClient_AddSymbolEventHandler AddSymbol;

        public event _IStClient_AddTickEventHandler AddTick;

        public event _IStClient_AddTickHistoryEventHandler AddTickHistory;

        public event _IStClient_AddTradeEventHandler AddTrade;

        public event _IStClient_ConnectedEventHandler Connected;

        public event _IStClient_DisconnectedEventHandler Disconnected;

        public event _IStClient_OrderCancelFailedEventHandler OrderCancelFailed;

        public event _IStClient_OrderCancelSucceededEventHandler OrderCancelSucceeded;

        public event _IStClient_OrderFailedEventHandler OrderFailed;

        public event _IStClient_OrderMoveFailedEventHandler OrderMoveFailed;

        public event _IStClient_OrderMoveSucceededEventHandler OrderMoveSucceeded;

        public event _IStClient_OrderSucceededEventHandler OrderSucceeded;

        public event _IStClient_SetMyClosePosEventHandler SetMyClosePos;

        public event _IStClient_SetMyOrderEventHandler SetMyOrder;

        public event _IStClient_SetMyTradeEventHandler SetMyTrade;

        public event _IStClient_SetPortfolioEventHandler SetPortfolio;

        public event _IStClient_SetSubscribtionCheckReultEventHandler SetSubscribtionCheckReult;

        public event _IStClient_UpdateBidAskEventHandler UpdateBidAsk;

        public event _IStClient_UpdateOrderEventHandler UpdateOrder;

        public event _IStClient_UpdatePositionEventHandler UpdatePosition;

        public event _IStClient_UpdateQuoteEventHandler UpdateQuote;


        public string GetMoneyAccount(string portfolioID)
        {
            throw new NotImplementedException();
        }
    }
}
