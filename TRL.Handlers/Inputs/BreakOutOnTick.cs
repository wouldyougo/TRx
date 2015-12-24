using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Collections;
using TRL.Common.Extensions.Collections;
using TRL.Common.Handlers;
using TRL.Common.Models;
//using TRL.Common.Extensions.Models;
//using TRL.Common.Extensions;
using TRL.Common.TimeHelpers;
using TRL.Logging;

namespace TRL.Handlers.Inputs
{
    public class BreakOutOnTick:AddedItemHandler<Tick>
    {
        private StrategyHeader strategyHeader;
        private BarSettings barSettings;
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ILogger logger;

        private IEnumerable<Bar> bars;
        private double high, low;
        private Signal signal;

        public BreakOutOnTick(StrategyHeader strategyHeader, IDataContext tradingData, ObservableQueue<Signal> signalQueue, ILogger logger)
            :base(tradingData.Get<ObservableCollection<Tick>>())
        {
            this.strategyHeader = strategyHeader;
            this.tradingData = tradingData;
            this.signalQueue = signalQueue;
            this.logger = logger;

            this.barSettings = this.tradingData.Get<IEnumerable<BarSettings>>().SingleOrDefault(s => s.StrategyId == this.strategyHeader.Id);
        }

        public override void OnItemAdded(Tick item)
        {
            ClearTemporaryProperties();

            if (this.barSettings == null)
                return;

            if (this.strategyHeader.Symbol != item.Symbol)
                return;

            if (this.tradingData.PositionExists(this.strategyHeader))
                return;

            if (this.tradingData.UnfilledExists(this.strategyHeader, OrderType.Market))
                return;

            UpdateBars();

            if (this.bars == null)
                return;

            if (this.bars.Count() < this.barSettings.Period)
                return;

            UpdateHigh();
            UpdateLow();

            if (this.high < item.Price)
                this.signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, this.high, 0, 0);

            if (this.low > item.Price)
                this.signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, this.low, 0, 0);

            if (this.signal == null)
                return;

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал пробой {2}", DateTime.Now, this.GetType().Name, signal.ToString()));

            this.signalQueue.Enqueue(signal);
        }

        private void ClearTemporaryProperties()
        {
            this.signal = null;
            this.bars = null;
            this.high = 0;
            this.low = 0;
        }

        private void UpdateBars()
        {
            this.bars = this.tradingData.Get<IEnumerable<Bar>>().GetNewestBars(this.barSettings.Symbol, this.barSettings.Interval, this.barSettings.Period);
        }

        private void UpdateHigh()
        {
            this.high = this.bars.Max(b => b.High);
        }

        private void UpdateLow()
        {
            this.low = this.bars.Min(b => b.Low);
        }
    }
}
