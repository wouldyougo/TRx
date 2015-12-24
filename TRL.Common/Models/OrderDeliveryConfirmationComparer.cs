using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class OrderDeliveryConfirmationComparer:EqualityComparer<OrderDeliveryConfirmation>
    {
        public override bool Equals(OrderDeliveryConfirmation x, OrderDeliveryConfirmation y)
        {
            if (x.OrderId == y.OrderId)
                return true;

            return false;
        }

        public override int GetHashCode(OrderDeliveryConfirmation obj)
        {
            return obj.OrderId.GetHashCode();
        }
    }
}
