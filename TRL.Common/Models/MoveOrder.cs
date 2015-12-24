using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.TimeHelpers;
using System.Globalization;

namespace TRL.Common.Models
{
    public class MoveOrder:IIdentified, IDateTime
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public double Price { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime MoveDate { get; set; }
        public string Description { get; set; }

        public MoveOrder(Order order, double price, DateTime date, string description)
        {
            this.Id = SerialIntegerFactory.Make();
            this.DateTime = date;
            this.Order = order;
            this.OrderId = order.Id;
            this.Price = price;
            this.DeliveryDate = DateTime.MinValue;
            this.MoveDate = DateTime.MinValue;
            this.Description = description;
        }

        public bool IsDelivered
        {
            get
            {
                return this.DeliveryDate != DateTime.MinValue;
            }
        }

        public bool IsMoved
        {
            get
            {
                return this.MoveDate != DateTime.MinValue;
            }
        }

        public override string ToString()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            return String.Format("Запрос на перемещение заявки: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
                this.Id,
                this.OrderId,
                this.DateTime.ToString(ci),
                this.Price.ToString("0.0000", ci),
                this.Description,
                this.DeliveryDate.ToString(ci),
                this.MoveDate.ToString(ci),
                this.Order);
        }
    }
}
