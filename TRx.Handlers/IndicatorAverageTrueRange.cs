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
    /// Обработчик появления новых данных
    /// Вычисляет максимум за период
    /// Вычисляет отклонение источника от средней за период
    /// </summary>
    public class IndicatorAverageAverageTrueRange//: AddedItemHandler<Bar>
                               : DataOutput<double>
    {
        //TODO 3. Добавить в индикатор Имя и Номер
        public IDataInput<Bar> Input { get; private set; }
        //public IList<double> Value { get; private set; }

        //private StrategyHeader strategyHeader { get; set; }
        //private IDataContext tradingData { get; set; }

        private ILogger logger { get; set; }
        
        /// <summary>
        /// скользящее среднее
        /// </summary>
        public IList<double> AverageTrueRange { get;} = new List<double>();

        /// <summary>
        /// значения для отправки, отрисовки
        /// скользящее среднее
        /// </summary>
        public IList<ValueDouble> ValueAverageTrueRange { get; } = new List<ValueDouble>();

        /// <summary>
        /// список сторонних обработчиков AverageTrueRange
        /// </summary>
        private IList<ItemAddedNotification<double>> HandlersAverageTrueRange { get; set; }
           = new List<ItemAddedNotification<double>>();

        /// <summary>
        /// список сторонних обработчиков ValueAverageTrueRange
        /// </summary>
        private IList<ItemAddedNotification<ValueDouble>> HandlersValueAverageTrueRange { get; set; }
           = new List<ItemAddedNotification<ValueDouble>>();

        /// <summary>
        /// период
        /// </summary>
        //public int Period { get; private set; }

        public IndicatorAverageAverageTrueRange(int period, IDataInput<Bar> dataInput, ILogger logger)
            :base(1)//Output
        {
            //this.Period = period;
            this.Input = dataInput;
            this.logger = logger;
            this.Output[0] = this.AverageTrueRange;
        }

        /// <summary>
        /// Обработчик появления новых данных
        /// Вычисляет среднюю за период
        /// Вычисляет отклонение источника от средней за период
        /// </summary>
        ///// <param name="item">Bar</param>
        //public override void OnEvent(long id)
        public void Do(long id)
        {
            ///вычисляем новые занчения
            ///Input
            var input = Input.Value.ToList<Bar>();
            double iAverageTrueRange = TRx.Indicators.BarSource.Indicator.AverageTrueRange_i(
                input, input.Count-1);

            AverageTrueRange.Add(iAverageTrueRange);

            ///вызываем обработчики значений
            foreach (var handler in HandlersAverageTrueRange)
            {
                handler.Invoke(AverageTrueRange.Last());
            }

            ///упаковка посчитанных значений
            ValueAverageTrueRange.Add(new ValueDouble()
            {
                Id = id,
                //DateTime = item.DateTime,
                //TODO 4. сейчас отрисовывается по имени MaFast, надо переделать на стороне отрисовки
                Name = "AverageTrueRange",
                Value = iAverageTrueRange
            });

            ///отправка посчитанных значений
            foreach (var handler in HandlersValueAverageTrueRange)
            {
                handler.Invoke(ValueAverageTrueRange.Last());
            }
        }

        /// <summary>
        /// добавить сторонний обработчик ValueAverageTrueRange
        /// </summary>
        /// <param name="handler"></param>
        public void AddHandlerValueAverageTrueRange(ItemAddedNotification<ValueDouble> handler)
        {
            this.HandlersValueAverageTrueRange.Add(handler);
        }

        /// <summary>
        /// добавить сторонний обработчик AverageTrueRange
        /// </summary>
        /// <param name="handler"></param>
        public void AddHandlerAverageTrueRange(ItemAddedNotification<double> handler)
        {
            this.HandlersAverageTrueRange.Add(handler);
        }
    }
}
