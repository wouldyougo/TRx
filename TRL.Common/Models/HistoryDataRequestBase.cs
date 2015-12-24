using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public abstract class HistoryDataRequestBase : IHistoryDataRequest
    {
        private string symbol;
        public string Symbol
        {
            get { return this.symbol; }
        }

        private int interval;
        public int Interval
        {
            get { return this.interval; }
        }

        private int quantity;
        public int Quantity
        {
            get { return this.quantity; }
        }

        private DateTime fromDate;
        public DateTime FromDate
        {
            get { return this.fromDate; }
        }

        public HistoryDataRequestBase(string symbol,
            int interval,
            int quantity,
            DateTime fromDate)
        {
            this.symbol = symbol;
            this.quantity = quantity;
            this.interval = interval;
            this.fromDate = fromDate;
        }
    }
}
