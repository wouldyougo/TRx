using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;

namespace TRL.Connect.Smartcom.Data
{
    public class BarIntervalFactory
    {
        public static int Make(StBarInterval interval)
        {
            switch (interval)
            {
                case StBarInterval.StBarInterval_1Min:
                    return 60;
                case StBarInterval.StBarInterval_5Min:
                    return 300;
                case StBarInterval.StBarInterval_10Min:
                    return 600;
                case StBarInterval.StBarInterval_15Min:
                    return 900;
                case StBarInterval.StBarInterval_30Min:
                    return 1800;
                case StBarInterval.StBarInterval_60Min:
                    return 3600;
                case StBarInterval.StBarInterval_2Hour:
                    return 7200;
                case StBarInterval.StBarInterval_4Hour:
                    return 14400;
                case StBarInterval.StBarInterval_Day:
                    return 86400;
                case StBarInterval.StBarInterval_Week:
                    return 604800;
                case StBarInterval.StBarInterval_Month:
                    return 2592000;
                case StBarInterval.StBarInterval_Quarter:
                    return 7776000;
                case StBarInterval.StBarInterval_Year:
                    return 31536000;
                case StBarInterval.StBarInterval_Tick:
                    return 0;
                default:
                    return 0;
            }
        }
    }
}
