using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace TRL.Common.Models
{
    public class OrderDeliveryConfirmation:IDateTime
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public DateTime DateTime { get; set; }

        public OrderDeliveryConfirmation(Order order, DateTime date)
        {
            this.Order = order;
            this.OrderId = order.Id;
            this.DateTime = date;
        }

        public override string ToString()
        {
            return String.Format("Order delivery confirmation for: {0}, {1}",
                this.OrderId,
                this.DateTime.ToString(CultureInfo.InvariantCulture));
        }
    }
}
