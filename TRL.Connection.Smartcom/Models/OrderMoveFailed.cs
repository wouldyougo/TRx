using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Models
{
    public class OrderMoveFailed
    {
        public int Cookie { get; set; }
        public string OrderId { get; set; }
        public string Reason { get; set; }

        public OrderMoveFailed() { }

        public OrderMoveFailed(int cookie, string orderId, string reason)
        {
            this.Cookie = cookie;
            this.OrderId = orderId;
            this.Reason = reason;
        }
    }
}
