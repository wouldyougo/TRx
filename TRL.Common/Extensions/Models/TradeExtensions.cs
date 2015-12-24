using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Extensions.Models
{
    public static class TradeExtensions
    {
        public static double GetTradeResult(this Trade trade, Trade close)
        {
            if (trade.DateTime > close.DateTime)
                return 0;

            if (trade.Portfolio != close.Portfolio)
                return 0;

            if (trade.Symbol != close.Symbol)
                return 0;

            double oSign = trade.Amount / Math.Abs(trade.Amount);
            double cSign = close.Amount / Math.Abs(close.Amount);

            if ( oSign == cSign )
                return 0;

            if (trade.Amount > 0 && close.Amount < 0)
            {
                return close.Price - trade.Price;
            }

            return trade.Price - close.Price;
        }
    }
}
