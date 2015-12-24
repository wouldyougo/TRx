using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Collections;
using TRL.Logging;
//using TRL.Common.Extensions;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Handlers
{
    public class TrailStopOnTick:AddedItemHandler<Tick>
    {
        private IDataContext tradingData;
        private StrategyHeader strategyHeader;
        private ILogger logger;

        private Order stopOrder;
        private double stopPrice;
        private MoveOrder moveOrder;

        public TrailStopOnTick(StrategyHeader strategyHeader,
            IDataContext tradingData,
            ILogger logger)
            : base(tradingData.Get<ObservableCollection<Tick>>())
        {
            this.strategyHeader = strategyHeader;
            this.tradingData = tradingData;
            this.logger = logger;

            this.stopOrder = null;
            this.moveOrder = null;
            this.stopPrice = 0;
        }

        public override void OnItemAdded(Tick item)
        {
            if (!this.tradingData.PositionExists(this.strategyHeader))
                return;

            if (item.Symbol != this.strategyHeader.Symbol)
                return;

            this.stopOrder = null;
            this.moveOrder = null;

            this.stopOrder = this.tradingData.GetUnfilled(this.strategyHeader, OrderType.Stop).Last();

            if (stopOrder == null)
                return;

            if (TickPriceWasNotBecomeCloserToProfit(item.Price))
                return;

            this.stopPrice = MakeNextStopPrice(item.Price);

            this.moveOrder =
                new MoveOrder(this.stopOrder,
                    this.stopPrice,
                    BrokerDateTime.Make(DateTime.Now),
                    String.Format("Необходимо подтянуть стоп к значению {0}", this.stopPrice));

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, создан запрос на сдвиг заявки {2}", 
                DateTime.Now, 
                this.GetType().Name, 
                this.moveOrder.ToString()));

            this.tradingData.Get<ObservableHashSet<MoveOrder>>().Add(this.moveOrder);
        }

        private bool TickPriceWasNotBecomeCloserToProfit(double price)
        {
            if (this.stopOrder.TradeAction == TradeAction.Buy)
                return this.stopOrder.Signal.Price <= price;

            return this.stopOrder.Signal.Price >= price;
        }

        private double MakeNextStopPrice(double currentPrice)
        {
            if (this.stopOrder.TradeAction == TradeAction.Buy)
                return this.stopOrder.Stop - (this.stopOrder.Signal.Price - currentPrice);

            return this.stopOrder.Stop + (currentPrice - this.stopOrder.Signal.Price);
        }
    }
}
