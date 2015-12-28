using TRL.Common.Data;
using TRL.Common.Events;
using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Data
{
    public class TickDataFeedUpdateTimeStamp : ItemAddedLastTimeStamped<Tick>
    {
        private static TickDataFeedUpdateTimeStamp instance = null;

        public static TickDataFeedUpdateTimeStamp Instance
        {
            get
            {
                if (instance == null)
                    instance = new TickDataFeedUpdateTimeStamp();

                return instance;
            }
        }

        private TickDataFeedUpdateTimeStamp():
            base(TradingData.Instance.Get<ItemAddedNotifier<Tick>>()) { }
    }
}
