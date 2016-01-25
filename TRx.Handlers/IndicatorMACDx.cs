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
    /// Вычисляет среднюю за период
    /// Вычисляет отклонение источника от средней за период
    /// 
    /// Возможные изменения:
    /// Модифицировать генерацию события
    /// Модифицировать обработчик - добавить параметр конструктора тип скользящей
    /// </summary>
    public class IndicatorMACDx//: AddedItemHandler<Bar>
                               : IDataSource<double>
    {
        public IDataInput<double> Input { get; private set; }
        //public IList<double> Value { get; private set; }
        public IList<double> Source(int i = 0)
        {
            if (i == 0)
            {
                return this.Ma;
            }
            else if (i == 1)
            {
                return this.De;
            }
            else {
                //throw new NotImplementedException();
                return null;
            }
        }


        //private StrategyHeader strategyHeader { get; set; }
        //private IDataContext tradingData { get; set; }
        //private ObservableQueue<Signal> signalQueue { get; set; }
        private ILogger logger { get; set; }
        
        /// <summary>
        /// скользящее среднее
        /// </summary>
        public IList<double> Ma { get;} = new List<double>();
        /// <summary>
        /// отклонение цены от скользящей средней
        /// </summary>
        public IList<double> De { get; } = new List<double>();

        /// <summary>
        /// значения для отправки, отрисовки
        /// скользящее среднее
        /// </summary>
        public IList<ValueDouble> ValueMa { get; } = new List<ValueDouble>();
        /// <summary>
        /// значения для отправки, отрисовки
        /// отклонение цены от скользящей средней
        /// </summary>
        public IList<ValueDouble> ValueDe { get; } = new List<ValueDouble>();

        /// <summary>
        /// список сторонних обработчиков Ma
        /// </summary>
        private IList<ItemAddedNotification<double>> HandlersMa { get; set; }
           = new List<ItemAddedNotification<double>>();
        /// <summary>
        /// список сторонних обработчиков De
        /// </summary>
        private IList<ItemAddedNotification<double>> HandlersDe { get; set; } 
           = new List<ItemAddedNotification<double>>();

        /// <summary>
        /// список сторонних обработчиков ValueMa
        /// </summary>
        private IList<ItemAddedNotification<ValueDouble>> HandlersValueMa { get; set; } 
           = new List<ItemAddedNotification<ValueDouble>>();
        /// <summary>
        /// список сторонних обработчиков ValueDe
        /// </summary>
        private IList<ItemAddedNotification<ValueDouble>> HandlersValueDe { get; set; } 
           = new List<ItemAddedNotification<ValueDouble>>();

        //private BarSettings bs { get; set; }

        //public Bar bar { get; private set; }
        //private SMASettings ss { get; set; }
        /// <summary>
        /// период скользящей средней цены
        /// </summary>
        public double Period { get; private set; }
        //public IndicatorMACDx(StrategyHeader strategyHeader, IDataContext tradingData, double period, ILogger logger)
        //    //: base(tradingData.Get<ObservableCollection<Bar>>())
        //{
        //    this.strategyHeader = strategyHeader;
        //    this.tradingData = tradingData;
        //    //this.signalQueue = signalQueue;
        //    this.logger = logger;
        //    //ma period
        //    this.Period = period;

        //    this.HandlersMa = new List<ItemAddedNotification<double>>();
        //    this.HandlersDe = new List<ItemAddedNotification<double>>();

        //    this.HandlersValueMa = new List<ItemAddedNotification<ValueDouble>>();
        //    this.HandlersValueDe = new List<ItemAddedNotification<ValueDouble>>();

        //    this.Ma = new List<double>();
        //    this.De = new List<double>();

        //    this.ValueMa = new List<ValueDouble>();
        //    this.ValueDe = new List<ValueDouble>();
        //}

        public IndicatorMACDx(double period, IDataInput<double> dataInput, ILogger logger)
        {
            this.Period = period;
            this.Input = dataInput;
            this.logger = logger;
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
            //double iMa = Indicator.EMAi(closePrices.ToList<double>(), Period, Ma.ToList<double>());
            //double iDe = closePrices.Last() - iMa;
            double iMa = Indicator.EMAi(Input.Value.ToList<double>(), Period, Ma.ToList<double>());
            double iDe = Input.Value.Last() - iMa;


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
                Id = id,
                //DateTime = item.DateTime,
                Name = "Ma",
                Value = iMa
            });

            ValueDe.Add(new ValueDouble()
            {
                Id = id,
                //DateTime = item.DateTime,
                Name = "De",                //ToDo
                // сейчас отрисовывается по имени MaFast, надо переделать на стороне отрисовки
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
