using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class TimePeriod
    {
        private DateTime begin;
        private DateTime end;
        private TimeSpan timeSpan;

        private TimePeriod() { }

        public TimePeriod(DateTime begin, DateTime end)
        {
            if (begin > end)
                throw new ArgumentException("Дата начала периода не может иметь более позднее значение, чем дата окончания периода!", "begin");

            this.begin = begin;
            this.end = end;
            this.timeSpan = this.end - this.begin;
        }

        public DateTime Begin { get { return this.begin; } }
        public DateTime End { get { return this.end; } }
        public TimeSpan TimeSpan { get { return this.timeSpan; } }
    }
}
