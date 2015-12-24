using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Handlers
{
    public interface IOrderManager
    {
        void PlaceOrder(Order order);
        void MoveOrder(Order order, double price);
        void CancelOrder(Order order);
    }
}
