using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Connect.Smartcom.Models;
using TRL.Common.Handlers;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Collections;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Handlers
{
    public class MakeOrderDeliveryConfirmationOnOrderSucceeded:GenericCollectionObserver<OrderSucceeded>
    {
        private IDataContext tradingData;
        private ILogger logger;

        public MakeOrderDeliveryConfirmationOnOrderSucceeded()
            : this(RawTradingData.Instance, TradingData.Instance, DefaultLogger.Instance) { }

        public MakeOrderDeliveryConfirmationOnOrderSucceeded(BaseDataContext rawData, IDataContext tradingData, ILogger logger)
            :base(rawData)
        {
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public override void Update(OrderSucceeded item)
        {
            Order order = GetOrder(item.Cookie);

            if (order == null)
                return;

            OrderDeliveryConfirmation confirmation = new OrderDeliveryConfirmation(order, item.DateTime);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сформировано уведомление о доставке заявки {2}", 
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name, 
                confirmation.ToString()));

            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(confirmation);
        }

        private Order GetOrder(int orderId)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<Order>>().Single(o => o.Id == orderId);
            }
            catch
            {
                return null;
            }
        }
    }
}
