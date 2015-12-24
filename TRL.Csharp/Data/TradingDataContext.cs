using TRL.Csharp.Collections;
using TRL.Csharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Data
{
    public class TradingDataContext:BaseDataContext
    {
        public GenericObservableHashSet<Trade> Trades { get; private set; }
        public GenericObservableList<Bar> Bars { get; private set; }
        public GenericObservableHashSet<Order> Orders { get; private set; }

        public TradingDataContext()
        {
            this.Trades = new GenericObservableHashSet<Trade>(new IdentifiedEqualityComparer());
            this.Bars = new GenericObservableList<Bar>();
            this.Orders = new GenericObservableHashSet<Order>(new IdentifiedEqualityComparer());
        }

    }
}
