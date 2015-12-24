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

namespace TRL.Handlers.TakeProfit
{
    public class StrategiesTakeProfitByPointsOnTickHandlers:HashSet<StrategyTakeProfitByPointsOnTick>
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ILogger logger;
        private bool measureStopFromSignal;

        public StrategiesTakeProfitByPointsOnTickHandlers()
            : this(TradingData.Instance, SignalQueue.Instance, DefaultLogger.Instance) { }

        public StrategiesTakeProfitByPointsOnTickHandlers(IDataContext tradingData, 
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
                if (!this.tradingData.Get<IEnumerable<ProfitPointsSettings>>().Any(s => s.Strategy.Id == strategyHeader.Id))
                    continue;

                if (!this.tradingData.Get<IEnumerable<TakeProfitOrderSettings>>().Any(s => s.Strategy.Id == strategyHeader.Id))
                    continue;

                base.Add(new StrategyTakeProfitByPointsOnTick(strategyHeader, this.tradingData, this.signalQueue, this.logger, this.measureStopFromSignal));
                this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, StrategyTakeProfitOnTick handler has been activated for {2} ", 
                    BrokerDateTime.Make(DateTime.Now), 
                    this.GetType().Name, 
                    strategyHeader.ToString()));
            }
        }
    }
}
