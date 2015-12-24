using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class CloseOrder: IIdentified
    {
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public int Id { get; set; }

        public CloseOrder(Order order)
        {
            this.Order = order;
            this.OrderId = order.Id;
            this.Id = order.Id;
        }
    }
}
