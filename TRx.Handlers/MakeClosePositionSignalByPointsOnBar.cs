using TRL.Common.Handlers;
using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TRL.Common.Extensions;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Collections;
using TRL.Common;
using TRL.Logging;
using TRL.Common.TimeHelpers;

namespace TRx.Handlers
{
    public abstract class MakeClosePositionSignalByPointsOnBar : AddedItemHandler<Bar>
    {
        protected IDataContext tradingData;
        protected ObservableQueue<Signal> signalQueue;
        protected ILogger logger;

        protected StrategyHeader strategyHeader;
        protected BarSettings barSettings;
        protected double points;

        public MakeClosePositionSignalByPointsOnBar(StrategyHeader strategyHeader,
            double points,
            IDataContext tradingData,
            ObservableQueue<Signal> signalQueue,
            ILogger logger)
            : base(tradingData.Get<ObservableCollection<Bar>>())
        {
            this.strategyHeader = strategyHeader;
            this.tradingData = tradingData;
            this.signalQueue = signalQueue;
            this.logger = logger;

            this.barSettings =
                this.tradingData.Get<IEnumerable<BarSettings>>().SingleOrDefault(s => s.StrategyId == this.strategyHeader.Id);
            this.points = points;
        }

        public override void OnItemAdded(Bar item)
        {
            if (this.barSettings == null)
                return;

            if (this.points == 0)
                return;

            if (item.Symbol != this.barSettings.Symbol)
                return;

            if (item.Interval != this.barSettings.Interval)
                return;

            if (!this.tradingData.PositionExists(this.strategyHeader))
                return;

            if (this.tradingData.UnfilledExists(this.strategyHeader, OrderType.Market))
                return;

            double amount = this.tradingData.GetAmount(this.strategyHeader);

            TradeAction closeAction = GetClosePositionAction(amount);

            double closePrice = CalculatePositionClosePrice();

            if (!ItsTimeToClosePosition(item, closePrice, closeAction))
                return;

            Signal signal =
                new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), closeAction, OrderType.Market, item.Close, 0, 0);

            string logMessage = String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сгенерирован сигнал {2}",
                DateTime.Now,
                this.GetType().Name,
                signal.ToString());

            this.logger.Log(logMessage);

            this.signalQueue.Enqueue(signal);
        }

        private TradeAction GetClosePositionAction(double amount)
        {
            return amount > 0 ? TradeAction.Sell : TradeAction.Buy;
        }

        public abstract double CalculatePositionClosePrice();

        public abstract bool ItsTimeToClosePosition(Bar item, double closePrice, TradeAction closeAction);

    }
}
