using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Extensions.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Handlers;
using TRL.Common.Models;
using TRL.Logging;
using TRL.Common.TimeHelpers;
using TRx.Indicators;

namespace TRx.Handlers
{
    public class ReversOnTick:AddedItemHandler<Tick>
    {
        private StrategyHeader strategyHeader;
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ILogger logger;

        public ReversOnTick(StrategyHeader strategyHeader, IDataContext tradingData, ObservableQueue<Signal> signalQueue, ILogger logger)
            :base(tradingData.Get<ObservableCollection<Tick>>())
        {
            this.strategyHeader = strategyHeader;
            this.tradingData = tradingData;
            this.signalQueue = signalQueue;
            this.logger = logger;
        }

        public override void OnItemAdded(Tick item)
        {
            if (item.Symbol != this.strategyHeader.Symbol)
                return;

            BarSettings bs = this.tradingData.Get<IEnumerable<BarSettings>>().SingleOrDefault(s => s.StrategyId == this.strategyHeader.Id);

            if (bs == null)
                return;

            SMASettings ss = this.tradingData.Get<IEnumerable<SMASettings>>().SingleOrDefault(s => s.StrategyId == this.strategyHeader.Id);

            if (ss == null)
                return;


            //double strategyAmount = this.tradingData.GetAmount(this.strategyHeader);
            //if (strategyAmount > 0)
            //    return;
            
            bool UnfilledExists = this.tradingData.UnfilledExists(this.strategyHeader);
            if (UnfilledExists)
            {
                return;
            }

            IEnumerable<Bar> bars = this.tradingData.Get<IEnumerable<Bar>>().GetNewestBars(bs.Symbol, bs.Interval, bs.Period + 1);
            

            if (this.tradingData.UnfilledExists(this.strategyHeader, OrderType.Limit))
                return;

            if (bars == null || bars.Count() == 0)
                return;

            if (bars.Count() < ss.PeriodFast + 1)
                return;

            if (bars.Count() < ss.PeriodSlow + 1)
                return;

            IEnumerable<double> closePrices = from b in bars
                                              select b.Close;

            //IEnumerable<double> fastSMA = Ema.Make(closePrices, ss.PeriodFast);
            //IEnumerable<double> slowSMA = Ema.Make(closePrices, ss.PeriodSlow);
            IEnumerable<double> fastSMA = Indicator.EMA(closePrices.ToList<double>(), ss.PeriodFast);
            IEnumerable<double> slowSMA = Indicator.EMA(closePrices.ToList<double>(), ss.PeriodSlow);

            //IEnumerable<double> fastSMA = null;
            //IEnumerable<double> slowSMA = null;

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, fast {2} slow {3}", DateTime.Now, this.GetType().Name, fastSMA.Last(), slowSMA.Last()));

            //надо лонг
            if (slowSMA.Last() < fastSMA.Last())
            {
                bool hasLong = this.tradingData.HasLongPosition(this.strategyHeader);
                bool hasShort = this.tradingData.HasShortPosition(this.strategyHeader);

                Signal signal;
                if(this.tradingData.UnfilledExists(this.strategyHeader))
                {
                    return;
                }
                else if (hasLong) 
                {
                    return;
                }
                else if (hasShort)
                {
                    signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, item.Price, 0, item.Price);
                    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал на закрытие короткой позиции {2}", DateTime.Now, this.GetType().Name, signal.ToString()));
                    this.signalQueue.Enqueue(signal);
                }      
                signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, item.Price, 0, item.Price);
                this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал на открытие длинной позиции {2}", DateTime.Now, this.GetType().Name, signal.ToString()));
                this.signalQueue.Enqueue(signal);
            }
            //надо шорт
            else if (slowSMA.Last() > fastSMA.Last())
            {
                bool hasLong = this.tradingData.HasLongPosition(this.strategyHeader);
                bool hasShort = this.tradingData.HasShortPosition(this.strategyHeader);

                Signal signal;
                if (this.tradingData.UnfilledExists(this.strategyHeader))
                {
                    return;
                }
                else if (hasShort)
                {
                    return;
                }
                else if (hasLong)
                {
                    signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, item.Price, 0, item.Price);
                    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал на закрытие длинной позиции {2}", DateTime.Now, this.GetType().Name, signal.ToString()));
                    this.signalQueue.Enqueue(signal);
                }
                signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, item.Price, 0, item.Price);
                this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал на открытие короткой позиции {2}", DateTime.Now, this.GetType().Name, signal.ToString()));
                this.signalQueue.Enqueue(signal);
            }
        }
    }
}
