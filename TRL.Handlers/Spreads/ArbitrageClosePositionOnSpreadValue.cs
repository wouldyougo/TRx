using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Handlers;
using TRL.Common.Models;
using TRL.Logging;
using TRL.Common.TimeHelpers;
//using TRL.Common.Extensions;

namespace TRL.Handlers.Spreads
{
    public class ArbitrageClosePositionOnSpreadValue : AddedItemHandler<SpreadValue>
    {
        private ArbitrageSettings arbitrageSetings;
        private StrategyHeader strategyHeader;
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ILogger logger;
        private bool isLeftLegStrategy;

        public ArbitrageClosePositionOnSpreadValue(ArbitrageSettings arbitrageSettings, StrategyHeader strategyHeader, IDataContext tradingData, ObservableQueue<Signal> signalQueue, ILogger logger)
            : base(tradingData.Get<ObservableCollection<SpreadValue>>())
        {
            this.arbitrageSetings = arbitrageSettings;
            this.strategyHeader = strategyHeader;
            this.tradingData = tradingData;
            this.signalQueue = signalQueue;
            this.logger = logger;

            this.isLeftLegStrategy = arbitrageSettings.LeftLeg.Any(s => s.Id == strategyHeader.Id);
        }

        public override void OnItemAdded(SpreadValue item)
        {
            if (!tradingData.PositionExists(strategyHeader))
                return;

            if (tradingData.UnfilledExists(strategyHeader, OrderType.Market))
                return;

            TradeAction? action = GetAction(item);

            if (action == null)
                return;

            Signal signal = MakeSignal(item, action.Value);
            
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сгенерирован {2}.", DateTime.Now, this.GetType().Name, signal.ToString()));

            signalQueue.Enqueue(signal);
        }

        private Signal MakeSignal(SpreadValue item, TradeAction action)
        {
            StrategyVolumeChangeStep strategyVolumeChangeStep = GetStrategyVolumeChangeStep(this.strategyHeader);

            Signal signal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), action, OrderType.Market, arbitrageSetings.SpreadSettings.FairPrice, 0, 0);

            if (strategyVolumeChangeStep != null)
                signal.Amount = GetAmount(strategyVolumeChangeStep.Amount);

            return signal;
        }

        private TradeAction? GetAction(SpreadValue item)
        {
            if (this.tradingData.HasShortPosition(this.strategyHeader))
            {
                if (arbitrageSetings.SpreadSettings.FairPrice >= item.SellAfterPrice)
                    return TradeAction.Buy;
            }

            if (this.tradingData.HasLongPosition(this.strategyHeader))
            {
                if (arbitrageSetings.SpreadSettings.FairPrice <= item.BuyBeforePrice)
                    return TradeAction.Sell;
            }

            return null;
        }

        private double GetAmount(double step)
        {
            double positionAmount = Math.Abs(tradingData.GetAmount(strategyHeader));

            if (positionAmount < step)
                return positionAmount;
            else
                return step;
        }

        private StrategyVolumeChangeStep GetStrategyVolumeChangeStep(StrategyHeader strategyHeader)
        {
            return this.tradingData.Get<IEnumerable<StrategyVolumeChangeStep>>().FirstOrDefault(s => s.StrategyId == strategyHeader.Id);
        }
    }
}
