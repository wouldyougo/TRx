using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Events;
using TRL.Common.Models;
using TRL.Logging;
using TRL.Common.Handlers;
using TRL.Common;
//using TRL.Common.Extensions;
using TRL.Common.TimeHelpers;

namespace TRL.Handlers.TakeProfit
{
    public abstract class StrategyTakeProfitOnItemAddedBase<T>:AddedItemHandler<T>, IIdentified
    {
        protected StrategyHeader strategyHeader;
        protected IDataContext tradingData;
        protected ObservableQueue<Signal> signalQueue;
        protected ILogger logger;

        protected ProfitPointsSettings ppSettings;
        protected TakeProfitOrderSettings tpoSettings;
        protected bool measureFromSignalPrice;
        protected Signal signal;

        public StrategyTakeProfitOnItemAddedBase(StrategyHeader strategyHeader,
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

            SetProfitPointsSettings();
            SetTakeProfitOrderSettings();
        }

        public override void OnItemAdded(T item)
        {
            this.signal = null;

            if (this.ppSettings == null)
                return;

            if (this.tpoSettings == null)
                return;

            if (this.tradingData.UnfilledExists(this.strategyHeader, OrderType.Market) ||
                this.tradingData.UnfilledExists(this.strategyHeader, OrderType.Limit))
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

        private void SetTakeProfitOrderSettings()
        {
            this.tpoSettings =
                this.tradingData.
                Get<IEnumerable<TakeProfitOrderSettings>>().
                SingleOrDefault(i => i.Strategy.Id == this.strategyHeader.Id);
        }

        private void SetProfitPointsSettings()
        {
            this.ppSettings = this.tradingData.
                Get<IEnumerable<ProfitPointsSettings>>().
                SingleOrDefault(i => i.Strategy.Id == this.strategyHeader.Id);
        }

        public int Id
        {
            get { return this.strategyHeader.Id; }
        }
    }
}
