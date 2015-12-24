using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Connect.Smartcom.Events;
using TRL.Common.Collections;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Data
{
    public class SymbolsDataProvider
    {
        private ILogger logger;
        private SmartComHandlersDatabase handlers;
        private IDataContext symbolsData;

        public SymbolsDataProvider() : this(SmartComHandlers.Instance, SymbolsSummary.Instance, new NullLogger()) { }

        public SymbolsDataProvider(ILogger logger) : this(SmartComHandlers.Instance, SymbolsSummary.Instance, logger) { }

        public SymbolsDataProvider(SmartComHandlersDatabase handlers, IDataContext symbolsData, ILogger logger)
        {
            this.handlers = handlers;
            this.symbolsData = symbolsData;
            this.logger = logger;
            this.handlers.Add<_IStClient_AddSymbolEventHandler>(stServer_AddSymbol);
            this.handlers.Add<_IStClient_UpdateQuoteEventHandler>(stServer_UpdateQuote);
        }

        private void stServer_AddSymbol(int row, int nrows, string symbol, string short_name, string long_name, string type, int decimals, int lot_size, double punkt, double step, string sec_ext_id, string sec_exch_name, DateTime expiryDate, double days_before_expiry, double strike)
        {
            SymbolSettings item = new SymbolSettings(symbol, short_name, long_name, type, decimals, lot_size, punkt, step, expiryDate, days_before_expiry, strike);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, получен {2}",
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name, 
                item.ToString()));

            this.symbolsData.Get<HashSetOfNamedMutable<SymbolSettings>>().Add(item);
        }

        private void stServer_UpdateQuote(string symbol, DateTime datetime, double open, double high, double low, double close, double last, double volume, double size, double bid, double ask, double bidsize, double asksize, double open_int, double go_buy, double go_sell, double go_base, double go_base_backed, double high_limit, double low_limit, int trading_status , double volat, double theor_price)
        {
            SymbolSummary item =
                new SymbolSummary(symbol,
                    datetime,
                    open,
                    high,
                    low,
                    close,
                    last,
                    volume,
                    size,
                    ask,
                    bid,
                    asksize,
                    bidsize,
                    open_int,
                    go_buy,
                    go_sell,
                    high_limit,
                    low_limit,
                    (trading_status == 0) ? true : false);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, получен {2}",
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name, 
                item.ToString()));

            this.symbolsData.Get<HashSetOfNamedMutable<SymbolSummary>>().Add(item);
        }
    }
}
