using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Data
{
    public class RawTradingData : RawTradingDataContext
    {
        private static RawTradingData orderData = null;

        public static RawTradingData Instance
        {
            get
            {
                if (orderData == null)
                    orderData = new RawTradingData();

                return orderData;
            }
        }

        private RawTradingData() { }

    }
}
