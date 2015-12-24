using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Models
{
    public class OrderMoveRequest:IIdentified, IDateTime
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public double LimitPrice { get; set; }
        public double StopPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime FaultDate { get; set; }
        public string FaultDescription { get; set; }

        public OrderMoveRequest() { }

        public OrderMoveRequest(Order order, double limitPrice, double stopPrice, string description)
        {
            this.Id = SerialIntegerFactory.Make();
            this.Order = order;
            this.OrderId = order.Id;
            this.DateTime = BrokerDateTime.Make(DateTime.Now);
            this.LimitPrice = limitPrice;
            this.StopPrice = stopPrice;
            this.Description = description;
            this.DeliveryDate = DateTime.MinValue;
            this.ExpirationDate = this.DateTime.AddSeconds(60);
            this.FaultDate = DateTime.MinValue;
            this.FaultDescription = string.Empty;
        }

        public bool IsDelivered
        {
            get
            {
                return this.DeliveryDate != DateTime.MinValue;
            }
        }

        public bool IsExpired
        {
            get
            {
                return BrokerDateTime.Make(DateTime.Now) > this.ExpirationDate;
            }
        }

        public bool IsFailed
        {
            get
            {
                return this.FaultDate != DateTime.MinValue;
            }
        }

        public void Failed(DateTime date, string reason)
        {
            this.FaultDate = date;
            this.FaultDescription = reason;
        }
    }
}
