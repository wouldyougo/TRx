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
using TRL.Common.Events;
using TRx.Indicators;
using TRx.Helpers;
using TRL.Common.Extensions;

namespace TRx.Handlers
{
    /// <summary>
    /// Обработчик появления нового бара
    /// 
    /// Возможные изменения:
    /// </summary>
    public class SourcePriceOnBar : IDataOutput<double> //AddedItemHandler<Bar>
    {
        private StrategyHeader strategyHeader { get; set; }
        private IDataContext tradingData { get; set; }
        private ILogger logger { get; set; }
        
        private BarSettings bs { get; set; }

        //public Bar bar { get; private set; }

        public SourcePriceOnBar(StrategyHeader strategyHeader, IDataContext tradingData, ILogger logger)
            //: base(tradingData.Get<ObservableCollection<Bar>>())
        {
            this.strategyHeader = strategyHeader;
            this.tradingData = tradingData;
            //this.signalQueue = signalQueue;
            this.logger = logger;
        }

        /// <summary>
        /// Обработчик появления нового бара
        /// </summary>
        /// <param name="item">Bar</param>
        //public override void OnItemAdded(Bar item)
        public void OnItemAdded(Bar item)
        {
            //bar = item;
            if (item.Symbol != this.strategyHeader.Symbol)
                return;

            bs = this.tradingData.Get<IEnumerable<BarSettings>>().SingleOrDefault(s => s.StrategyId == this.strategyHeader.Id);
            if (bs == null) {
                throw new System.Exception("BarSettings bs == null");
                //return;
            }

            //barsSet <= bars
            //IEnumerable<Bar> bars = this.tradingData.Get<IEnumerable<Bar>>().GetNewestBars(bs.Symbol, bs.Interval);
            IEnumerable<Bar> barsSet = this.tradingData.Get<IEnumerable<Bar>>().GetNewestBars(bs.Symbol, bs.Interval, bs.Period + 1);
            //int barsCount = bars.Count();
            int barsCount = this.tradingData.Get<IEnumerable<Bar>>().GetNewestBars(bs.Symbol, bs.Interval).Count();

            if (barsSet == null || barsSet.Count() == 0)
                return;

            IEnumerable<double> close = from b in barsSet
                                                 select b.Close;

            IEnumerable<double> volume = from b in barsSet
                                              select b.Volume;

            Close = close.ToList<double>();
            Volume = volume.ToList<double>();
        }
        private IList<double> Close { get; set; }
        private IList<double> Volume { get; set; }

        /// <summary>
        /// Предоставляет доступ по индексу i к [i] списку IList<T> с данными
        /// </summary>
        /// <param name="index">индекс i для доступа к [i] списку IList<T> с данными</param>
        /// <returns>IList<T></returns>
        public IList<double> this[int index]   // indexer declaration
        {
            get
            {
                if (index == 0)
                {
                    return this.Close;
                }
                if (index == 1)
                {
                    return this.Volume;
                }
                else {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (index == 0)
                {
                    this.Close = value;
                }
                if (index == 1)
                {
                    this.Volume = value;
                }
                else {
                    throw new IndexOutOfRangeException();
                }
            }
        }

    }
}
