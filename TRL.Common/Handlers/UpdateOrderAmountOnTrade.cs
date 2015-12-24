using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Logging;

namespace TRL.Common.Handlers
{
    public class UpdateOrderAmountOnTrade:AddedItemHandler<Trade>
    {
        private ILogger logger;

        public UpdateOrderAmountOnTrade(IDataContext tradingData, ILogger logger)
            :base(tradingData.Get<ObservableHashSet<Trade>>())
        {
            this.logger = logger;
        }

        public override void OnItemAdded(Trade item)
        {
            if (item.Order.IsFilled)
                return;

            if(item.Order.FilledAmount + item.AbsoluteAmount <= item.Order.Amount)
                item.Order.FilledAmount += item.AbsoluteAmount;

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, исполненный объем заявки обновлен {2}.", 
                BrokerDateTime.Make(DateTime.Now), this.GetType().Name, item.Order.ToString()));
        }
    }
}
