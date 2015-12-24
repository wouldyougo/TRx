using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
//using TRL.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Logging;
using TRL.Common.Handlers;
using TRL.Common;

namespace TRL.Handlers.Spreads
{
    public class ArbitrageOpenPositionOnSpreadValue : AddedItemHandler<SpreadValue>
    {
        private ArbitrageSettings arbitrageSetings;
        private StrategyHeader strategyHeader;
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ILogger logger;
        private bool isLeftLegStrategy;

        public ArbitrageOpenPositionOnSpreadValue(ArbitrageSettings arbitrageSettings, StrategyHeader strategyHeader, IDataContext tradingData, ObservableQueue<Signal> signalQueue, ILogger logger)
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
            if (strategyHeader.Amount <= Math.Abs(tradingData.GetAmount(strategyHeader)))
                return;

            if (tradingData.UnfilledExists(strategyHeader, OrderType.Market))
                return;

            TradeAction? action = GetTradeAction(item);

            if (action == null)
                return;

            Signal signal = MakeSignal(item, action.Value);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сгенерирован {2}.", DateTime.Now, this.GetType().Name, signal.ToString()));

            signalQueue.Enqueue(signal);
        }

        private Signal MakeSignal(SpreadValue item, TradeAction action)
        {
            StrategyVolumeChangeStep strategyVolumeChangeStep = GetStrategyVolumeChangeStep(this.strategyHeader);

            double price = GetPrice(item, action);

            Signal signal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), action, OrderType.Market, price, 0, 0);

            if (strategyVolumeChangeStep != null)
                signal.Amount = GetAmount(strategyVolumeChangeStep.Amount);

            return signal;
        }

        private StrategyVolumeChangeStep GetStrategyVolumeChangeStep(StrategyHeader strategyHeader)
        {
            return this.tradingData.Get<IEnumerable<StrategyVolumeChangeStep>>().FirstOrDefault(s => s.StrategyId == strategyHeader.Id);
        }

        private TradeAction? GetTradeAction(SpreadValue item)
        {
            if (arbitrageSetings.SpreadSettings.SellAfterPrice <= item.SellAfterPrice)
                return isLeftLegStrategy ? TradeAction.Sell : TradeAction.Buy;

            if (arbitrageSetings.SpreadSettings.BuyBeforePrice >= item.BuyBeforePrice)
                return isLeftLegStrategy ? TradeAction.Buy : TradeAction.Sell;

            return null;
        }

        private double GetPrice(SpreadValue item, TradeAction action)
        {
            if (isLeftLegStrategy)
                return action == TradeAction.Sell ? item.SellAfterPrice : item.BuyBeforePrice;
            else
                return action == TradeAction.Sell ? item.BuyBeforePrice : item.SellAfterPrice;
        }

        private double GetAmount(double step)
        {
            double unfilledAmount = strategyHeader.Amount - Math.Abs(tradingData.GetAmount(strategyHeader));

            if (unfilledAmount < step)
                return unfilledAmount;
            else
                return step;
        }
    }
}
