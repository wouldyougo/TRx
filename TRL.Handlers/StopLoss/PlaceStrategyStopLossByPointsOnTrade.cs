using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Extensions;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Handlers.StopLoss
{
    public class PlaceStrategyStopLossByPointsOnTrade:StrategyStopLossOnItemAddedBase<Trade>
    {
        public PlaceStrategyStopLossByPointsOnTrade(StrategyHeader strategyHeader, bool measureFromSignalPrice = false)
            :base(strategyHeader, 
            TradingData.Instance,
            SignalQueue.Instance,
            DefaultLogger.Instance,
            measureFromSignalPrice) { }

        public PlaceStrategyStopLossByPointsOnTrade(StrategyHeader strategyHeader,
            IDataContext tradingData,
            ObservableQueue<Signal> signalQueue,
            ILogger logger,
            bool measureFromSignalPrice = false)
            :base(strategyHeader, tradingData, signalQueue, logger, measureFromSignalPrice)
        {
        }

        public override void MakeSignal(Trade item, double positionAmount)
        {
            if (positionAmount.HasOppositeSignWith(item.Amount))
                return;

            if (item.Amount > 0)
                this.signal =
                    new Signal(this.strategyHeader,
                        BrokerDateTime.Make(DateTime.Now),
                        TradeAction.Sell,
                        OrderType.Stop,
                        item.Price,
                        this.measureFromSignalPrice ? (item.Order.Signal.Price - this.spSettings.Points) : (item.Price - this.spSettings.Points),
                        0);
            else
                this.signal =
                    new Signal(this.strategyHeader,
                        BrokerDateTime.Make(DateTime.Now),
                        TradeAction.Buy,
                        OrderType.Stop,
                        item.Price,
                        this.measureFromSignalPrice ? (item.Order.Signal.Price + this.spSettings.Points) : (item.Price + this.spSettings.Points),
                        0);

            if(this.signal != null)
                this.signal.Amount = Math.Abs(positionAmount);

        }
    }
}