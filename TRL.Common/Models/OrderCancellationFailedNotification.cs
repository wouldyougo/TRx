using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;

namespace TRL.Common.Models
{
    public class OrderCancellationFailedNotification:IIdentified, IDateTime
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public OrderCancellationFailedNotification(Order order, DateTime failedDate, string description)
        {
            this.Id = SerialIntegerFactory.Make();
            this.DateTime = failedDate;
            this.Description = description;
            this.Order = order;
            this.OrderId = order.Id;
        }
    }
}
