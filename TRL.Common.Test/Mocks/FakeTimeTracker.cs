using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.TimeHelpers;
using TRL.Logging;

namespace TRL.Common.Test
{
    public class FakeTimeTracker:ITimeTrackable
    {
        private DateTime start;
        private DateTime stop;

        private FakeTimeTracker() { }

        public FakeTimeTracker(DateTime start, DateTime stop)
        {
            this.start = start;
            this.stop = stop;
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
                return this.stop - this.start;
            }
        }

        public void IncrementStopDate(int seconds)
        {
            this.stop = this.stop.AddSeconds(seconds);
        }

    }
}
