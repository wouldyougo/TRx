using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using TRL.Common.Extensions;
//using TRL.Common.Extensions.Data;

namespace TRL.Common.TimeHelpers
{
    public class FortsTradingSchedule:ITradingSchedule
    {
        public bool ItIsTimeToTrade(DateTime dateTime)
        {
            return dateTime.ItIsWorkDay() && ItsWorkHours(dateTime);
        }

        private bool ItsWorkHours(DateTime dateTime)
        {
            if (dateTime.Hour < 10)
                return false;

            if (dateTime.Hour == 18 && dateTime.Minute >= 45)
                return false;

            if (dateTime.Hour == 14 && dateTime.Minute < 3)
                return false;

            if (dateTime.Hour == 23 && dateTime.Minute >= 50)
                return false;

            return true;
        }

        //private static readonly int SessionEndHour = 19;
        private static readonly int SessionEndHour = 23;

        public DateTime SessionEnd
        {
            get
            {
                DateTime today = DateTime.Now;

                if (today.Hour >= SessionEndHour)
                    today = today.AddDays(1);

                return new DateTime(today.Year, today.Month, today.Day, SessionEndHour, 0, 0);
            }
        }
    }
}
