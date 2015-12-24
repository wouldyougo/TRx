using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;

namespace TRL.Common.Models
{
    public class OrderRejection:IIdentified, IDateTime
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public OrderRejection(Order order, DateTime rejectionDate, string description)
        {
            this.Id = SerialIntegerFactory.Make();
            this.DateTime = rejectionDate;
            this.OrderId = order.Id;
            this.Order = order;
            this.Description = description;
        }
    }
}
