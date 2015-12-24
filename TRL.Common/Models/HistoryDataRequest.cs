using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public interface IHistoryDataRequest
    {
        string Symbol { get; }
        int Interval { get; }
        int Quantity { get; }
        DateTime FromDate { get; }
    }
}
