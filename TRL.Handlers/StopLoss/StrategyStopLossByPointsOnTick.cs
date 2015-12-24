using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using TRL.Common.Extensions;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Collections;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Handlers.StopLoss
{
    public class StrategyStopLossByPointsOnTick:StrategyStopLossOnItemAddedBase<Tick>
    {
        private double openPrice;
        private Trade openTrade;

        public StrategyStopLossByPointsOnTick(StrategyHeader strategyHeader, bool measureFromSignalPrice = false)
            : this(strategyHeader,
            TradingData.Instance,
            SignalQueue.Instance,
            DefaultLogger.Instance, measureFromSignalPrice) { }

        public StrategyStopLossByPointsOnTick(StrategyHeader strategyHeader,
            IDataContext tradingData,
            ObservableQueue<Signal> signalQueue,
            ILogger logger,
            bool measureFromSignalPrice = false)
            : base(strategyHeader, tradingData, signalQueue, logger, measureFromSignalPrice) { }

        private void UpdateOpenPrice(Trade item)
        {
            if (this.measureFromSignalPrice)
                this.openPrice = item.Order.Signal.Price;
            else
                this.openPrice = item.Price;
        }

        private void UpdateOpenTrade()
        {
            this.openTrade = this.tradingData.GetPositionOpenTrade(this.strategyHeader);
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
                OrderType.Market,
                price,
                0,
                0);
        }

        private void MakeSignalToSellWith(double price)
        {
            this.signal =
                new Signal(this.strategyHeader,
                    BrokerDateTime.Make(DateTime.Now),
                    TradeAction.Sell,
                    OrderType.Market,
                    price,
                    0,
                    0);
        }

        public override void MakeSignal(Tick item, double positionAmount)
        {
            UpdateOpenTrade();

            if (this.openTrade == null)
                return;

            UpdateOpenPrice(this.openTrade);

            if (positionAmount > 0 && this.openPrice - this.spSettings.Points >= item.Price)
                MakeSignalToSellWith(item.Price);
            else if (positionAmount < 0 && this.openPrice + this.spSettings.Points <= item.Price)
                MakeSignalToBuyWith(item.Price);

            if(this.signal != null)
                UpdateCloseSignalAmountWith(positionAmount);
        }
    }
}
