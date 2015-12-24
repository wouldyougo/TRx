using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.TimeHelpers;
using TRL.Common.Data;

namespace TRL.Common.Models
{
    public class OrderCancellationRequest:IIdentified, IDateTime
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public OrderCancellationRequest(Order order, string description)
        {
            this.Id = SerialIntegerFactory.Make();
            this.OrderId = order.Id;
            this.Order = order;
            this.Description = description;
            this.DateTime = BrokerDateTime.Make(DateTime.Now);
        }

        public override string ToString()
        {
            return String.Format("Запрос на отмену заявки Id: {0}, DateTime: {1}, Description: {2}, OrderId: {3}", this.Id, this.DateTime, this.Description, this.OrderId);
        }
    }
}
