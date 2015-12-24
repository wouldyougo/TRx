using TRL.Common.Data;
using TRL.Common.Events;
using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Data
{
    public class OrderBookUpdateTimeStamp : OrderBookLastUpdateTimeStamped
    {
        private static OrderBookUpdateTimeStamp instance = null;

        public static OrderBookUpdateTimeStamp Instance
        {
            get
            {
                if (instance == null)
                    instance = new OrderBookUpdateTimeStamp();

                return instance;
            }
        }

        private OrderBookUpdateTimeStamp() :
            base(OrderBook.Instance) { }
    }
}
