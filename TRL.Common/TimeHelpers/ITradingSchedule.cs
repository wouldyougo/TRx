using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.TimeHelpers
{
    public interface ITradingSchedule
    {
        bool ItIsTimeToTrade(DateTime dateTime);
        DateTime SessionEnd { get; }
    }
}
