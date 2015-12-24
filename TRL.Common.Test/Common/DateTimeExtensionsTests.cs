using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common;
//using TRL.Common.Extensions;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Test
{
    [TestClass]
    public class DateTimeExtensionsTests
    {
        [TestMethod]
        public void DateTimeExtensions_DateTime_Within_TimePeriod()
        {
            TimePeriod timePeriod = new TimePeriod(new DateTime(2013, 1, 1, 10, 0, 0), new DateTime(2013, 1, 1, 11, 0, 0));

            DateTime dateTime = new DateTime(2013, 1, 1, 10, 30, 0);

            Assert.IsTrue(dateTime.Within(timePeriod));
            
        }

        [TestMethod]
        public void DateTimeExtensions_Early_DateTime_Is_Not_Within_TimePeriod()
        {
            TimePeriod timePeriod = new TimePeriod(new DateTime(2013, 1, 1, 10, 0, 0), new DateTime(2013, 1, 1, 11, 0, 0));

            DateTime dateTime = new DateTime(2013, 1, 1, 9, 59, 59);

            Assert.IsFalse(dateTime.Within(timePeriod));
        }

        [TestMethod]
        public void DateTimeExtensions_Later_DateTime_Is_Not_Within_TimePeriod()
        {
            TimePeriod timePeriod = new TimePeriod(new DateTime(2013, 1, 1, 10, 0, 0), new DateTime(2013, 1, 1, 11, 0, 0));

            DateTime dateTime = new DateTime(2013, 1, 1, 11, 0, 1);

            Assert.IsFalse(dateTime.Within(timePeriod));
        }

        [TestMethod]
        public void DateTimeExtensions_ItsWorkDay()
        {
            DateTime dateTime = new DateTime(2013, 2, 16, 0, 0, 0);

            Assert.IsFalse(dateTime.ItIsWorkDay());
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_1_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 6, 33);

            DateTime round = date.RoundDownToNearestMinutes(1);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(6, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_2_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 1, 33);

            DateTime round = date.RoundDownToNearestMinutes(1);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(1, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_Five_1_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 6, 33);

            DateTime round = date.RoundDownToNearestMinutes(5);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(5, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_Five_2_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 5, 33);

            DateTime round = date.RoundDownToNearestMinutes(5);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(5, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_Five_3_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 3, 33);

            DateTime round = date.RoundDownToNearestMinutes(5);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(0, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_Ten_1_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 3, 33);

            DateTime round = date.RoundDownToNearestMinutes(10);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(0, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_Ten_2_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 59, 33);

            DateTime round = date.RoundDownToNearestMinutes(10);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(50, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_Fifteen_1_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 59, 33);

            DateTime round = date.RoundDownToNearestMinutes(15);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(45, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_Fifteen_2_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 14, 33);

            DateTime round = date.RoundDownToNearestMinutes(15);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(0, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_Thirty_1_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 14, 33);

            DateTime round = date.RoundDownToNearestMinutes(30);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(0, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_Thirty_2_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 31, 33);

            DateTime round = date.RoundDownToNearestMinutes(30);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(30, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_Thirty_3_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 30, 33);

            DateTime round = date.RoundDownToNearestMinutes(30);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(30, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_Sixty_1_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 30, 33);

            DateTime round = date.RoundDownToNearestMinutes(60);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(0, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes_Sixty_2_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 1, 33);

            DateTime round = date.RoundDownToNearestMinutes(60);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(0, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes2_Sixty_1_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 30, 33);

            DateTime round = date.RoundDownToNearestMinutes2(60);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(0, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes2_Sixty_2_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 1, 33);

            DateTime round = date.RoundDownToNearestMinutes2(60);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(0, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes2_1_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 6, 33);

            DateTime round = date.RoundDownToNearestMinutes2(1);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(6, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes2_2_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 1, 33);

            DateTime round = date.RoundDownToNearestMinutes2(1);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(1, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }

        [TestMethod]
        public void RoundDownToNearestMinutes2_3_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 1, 33);

            DateTime round = date.RoundDownToNearestMinutes2(1.5);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(1, round.Minute);
            Assert.AreEqual(30, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }
        [TestMethod]
        public void RoundDownToNearestMinutes2_4_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 1, 23);

            DateTime round = date.RoundDownToNearestMinutes2(1.5);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(0, round.Minute);
            Assert.AreEqual(0, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }
        [TestMethod]
        public void RoundDownToNearestMinutes2_5_Test()
        {
            DateTime date = new DateTime(2013, 5, 15, 10, 2, 23);

            DateTime round = date.RoundDownToNearestMinutes2(1.5);

            Assert.AreEqual(date.Year, round.Year);
            Assert.AreEqual(date.Month, round.Month);
            Assert.AreEqual(date.Day, round.Day);
            Assert.AreEqual(date.Hour, round.Hour);
            Assert.AreEqual(1, round.Minute);
            Assert.AreEqual(30, round.Second);
            Assert.AreEqual(0, round.Millisecond);
        }
    }
}
