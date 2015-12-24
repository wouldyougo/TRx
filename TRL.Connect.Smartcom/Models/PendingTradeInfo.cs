using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Models
{
    public class PendingTradeInfo:TradeInfo
    {
        private PendingTradeInfo() { }

        public PendingTradeInfo(TradeInfo item)
            :base(item.Portfolio, 
            item.Symbol, 
            item.OrderNo, 
            item.Price, 
            item.Amount, 
            item.DateTime, 
            item.TradeNo)
        { }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 
                this.Portfolio,
                this.Symbol,
                this.OrderNo,
                this.Price,
                this.Amount,
                this.DateTime,
                this.TradeNo);
        }
    }
}
