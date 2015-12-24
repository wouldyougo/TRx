using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class OpenOrder:IIdentified
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public OpenOrder(Order order)
        {
            this.Id = order.Id;
            this.Order = order;
            this.OrderId = order.Id;
        }
    }
}
