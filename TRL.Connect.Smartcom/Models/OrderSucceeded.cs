using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Models
{
    public class OrderSucceeded:IDateTime
    {
        public int Cookie { get; set; }
        public string OrderId { get; set; }
        public DateTime DateTime { get; set; }

        public OrderSucceeded(int cookie, string orderId)
        {
            this.Cookie = cookie;
            this.OrderId = orderId;
            this.DateTime = BrokerDateTime.Make(DateTime.Now);
        }
    }
}
