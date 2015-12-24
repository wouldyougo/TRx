using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Test
{
    [TestClass]
    public class FortsTradingScheduleTests
    {
        [TestMethod]
        public void Common_FortsTradingSchedule_It_Is_DateToTrade()
        {
            DateTime dateTime = new DateTime(2013, 2, 15, 11, 0, 0);

            ITradingSchedule tradingSchedule = new FortsTradingSchedule();

            Assert.IsTrue(tradingSchedule.ItIsTimeToTrade(dateTime));
        }

        [TestMethod]
        public void Common_FortsTradingSchedule_It_Is_Not_DateToTrade()
        {
            DateTime dateTime = new DateTime(2013, 2, 16, 11, 0, 0);

            ITradingSchedule tradingSchedule = new FortsTradingSchedule();

            Assert.IsFalse(tradingSchedule.ItIsTimeToTrade(dateTime));
        }

        [TestMethod]
        public void Common_FortsTradingSchedule_It_Is_Not_TimeToTrade()
        {
            DateTime dateTime = new DateTime(2013, 2, 15, 6, 0, 0);

            ITradingSchedule tradingSchedule = new FortsTradingSchedule();

            Assert.IsFalse(tradingSchedule.ItIsTimeToTrade(dateTime));
        }

        [TestMethod]
        public void Common_FortsTradingSchedule_PreClearingBegin_It_Is_Not_TimeToTrade()
        {
            DateTime dateTime = new DateTime(2013, 2, 15, 14, 0, 0);

            ITradingSchedule tradingSchedule = new FortsTradingSchedule();

            Assert.IsFalse(tradingSchedule.ItIsTimeToTrade(dateTime));
        }

        [TestMethod]
        public void Common_FortsTradingSchedule_PreClearingEnd_It_Is_Not_TimeToTrade()
        {
            DateTime dateTime = new DateTime(2013, 2, 15, 14, 2, 59);

            ITradingSchedule tradingSchedule = new FortsTradingSchedule();

            Assert.IsFalse(tradingSchedule.ItIsTimeToTrade(dateTime));
        }

        [TestMethod]
        public void Common_FortsTradingSchedule_After_PreClearing_ItIsTimeToTrade()
        {
            DateTime dateTime = new DateTime(2013, 2, 15, 14, 3, 1);

            ITradingSchedule tradingSchedule = new FortsTradingSchedule();

            Assert.IsTrue(tradingSchedule.ItIsTimeToTrade(dateTime));
        }

        [TestMethod]
        public void Common_FortsTradingSchedule_ClearingBegin_It_Is_Not_TimeToTrade()
        {
            DateTime dateTime = new DateTime(2013, 2, 15, 18, 45, 0);

            ITradingSchedule tradingSchedule = new FortsTradingSchedule();

            Assert.IsFalse(tradingSchedule.ItIsTimeToTrade(dateTime));
        }

        [TestMethod]
        public void Common_FortsTradingSchedule_ClearingEnd_It_Is_Not_TimeToTrade()
        {
            DateTime dateTime = new DateTime(2013, 2, 15, 18, 59, 59);

            ITradingSchedule tradingSchedule = new FortsTradingSchedule();

            Assert.IsFalse(tradingSchedule.ItIsTimeToTrade(dateTime));
        }

        [TestMethod]
        public void Common_FortsTradingSchedule_After_Clearing_ItIsTimeToTrade()
        {
            DateTime dateTime = new DateTime(2013, 2, 15, 19, 0, 0);

            ITradingSchedule tradingSchedule = new FortsTradingSchedule();

            Assert.IsTrue(tradingSchedule.ItIsTimeToTrade(dateTime));
        }

        [TestMethod]
        public void Common_FortsTradingSchedule_EndOfSession_It_Is_Not_TimeToTrade()
        {
            DateTime dateTime = new DateTime(2013, 2, 15, 23, 50, 0);

            ITradingSchedule tradingSchedule = new FortsTradingSchedule();

            Assert.IsFalse(tradingSchedule.ItIsTimeToTrade(dateTime));
        }

        [TestMethod]
        public void Common_FortsTradingSchedule_SessionEnd_test()
        {
            DateTime nowDate = DateTime.Now;

            DateTime sessionEnd = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, 23, 0, 0);
            //DateTime sessionEnd = new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, 19, 0, 0);

            ITradingSchedule fortsSchedule = new FortsTradingSchedule();

            if(nowDate.Hour >= 19)
                Assert.AreEqual(sessionEnd.AddDays(1), fortsSchedule.SessionEnd);
            else
                Assert.AreEqual(sessionEnd, fortsSchedule.SessionEnd);
        }
    }
}
