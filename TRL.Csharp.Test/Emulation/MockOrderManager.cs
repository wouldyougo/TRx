using TRL.Csharp.Collections;
using TRL.Csharp.Data;
using TRL.Csharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Test.Emulation
{
    public class MockOrderManager:OrderManager
    {
        private DataContext tradingData;

        public MockOrderManager(DataContext tradingData)
        {
            this.tradingData = tradingData;
        }

        public void PlaceOrder(Order item)
        {
            this.tradingData.Get<GenericObservableHashSet<Order>>().Add(item);

            Trade trade = new Trade(item.Id, item.Portfolio, item.Symbol, DateTime.Now, item.Price, item.Amount);
            trade.Order = item;

            this.tradingData.Get<GenericObservableHashSet<Trade>>().Add(trade);
        }
    }
}
