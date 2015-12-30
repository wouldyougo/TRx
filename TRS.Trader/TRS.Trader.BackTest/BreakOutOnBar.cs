using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Extensions.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Handlers;
using TRL.Common.Models;
//using TRL.Common.Extensions.Models;
//using TRL.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Logging;
using TRL.Common.TimeHelpers;

namespace TRx.Trader.BackTest
{
    public class BreakOutOnBar:AddedItemHandler<Bar>
    {
        private StrategyHeader strategyHeader;
        private BarSettings barSettings;

        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ILogger logger;

        public BreakOutOnBar(StrategyHeader strategyHeader, IDataContext tradingData, ObservableQueue<Signal> signalQueue, ILogger logger)
            :base(tradingData.Get<ObservableCollection<Bar>>())
        {
            this.strategyHeader = strategyHeader;
            this.tradingData = tradingData;
            this.signalQueue = signalQueue;
            this.logger = logger;

            this.barSettings = this.tradingData.Get<IEnumerable<BarSettings>>().SingleOrDefault(s => s.StrategyId == this.strategyHeader.Id);
        }

        public override void OnItemAdded(Bar item)
        {
            if (this.barSettings == null)
                return;

            if (this.tradingData.PositionExists(this.strategyHeader))
                return;

            if (this.tradingData.UnfilledExists(this.strategyHeader, OrderType.Market))
                return;

            IEnumerable<Bar> bars =
                this.tradingData.Get<IEnumerable<Bar>>().GetNewestBars(this.barSettings.Symbol,
                this.barSettings.Interval,
                this.barSettings.Period);

            if (bars.Count() < this.barSettings.Period)
                return;

            TradeAction? action = GetTradeAction(bars);

            if (action == null)
                return;

            double price = action == TradeAction.Buy ? item.High : item.Low;

            Signal signal =
                new Signal(this.strategyHeader,
                    BrokerDateTime.Make(DateTime.Now),
                    action.Value,
                    OrderType.Market,
                    price,
                    0,
                    0);

            string logMessage = String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сгенерирован сигнал {2}",
                DateTime.Now,
                this.GetType().Name,
                signal.ToString());

            this.logger.Log(logMessage);

            this.signalQueue.Enqueue(signal);
        }

        private Nullable<TradeAction> GetTradeAction(IEnumerable<Bar> bars)
        {
            if (bars.LastBarHasHighestHigh())
                return TradeAction.Buy;

            if (bars.LastBarHasLowestLow())
                return TradeAction.Sell;

            return null;
        }
    }
}
