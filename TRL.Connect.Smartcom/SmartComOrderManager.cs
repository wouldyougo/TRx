using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using SmartCOM3Lib;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Logging;
using TRL.Common;
using TRL.Common.Handlers;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom
{
    public class SmartComOrderManager:IOrderManager
    {
        private IGenericSingleton<StServer> singleton;
        private BaseDataContext rawData;
        private ILogger logger;

        public SmartComOrderManager() :
            this(new StServerSingleton(), RawTradingData.Instance, DefaultLogger.Instance) { }

        public SmartComOrderManager(IGenericSingleton<StServer> singleton, BaseDataContext rawData, ILogger logger)
        {
            this.singleton = singleton;
            this.rawData = rawData;
            this.logger = logger;
        }

        public void PlaceOrder(Order order)
        {
            RawOrderFactory factory = new RawOrderFactory(new FortsTradingSchedule());

            RawOrder rawOrder = factory.Make(order);

            this.singleton.Instance.PlaceOrder(rawOrder.Portfolio, rawOrder.Symbol, rawOrder.Action, rawOrder.Type, rawOrder.Validity, rawOrder.Price, rawOrder.Amount, rawOrder.Stop, rawOrder.Cookie);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, отправлена заявка {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}", 
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name,
                rawOrder.Cookie, 
                rawOrder.Portfolio, 
                rawOrder.Symbol, 
                rawOrder.Action, 
                rawOrder.Type, 
                rawOrder.Validity, 
                rawOrder.Price,
                rawOrder.Amount, 
                rawOrder.Stop));
        }

        public void MoveOrder(Order order, double price)
        {
        }

        public void CancelOrder(Order order)
        {
            UpdateOrder update = GetUpdate(order.Id);

            if (update == null)
                return;

            this.singleton.Instance.CancelOrder(order.Portfolio, order.Symbol, update.OrderId);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, отправлен запрос на отмену заявки {2}", 
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name, 
                order.ToString()));
        }

        private IEnumerable<UpdateOrder> GetUpdates(int orderId)
        {
            try
            {
                return this.rawData.GetData<UpdateOrder>().Where(u => u.Cookie == orderId);
            }
            catch
            {
                return null;
            }
        }

        private UpdateOrder GetUpdate(int orderId)
        {
            try
            {
                return GetUpdates(orderId).Last();
            }
            catch
            {
                return null;
            }
        }
    }
}
