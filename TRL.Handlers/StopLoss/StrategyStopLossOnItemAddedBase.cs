using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Events;
using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Extensions;
using TRL.Logging;
using TRL.Common.Handlers;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Handlers.StopLoss
{
    public abstract class StrategyStopLossOnItemAddedBase<T>:AddedItemHandler<T>, IIdentified
    {
        protected StrategyHeader strategyHeader;
        protected IDataContext tradingData;
        protected ObservableQueue<Signal> signalQueue;
        protected ILogger logger;

        protected bool measureFromSignalPrice;
        protected StopPointsSettings spSettings;
        protected StopLossOrderSettings sloSettings;
        protected Signal signal;

        public StrategyStopLossOnItemAddedBase(StrategyHeader strategyHeader,
            IDataContext tradingData,
            ObservableQueue<Signal> signalQueue,
            ILogger logger,
            bool measureFromSignalPrice = false)
            :base(tradingData.Get<ItemAddedNotifier<T>>())
        {
            this.strategyHeader = strategyHeader;
            this.tradingData = tradingData;
            this.signalQueue = signalQueue;
            this.logger = logger;
            this.measureFromSignalPrice = measureFromSignalPrice;

            SetStopPointsSettings();
            SetStopLossOrderSettings();
        }

        public override void OnItemAdded(T item)
        {
            this.signal = null;

            if (this.spSettings == null)
                return;

            if (this.sloSettings == null)
                return;

            if (this.tradingData.UnfilledExists(this.strategyHeader, OrderType.Stop) ||
                this.tradingData.UnfilledExists(this.strategyHeader, OrderType.Market))
                return;

            if (this.tradingData.UndeliveredExists(this.strategyHeader, OrderType.Stop) ||
                this.tradingData.UndeliveredExists(this.strategyHeader, OrderType.Market))
                return;

            double amount = this.tradingData.GetAmount(this.strategyHeader);

            if (amount == 0)
                return;

            MakeSignal(item, amount);

            if (this.signal == null)
                return;

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сгенерирован сигнал {2}.",
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name,
                signal.ToString()));

            this.signalQueue.Enqueue(signal);

            this.signal = null;
        }

        public abstract void MakeSignal(T item, double positionAmount);

        private void SetStopLossOrderSettings()
        {
            try
            {
                this.spSettings =
                    this.tradingData.
                    Get<IEnumerable<StopPointsSettings>>().
                    SingleOrDefault(i => i.Strategy.Id == this.strategyHeader.Id);
            }
            catch
            {
                this.spSettings = null;
            }
        }

        private void SetStopPointsSettings()
        {
            try
            {
                this.sloSettings =
                    this.tradingData.
                    Get<IEnumerable<StopLossOrderSettings>>().
                    SingleOrDefault(i => i.Strategy.Id == this.strategyHeader.Id);
            }
            catch
            {
                this.sloSettings = null;
            }
        }

        public int Id
        {
            get { return this.strategyHeader.Id; }
        }
    }
}
