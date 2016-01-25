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
    public class SourcePriceOnBar : IDataSource<double> //AddedItemHandler<Bar>
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

            IEnumerable<double> closePrices = from b in barsSet
                                                 select b.Close;
            Source = closePrices.ToList<double>();
        }
        private IList<double> Source { get; set; }

        IList<double> IDataSource<double>.Source(int i = 0)
        {
            if (i == 0)
            {
                return this.Source;
            }
            else {
                return null;
            }
        }
    }
}
