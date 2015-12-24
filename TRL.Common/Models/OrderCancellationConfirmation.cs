using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.TimeHelpers;
using System.Globalization;

namespace TRL.Common.Models
{
    public class OrderCancellationConfirmation:IIdentified, IDateTime
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public OrderCancellationConfirmation(Order order, DateTime cancelDate, string description)
        {
            this.Id = SerialIntegerFactory.Make();
            this.OrderId = order.Id;
            this.Order = order;
            this.Description = description;
            this.DateTime = cancelDate;
        }

        public override string ToString()
        {
            return String.Format("Подтверждение об отмене заявки {0}, {1}, {2}",
                this.DateTime.ToString(CultureInfo.InvariantCulture),
                this.Description,
                this.Order.ToString());
        }
    }
}
