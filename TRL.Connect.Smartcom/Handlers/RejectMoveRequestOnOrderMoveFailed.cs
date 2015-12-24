using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Handlers;
//using TRL.Common.Extensions;
using TRL.Connect.Smartcom.Models;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Models;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Handlers
{
    public class RejectMoveRequestOnOrderMoveFailed:AddedItemHandler<OrderMoveFailed>
    {
        private IDataContext tradingData;
        private RawTradingDataContext rawData;
        private ILogger logger;

        public RejectMoveRequestOnOrderMoveFailed(IDataContext tradingData, RawTradingDataContext rawData, ILogger logger)
            :base(rawData.GetData<OrderMoveFailed>())
        {
            this.tradingData = tradingData;
            this.rawData = rawData;
            this.logger = logger;
        }

        public override void OnItemAdded(OrderMoveFailed item)
        {
            Order order = FindOrder(item.Cookie);

            if (order == null)
                return;

            OrderMoveRequest request = FindRequest(order);

            if (request == null)
                return;

            if (request.IsFailed)
                return;

            request.Failed(BrokerDateTime.Make(DateTime.Now), item.Reason);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, получено уведомление об отклонении запроса {2} на сдвиг заявки.", 
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name,
                request.ToString()));
        }

        private Order FindOrder(int id)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<Order>>().Single(o => o.Id == id);
            }
            catch
            {
                return null;
            }
        }

        private OrderMoveRequest FindRequest(Order order)
        {
            try
            {
                return this.tradingData.GetMoveRequests(order).Last();
            }
            catch
            {
                return null;
            }
        }
    }
}
