using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Models;
using TRL.Common.Data;
using System.Globalization;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Data
{
    public class RawTradingDataProvider
    {
        private ILogger logger;
        private SmartComHandlersDatabase handlers;
        private BaseDataContext orderData;

        public RawTradingDataProvider() : this(SmartComHandlers.Instance, RawTradingData.Instance, new NullLogger()) { }

        public RawTradingDataProvider(ILogger logger) : this(SmartComHandlers.Instance, RawTradingData.Instance, logger) { }

        public RawTradingDataProvider(SmartComHandlersDatabase handlers, BaseDataContext orderData, ILogger logger)
        {
            this.handlers = handlers;
            this.orderData = orderData;
            this.logger = logger;
            this.handlers.Add<_IStClient_OrderFailedEventHandler>(stServer_OrderFailed);
            this.handlers.Add<_IStClient_OrderSucceededEventHandler>(stServer_OrderSucceeded);
            this.handlers.Add<_IStClient_UpdateOrderEventHandler>(stServer_UpdateOrder);
            this.handlers.Add <_IStClient_SetPortfolioEventHandler>(stServer_SetPortfolio);
            this.handlers.Add<_IStClient_AddTradeEventHandler>(stServer_AddTrade);
            this.handlers.Add<_IStClient_AddSymbolEventHandler>(stServer_AddSymbol);
        }

        private void stServer_OrderFailed(int cookie, string orderId, string reason)
        {

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, получен OrderFailed {2}, {3}, {4}",
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name, 
                cookie, 
                orderId, 
                reason));

            this.orderData.GetData<OrderFailed>().Add(new OrderFailed(cookie, orderId, reason));
        }

        private void stServer_OrderSucceeded(int cookie, string orderId)
        {

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, получен OrderSucceeded {2}, {3}",
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name, 
                cookie, 
                orderId));

            this.orderData.GetData<OrderSucceeded>().Add(new OrderSucceeded(cookie, orderId));
        }

        private void stServer_UpdateOrder(string portfolio, string symbol, SmartCOM3Lib.StOrder_State state,
            SmartCOM3Lib.StOrder_Action action, SmartCOM3Lib.StOrder_Type type, SmartCOM3Lib.StOrder_Validity validity,
            double price, double amount, double stop, double filled, System.DateTime datetime, string orderid, string orderno,
            int status_mask, int Cookie)
        {
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, получен UpdateOrder {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12:dd/MM/yyyy H:mm:ss.fff}, {13}, {14}, {15}, {16}", 
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name,
                portfolio, 
                symbol, 
                state, 
                action, 
                type, 
                validity, 
                price, 
                amount, 
                stop, 
                filled, 
                datetime,
                orderid, 
                orderno, 
                status_mask, 
                Cookie));

            this.orderData.GetData<UpdateOrder>().Add(new UpdateOrder(portfolio,
                symbol,
                state,
                action,
                type,
                validity,
                price,
                amount,
                stop,
                filled,
                datetime,
                orderid,
                orderno,
                status_mask,
                Cookie));
        }

        private void stServer_SetPortfolio(string portfolio, double cash, double leverage, double commision, double saldo)
        {
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, получен SetPortfolio {2}, {3}, {4}, {5}, {6}",
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name, 
                portfolio, 
                cash, 
                leverage, 
                commision, 
                saldo));

            this.orderData.GetData<SetPortfolio>().Add(new SetPortfolio(portfolio, cash, leverage, commision, saldo));
        }

        private void stServer_AddTrade(string portfolio, string symbol, string orderno, double price, double amount, DateTime date, string tradeno)
        {
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, получен Trade {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name, 
                portfolio, 
                symbol, 
                orderno, 
                price, 
                amount, 
                date, 
                tradeno));

            this.orderData.GetData<TradeInfo>().Add(new TradeInfo(portfolio, symbol, orderno, price, amount, date, tradeno));
        }

        private void stServer_AddSymbol(int row, int nrows, string symbol, string short_name, string long_name, string type, int decimals, int lot_size, double punkt, double step, string sec_ext_id, string sec_exch_name, DateTime expiryDate, double days_before_expiry, double strike)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, получен Symbol {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}",
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name, 
                row, 
                nrows, 
                symbol, 
                short_name, 
                long_name, 
                type, 
                decimals, 
                lot_size, 
                punkt.ToString("0.0000", ci), 
                step.ToString("0.0000", ci), 
                sec_ext_id, 
                sec_exch_name, 
                expiryDate.ToString(ci), 
                days_before_expiry.ToString("0.0000", ci), 
                strike.ToString("0.0000", ci)));

            this.orderData.GetData<RawSymbol>().Add(new RawSymbol(symbol, short_name, long_name, type, decimals, lot_size, punkt, step, sec_ext_id, sec_exch_name, expiryDate, days_before_expiry, strike));
        }

    }
}
