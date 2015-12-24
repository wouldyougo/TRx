using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.TimeHelpers
{
    public class TimeTracker:ITimeTrackable
    {
        private DateTime start;

        public TimeTracker()
        {
            this.start = BrokerDateTime.Make(DateTime.Now);
        }
        
        public DateTime StartAt
        {
            get
            {
                return this.start;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return BrokerDateTime.Make(DateTime.Now) - this.start;
            }
        }

    }
}
