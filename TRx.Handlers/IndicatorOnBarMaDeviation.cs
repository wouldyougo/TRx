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
    /// Вычисляет отклонение цены (закрытия) от средней за период
    /// 
    /// Возможные изменения:
    /// Модифицировать обработчик - добавить параметр конструктора тип скользящей
    /// Модифицировать обработчик - добавить параметр конструктора тип цены
    /// Модифицировать обработчик - добавить входы - зависимости вычислений
    /// </summary>
    public class IndicatorOnBarMaDeviation: AddedItemHandler<Bar>
    {
        private StrategyHeader strategyHeader { get; set; }
        private IDataContext tradingData { get; set; }
        private ObservableQueue<Signal> signalQueue { get; set; }
        private ILogger logger { get; set; }
        
        /// <summary>
        /// скользящее среднее
        /// </summary>
        public IList<double> Ma { get; set; }
        /// <summary>
        /// отклонение цены от скользящей средней
        /// </summary>
        public IList<double> De { get; set; }

        /// <summary>
        /// значения для отправки, отрисовки
        /// скользящее среднее
        /// </summary>
        public IList<ValueDouble> ValueMa { get; set; }
        /// <summary>
        /// значения для отправки, отрисовки
        /// отклонение цены от скользящей средней
        /// </summary>
        public IList<ValueDouble> ValueDe { get; set; }

        /// <summary>
        /// список сторонних обработчиков Ma
        /// </summary>
        private IList<ItemAddedNotification<double>> HandlersMa { get; set; }
        /// <summary>
        /// список сторонних обработчиков De
        /// </summary>
        private IList<ItemAddedNotification<double>> HandlersDe { get; set; }

        /// <summary>
        /// список сторонних обработчиков ValueMa
        /// </summary>
        private IList<ItemAddedNotification<ValueDouble>> HandlersValueMa { get; set; }
        /// <summary>
        /// список сторонних обработчиков ValueDe
        /// </summary>
        private IList<ItemAddedNotification<ValueDouble>> HandlersValueDe { get; set; }

        private BarSettings bs { get; set; }

        public Bar bar { get; private set; }
        //private SMASettings ss { get; set; }
        /// <summary>
        /// период скользящей средней цены
        /// </summary>
        private double Period { get; set; }
        public IndicatorOnBarMaDeviation(StrategyHeader strategyHeader, IDataContext tradingData, double period, ILogger logger)
            : base(tradingData.Get<ObservableCollection<Bar>>())
        {
            this.strategyHeader = strategyHeader;
            this.tradingData = tradingData;
            //this.signalQueue = signalQueue;
            this.logger = logger;
            //ma period
            this.Period = period;

            this.HandlersMa = new List<ItemAddedNotification<double>>();
            this.HandlersDe = new List<ItemAddedNotification<double>>();

            this.HandlersValueMa = new List<ItemAddedNotification<ValueDouble>>();
            this.HandlersValueDe = new List<ItemAddedNotification<ValueDouble>>();

            this.Ma = new List<double>();
            this.De = new List<double>();

            this.ValueMa = new List<ValueDouble>();
            this.ValueDe = new List<ValueDouble>();
        }

        /// <summary>
        /// Обработчик появления нового бара
        /// Вычисляет отклонение цены (закрытия) от средней за период
        /// </summary>
        /// <param name="item">Bar</param>
        public override void OnItemAdded(Bar item)
        {
            bar = item;
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

            ///вычисляем новые занчения
            double iMa = Indicator.EMA_i(closePrices.ToList<double>(), Period, Ma.ToList<double>());
            double iDe = closePrices.Last() - iMa;

            Ma.Add(iMa);
            De.Add(iDe);

            ///вызываем обработчики значений
            foreach (var handler in HandlersMa)
            {
                handler.Invoke(Ma.Last());
            }

            foreach (var handler in HandlersDe)
            {
                handler.Invoke(De.Last());
            }

            ///упаковка посчитанных значений
            ValueMa.Add(new ValueDouble()
            {
                Id = barsCount - 1,
                DateTime = item.DateTime,
                Name = "MaSlow",
                Value = iMa
            });

            ValueDe.Add(new ValueDouble()
            {   
                Id = barsCount - 1,
                DateTime = item.DateTime,
                Name = "MaFast",
                //TODO 4. сейчас отрисовывается по имени MaFast, надо переделать на стороне отрисовки
                //Name = "MaDeviation",
                Value = iDe
            });
            
            ///отправка посчитанных значений
            foreach (var handler in HandlersValueMa)
            {
                handler.Invoke(ValueMa.Last());
            }

            foreach (var handler in HandlersValueDe)
            {
                handler.Invoke(ValueDe.Last());
            }
        }

        /// <summary>
        /// добавить сторонний обработчик ValueMa
        /// </summary>
        /// <param name="handler"></param>
        public void AddHandlerValueMa(ItemAddedNotification<ValueDouble> handler)
        {
            this.HandlersValueMa.Add(handler);
        }
        /// <summary>
        /// добавить сторонний обработчик ValueDe
        /// </summary>
        /// <param name="handler"></param>
        public void AddHandlerValueDe(ItemAddedNotification<ValueDouble> handler)
        {
            this.HandlersValueDe.Add(handler);
        }

        /// <summary>
        /// добавить сторонний обработчик Ma
        /// </summary>
        /// <param name="handler"></param>
        public void AddHandlerMa(ItemAddedNotification<double> handler)
        {
            this.HandlersMa.Add(handler);
        }
        /// <summary>
        /// добавить сторонний обработчик De
        /// </summary>
        /// <param name="handler"></param>
        public void AddHandlerDe(ItemAddedNotification<double> handler)
        {
            this.HandlersDe.Add(handler);
        }
    }
}
