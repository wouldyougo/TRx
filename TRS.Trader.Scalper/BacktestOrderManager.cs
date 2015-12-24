using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Logging;
using TRL.Common.Handlers;

namespace TRx.Trader.Scalper
{
    public class BacktestOrderManager:IOrderManager
    {
        private IDataContext tradingData;
        private ILogger logger;

        public BacktestOrderManager(IDataContext tradingData, ILogger logger)
        {
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public void PlaceOrder(Order order)
        {
            Bar lastBar = 
                this.tradingData.Get<IEnumerable<Bar>>().LastOrDefault(b => b.Symbol.Equals(order.Symbol));

            if (lastBar == null)
                return;

            Trade trade =
                new Trade(order,
                    order.Portfolio,
                    order.Symbol,
                    lastBar.Close,
                    order.TradeAction == TradeAction.Buy? order.Amount:-order.Amount,
                    lastBar.DateTime);

            order.Signal.DateTime = order.DateTime = trade.DateTime;

            string logMessage = String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, исполнена сделка {2}",
                DateTime.Now,
                this.GetType().Name,
                trade.ToString());

            this.logger.Log(logMessage);

            this.tradingData.Get<ObservableHashSet<Trade>>().Add(trade);
        }

        public void MoveOrder(Order order, double price)
        {
        }

        public void CancelOrder(Order order)
        {
        }
    }
}
