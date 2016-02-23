using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.TimeHelpers
{
    public class BrokerDateTime
    {
        ///TODO переделать BrokerDateTime
        public static DateTime Make(DateTime localDate)
        {
            //TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(ReadBrokerServerTimezoneIdFromAppConfig());
            //return TimeZoneInfo.ConvertTime(localDate, timeZoneInfo);
            return localDate;
        }

        private static string ReadBrokerServerTimezoneIdFromAppConfig()
        {
            return AppSettings.GetStringValue("BrokerServerTimezone");
        }

        public static DateTime TodayMidnight()
        {
            DateTime tomorrow = BrokerDateTime.Make(DateTime.Now).AddDays(1);

            return new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 0, 0);
        }

    }
}
