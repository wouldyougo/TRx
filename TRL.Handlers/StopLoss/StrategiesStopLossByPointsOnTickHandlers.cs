using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Logging;
using TRL.Common;

namespace TRL.Handlers.StopLoss
{
    public class StrategiesStopLossByPointsOnTickHandlers:HashSet<StrategyStopLossByPointsOnTick>
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ILogger logger;
        private bool measureStopFromSignal;

        public StrategiesStopLossByPointsOnTickHandlers()
            : this(TradingData.Instance, SignalQueue.Instance, DefaultLogger.Instance) { }

        public StrategiesStopLossByPointsOnTickHandlers(IDataContext tradingData, 
            ObservableQueue<Signal> signalQueue,
            ILogger logger,
            bool measureStopFromSignal = false)
        {
            this.tradingData = tradingData;
            this.signalQueue = signalQueue;
            this.logger = logger;
            this.measureStopFromSignal = measureStopFromSignal;

            ActivateHandlers();
        }

        private void ActivateHandlers()
        {
            foreach (StrategyHeader strategyHeader in this.tradingData.Get<IEnumerable<StrategyHeader>>())
            {
                if (!this.tradingData.Get<IEnumerable<StopPointsSettings>>().Any(s => s.Strategy.Id == strategyHeader.Id))
                    continue;

                if (!this.tradingData.Get<IEnumerable<StopLossOrderSettings>>().Any(s => s.Strategy.Id == strategyHeader.Id))
                    continue;

                base.Add(new StrategyStopLossByPointsOnTick(strategyHeader, this.tradingData, this.signalQueue, this.logger, this.measureStopFromSignal));
                this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, StrategyStopLossOnTick handler has been activated for {2} ", 
                    BrokerDateTime.Make(DateTime.Now), 
                    this.GetType().Name, 
                    strategyHeader.ToString()));
            }
        }
    }
}
