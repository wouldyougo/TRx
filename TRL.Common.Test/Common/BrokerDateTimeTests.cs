using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Test
{
    [TestClass]
    public class BrokerDateTimeTests
    {
        [TestMethod]
        public void Common_GetServerTimeZoneInfo()
        {
            string timeZoneId = AppSettings.GetStringValue("BrokerServerTimezone");

            TimeZoneInfo brokerServerTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            DateTime localTime = new DateTime(2012, 11, 30, 12, 00, 00);

            Assert.AreEqual(new DateTime(2012, 11, 30, 12, 00, 00), TimeZoneInfo.ConvertTime(localTime, brokerServerTimeZone));
        }

        [TestMethod]
        public void Common_Local_To_Server_Time_Converter()
        {
            DateTime localTime = new DateTime(2012, 11, 30, 12, 00, 00);

            Assert.AreEqual(new DateTime(2012, 11, 30, 12, 00, 00), BrokerDateTime.Make(localTime));
        }

        [TestMethod]
        public void Common_BrokerDateTime_test()
        {
            DateTime date = new DateTime(2013, 9, 9, 10, 0, 0, DateTimeKind.Utc);

            string tzId = "Russian Standard Time";

            TimeZoneInfo brokerTzInfo = TimeZoneInfo.FindSystemTimeZoneById(tzId);

            TimeZoneInfo localTzInfo = TimeZoneInfo.Local;

            DateTime localDate = TimeZoneInfo.ConvertTime(date, localTzInfo);

            DateTime brokerDate = TimeZoneInfo.ConvertTime(localDate, brokerTzInfo);

            Assert.AreEqual(brokerDate, BrokerDateTime.Make(localDate));
        }
    }
}
