using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Extensions;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Handlers.TakeProfit
{
    public class PlaceStrategyTakeProfitByPointsOnTrade:StrategyTakeProfitOnItemAddedBase<Trade>
    {
        private double openPrice;

        public PlaceStrategyTakeProfitByPointsOnTrade(StrategyHeader strategyHeader)
            :this(strategyHeader, 
            TradingData.Instance,
            SignalQueue.Instance,
            DefaultLogger.Instance) { }

        public PlaceStrategyTakeProfitByPointsOnTrade(StrategyHeader strategyHeader,
            IDataContext tradingData,
            ObservableQueue<Signal> signalQueue,
            ILogger logger,
            bool measureFromSignalPrice = false)
            :base(strategyHeader, tradingData,signalQueue,logger,measureFromSignalPrice) { }

        private void UpdateOpenPrice(Trade item)
        {
            if (this.measureFromSignalPrice)
                this.openPrice = item.Order.Signal.Price;
            else
                this.openPrice = item.Price;
        }

        private void UpdateCloseSignalAmountWith(double amount)
        {
            this.signal.Amount = Math.Abs(amount);
        }

        private void MakeSignalToBuyWith(double price)
        {
            this.signal =
                new Signal(this.strategyHeader,
                BrokerDateTime.Make(DateTime.Now),
                TradeAction.Buy,
                OrderType.Limit,
                price,
                0,
                this.openPrice - this.ppSettings.Points);
        }

        private void MakeSignalToSellWith(double price)
        {
            this.signal =
                new Signal(this.strategyHeader,
                BrokerDateTime.Make(DateTime.Now),
                TradeAction.Sell,
                OrderType.Limit,
                price,
                0,
                this.openPrice + this.ppSettings.Points);
        }

        public override void MakeSignal(Trade item, double positionAmount)
        {

            UpdateOpenPrice(item);

            if (positionAmount > 0)
                MakeSignalToSellWith(item.Price);
            else if (positionAmount < 0)
                MakeSignalToBuyWith(item.Price);

            if (this.signal != null)
                UpdateCloseSignalAmountWith(positionAmount);
        }
    }
}
