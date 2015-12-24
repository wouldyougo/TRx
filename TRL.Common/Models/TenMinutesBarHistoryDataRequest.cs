using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class TenMinutesBarHistoryDataRequest : HistoryDataRequestBase
    {
        public TenMinutesBarHistoryDataRequest(string symbol,
            int quantity,
            DateTime fromDate)
            :base(symbol,
            600,
            quantity,
            fromDate)
        {
        }
    }
}
