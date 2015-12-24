using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Handlers;
using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRx.Trader.BackTest.Test
{
    public class TickCounterHandler:AddedItemHandler<Tick>
    {
        public int TickCounter { get; set; }

        public TickCounterHandler(IDataContext tradingData)
            : base(tradingData.Get<ObservableCollection<Tick>>()) { }

        public override void OnItemAdded(Tick item)
        {
            this.TickCounter++;
        }
    }
}
