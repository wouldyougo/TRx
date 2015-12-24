using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class TickHistoryDataRequest:HistoryDataRequestBase
    {
        public TickHistoryDataRequest(string symbol,
            int quantity,
            DateTime fromDate)
            : base(symbol,
            0,
            quantity,
            fromDate) { }
    }
}
