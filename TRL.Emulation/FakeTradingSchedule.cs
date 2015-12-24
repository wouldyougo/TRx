using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Emulation
{
    public class AlwaysTimeToTradeSchedule:ITradingSchedule
    {
        public bool ItIsTimeToTrade(DateTime dateTime)
        {
            return true;
        }

        public DateTime SessionEnd
        {
            get
            {
                return DateTime.MaxValue;
            }
        }
    }

    public class NeverTimeToTradeSchedule : ITradingSchedule
    {
        public bool ItIsTimeToTrade(DateTime dateTime)
        {
            return false;
        }

        public DateTime SessionEnd
        {
            get
            {
                return DateTime.MinValue;
            }
        }
    }
}
