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
    public class IndicatorLowest//: AddedItemHandler<Bar>
                               : DataOutput<double>
    {
        //TODO 3. Добавить в индикатор Имя и Номер
        public IDataInput<double> Input { get; private set; }
        //public IList<double> Value { get; private set; }

        //private StrategyHeader strategyHeader { get; set; }
        //private IDataContext tradingData { get; set; }
        //private ObservableQueue<Signal> signalQueue { get; set; }
        private ILogger logger { get; set; }
        
        /// <summary>
        /// скользящее среднее
        /// </summary>
        public IList<double> Lowest { get;} = new List<double>();

        /// <summary>
        /// значения для отправки, отрисовки
        /// скользящее среднее
        /// </summary>
        public IList<ValueDouble> ValueLowest { get; } = new List<ValueDouble>();

        /// <summary>
        /// список сторонних обработчиков Highest
        /// </summary>
        private IList<ItemAddedNotification<double>> HandlersLowest { get; set; }
           = new List<ItemAddedNotification<double>>();

        /// <summary>
        /// список сторонних обработчиков ValueHighest
        /// </summary>
        private IList<ItemAddedNotification<ValueDouble>> HandlersValueLowest { get; set; }
           = new List<ItemAddedNotification<ValueDouble>>();

        /// <summary>
        /// период
        /// </summary>
        public int Period { get; private set; }

        public IndicatorLowest(int period, IDataInput<double> dataInput, ILogger logger)
            :base(1)//Output
        {
            this.Period = period;
            this.Input = dataInput;
            this.logger = logger;
            this.Output[0] = this.Lowest;
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
            double iLowest = Indicator.Lowest_i(Input.Value.ToList<double>(), Period);
            //(Input.Value.ToList<double>(), Period, Highest.ToList<double>());

            Lowest.Add(iLowest);

            ///вызываем обработчики значений
            foreach (var handler in HandlersLowest)
            {
                handler.Invoke(Lowest.Last());
            }

            ///упаковка посчитанных значений
            ValueLowest.Add(new ValueDouble()
            {
                Id = id,
                //DateTime = item.DateTime,
                //TODO 4. сейчас отрисовывается по имени MaFast, надо переделать на стороне отрисовки
                Name = "Lowest",
                Value = iLowest
            });

            ///отправка посчитанных значений
            foreach (var handler in HandlersValueLowest)
            {
                handler.Invoke(ValueLowest.Last());
            }
        }

        /// <summary>
        /// добавить сторонний обработчик ValueHighest
        /// </summary>
        /// <param name="handler"></param>
        public void AddHandlerValueLowest(ItemAddedNotification<ValueDouble> handler)
        {
            this.HandlersValueLowest.Add(handler);
        }

        /// <summary>
        /// добавить сторонний обработчик Highest
        /// </summary>
        /// <param name="handler"></param>
        public void AddHandlerLowest(ItemAddedNotification<double> handler)
        {
            this.HandlersLowest.Add(handler);
        }
    }
}
