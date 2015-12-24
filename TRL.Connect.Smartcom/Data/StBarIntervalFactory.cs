using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;

namespace TRL.Connect.Smartcom.Data
{
    public static class StBarIntervalFactory
    {
        public static StBarInterval Make(int seconds)
        {
            switch (seconds)
            {
                case 60:
                    return StBarInterval.StBarInterval_1Min;
                case 300:
                    return StBarInterval.StBarInterval_5Min;
                case 600:
                    return StBarInterval.StBarInterval_10Min;
                case 900:
                    return StBarInterval.StBarInterval_15Min;
                case 1800:
                    return StBarInterval.StBarInterval_30Min;
                case 3600:
                    return StBarInterval.StBarInterval_60Min;
                case 7200:
                    return StBarInterval.StBarInterval_2Hour;
                case 14400:
                    return StBarInterval.StBarInterval_4Hour;
                case 86400:
                    return StBarInterval.StBarInterval_Day;
                case 604800:
                    return StBarInterval.StBarInterval_Week;
                case 2592000:
                    return StBarInterval.StBarInterval_Month;
                case 7776000:
                    return StBarInterval.StBarInterval_Quarter;
                case 31536000:
                    return StBarInterval.StBarInterval_Year;
                default:
                    return StBarInterval.StBarInterval_Tick;
            }
        }
    }
}
