using TRL.Csharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Data
{
    public interface OrderManager
    {
        void PlaceOrder(Order item);
    }
}
