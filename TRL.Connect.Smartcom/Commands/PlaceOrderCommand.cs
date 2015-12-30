using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using SmartCOM3Lib;
using TRL.Common.Models;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

/*
namespace TRL.Connect.Smartcom.Commands
{
    public class PlaceOrderCommand
    {
        private Order order;
        private IGenericSingleton<StServer> singleton;
        private ILogger logger;

        public PlaceOrderCommand(Order order, IGenericSingleton<StServer> singleton, ILogger logger)
        {
            this.order = order;
            this.singleton = singleton;
            this.logger = logger;
        }

        public void Execute()
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
        
    }
}
*/
