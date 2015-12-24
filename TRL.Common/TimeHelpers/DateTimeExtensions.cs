using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;

namespace TRL.Common.TimeHelpers
{
    public static class DateTimeExtensions
    {
        public static bool Within(this DateTime dateTime, TimePeriod timePeriod)
        {
            return dateTime > timePeriod.Begin && dateTime < timePeriod.End;
        }

        public static bool ItIsWorkDay(this DateTime dateTime)
        {
            return dateTime.DayOfWeek != DayOfWeek.Sunday && dateTime.DayOfWeek != DayOfWeek.Saturday;
        }

        public static DateTime RoundDownToNearestMinutes(this DateTime date, int minutes)
        {
            int factor = date.Minute % minutes;

            int seconds = factor * 60 + date.Second;

            return date.AddSeconds(-seconds).AddMilliseconds(-date.Millisecond);
        }
        public static DateTime RoundDownToNearestMinutes2(this DateTime date, double minutes)
        {
            double factor = (date.Minute + (double)date.Second / 60) % minutes;

            double seconds = factor * 60;

            return date.AddSeconds(-seconds).AddMilliseconds(-date.Millisecond);
        }
    }
}
