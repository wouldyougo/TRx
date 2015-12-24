using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Models
{
    public class OrderFailed
    {
        public int Cookie { get; set; }
        public string OrderId { get; set; }
        public string Reason { get; set; }

        public OrderFailed() { }

        public OrderFailed(int cookie, string orderId, string reason)
        {
            this.Cookie = cookie;
            this.OrderId = orderId;
            this.Reason = reason;
        }
    }
}
