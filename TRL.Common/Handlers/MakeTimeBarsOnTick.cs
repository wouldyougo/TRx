using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using System.Collections.Specialized;
using TRL.Common.TimeHelpers;
using TRL.Common.Collections;
using TRL.Logging;

namespace TRL.Common.Handlers
{
    // <summary>
    // делаем из тиков рейндж бары:
    // первый вариант:
    // обрабатываем поступление нового тика:
    // поступивший тик добавляем в текущий бар
    //     если текущий бар отсутсвует или
    //     если текущий бар в состоянии "закончен"
    //     создаем новый текущий бар
    //         сгенерировать событие на "создание" бара
    //     добавить новый текущий бар в контекст
    //     проверяем завершение бара новым тиком
    //         если текущий бар готов т.е. может быть закончен
    //             переводим текущий бар в состояние "закончен"
    //             сгенерировать событие на "закончен" бара
    //     выходим из процесса обработки
    //
    // в этом случае интервал завершенного бара >= лимиту рейндж
    // после завершения бара новый бар не создается до поступления нового тика
    //             
    // вариант без проскальзывания:
    // обрабатываем поступление нового тика
    // ц0
    // п0 если текущий бар в состоянии "закончен"
    //         создаем новый текущий бар
    //         цена открытия = цене закрытия предыдущего бара
    //         переводим текущий бар в состоянии "обновлен"
    //         переходим в п1
    // п1 если текущий бар в состоянии "обновлен"
    //     проверяем возможность добавления нового тика в текущий бар
    //     если тело бара + расстояние до тика > значения интревала
    //     если добавить нельзя
    //         переводим текущий бар в состояние "закончен"
    //         цена закрытия = цене открытия + интервал
    //             сгенерировать событие на "закончен" бара
    //         добавить "законченый" бар в контекст
    //         переходим в п0
    //         continue;
    //     если добавить можно        
    //     если значения интревала >= тело бара + расстояние до тика
    //         поступивший тик добавляем в текущий бар
    //         если значения интревала >  тело бара + расстояние до тика
    //         сгенерировать событие бара "обновлен"
    //         если значения интревала == тело бара + расстояние до тика
    //         сгенерировать событие бара "закончен"
    //         выходим из процесса обработки
    //         break;
    // п2 если текущий бар отсутсвует
    //     создаем новый текущий бар
    //         поступивший тик добавляем в текущий бар
    //         цена открытия = цене поступивщего тика
    //         переводим текущий бар в состоянии "обновлен"
    //         выходим из процесса обработки
    //         break;
    // в этом случае лимит у рейнджа >= рейнджа завершенного бара
    // //новый бар завершается сразу после достижения интервала бара
    // новый бар завершается сразу после превышения интервала бара
    // добавить параметр проскальзывание для Range?
    // </summary>

    /// <summary>
    /// делаем из тиков рейндж бары
    /// </summary>
    public class MakeTimeBarsOnTick:AddedItemHandler<Tick>
    {
        private IDataContext tradingData;
        private BarSettings barSettings;
        private ITimeTrackable timeTracker;
        private ILogger logger;
        private BarBuilderTimeBar barBuilder;
        private Bar barCurrent;       

        public MakeTimeBarsOnTick(BarSettings barSettings, ITimeTrackable timeTracker, IDataContext tradingData, ILogger logger)
            : base(tradingData.Get<ObservableCollection<Tick>>())
        {
            this.tradingData = tradingData;
            this.barSettings = barSettings;
            this.timeTracker = timeTracker;
            this.logger = logger;
            //подменяем тип бара на RangeBar
            if (this.barSettings.BarType != Enums.DataModelType.TimeBar)
                this.barSettings.BarType  = Enums.DataModelType.TimeBar;
            this.barBuilder = new BarBuilderTimeBar(barSettings);
            barCurrent = new Bar();
        }

        /// <summary>
        /// обрабатываем поступление нового тика
        /// ц0
        /// п0 если текущий бар в состоянии "закончен"
        ///         создаем новый текущий бар
        ///         цена открытия = цене нового тика
        ///         переводим текущий бар в состоянии "обновлен"
        ///         переходим в п1
        /// п1 если текущий бар в состоянии "обновлен"
        ///     проверяем возможность добавления нового тика в текущий бар
        ///     если время тика > времени окончания текущего бара
        ///     если добавить нельзя
        ///         переводим текущий бар в состояние "закончен"
        ///             сгенерировать событие на "закончен" бара
        ///         добавить "законченый" бар в контекст
        ///         переходим в п0
        ///         continue;
        ///     если добавить можно        
        ///     если времени окончания текущего бара > время тика  
        ///         поступивший тик добавляем в текущий бар
        ///         сгенерировать событие бара "обновлен"
        ///         выходим из процесса обработки
        ///         break;
        /// п2 если текущий бар отсутсвует
        ///     создаем новый текущий бар
        ///         поступивший тик добавляем в текущий бар
        ///         цена открытия = цене поступивщего тика
        ///         переводим текущий бар в состоянии "обновлен"
        ///         выходим из процесса обработки
        ///         break;
        /// новый бар завершается сразу после превышения времени завершения текущего бара
        /// </summary>
        /// <param name="tick"></param>
        public void OnItemAdded2(Tick tick)
        {
            /// обрабатываем поступление нового тика:
            /// валидация Symbol тика
            if (barSettings.Symbol != tick.Symbol)
            {
                if (tick.Symbol == "")
                {
                    tick.Symbol = barSettings.Symbol;
                }
                else
                {
                    return;
                }
            }

            /// внутренние состояния бара в процессе обработки тика
            //String state;
            while (true)
            {
                /// п2 если текущий бар отсутсвует
                if (barCurrent == null)
                {
                    //state = "Created";
                    ///     создаем новый текущий бар
                    ///         поступивший тик добавляем в текущий бар
                    ///         цена открытия = цене поступивщего тика
                    barCurrent = barBuilder.CreateBar(tick, tick.DateTime);
                    ///         переводим текущий бар в состоянии "обновлен"
                    ///         выходим из процесса обработки
                    break;
                }
                /// п0 если текущий бар в состоянии "закончен"
                if (barCurrent.State == Enums.BarState.Finished)
                {
                    //state = "Finished";
                    ///         создаем новый текущий бар
                    ///         цена открытия = цене закрытия предыдущего бара
                    barCurrent = barBuilder.CreateBar(barCurrent.Close, tick.DateTime, tick.Symbol);
                    //state = "Updated";
                    ///         переводим текущий бар в состоянии "обновлен"
                    ///         переходим в п1
                }
                /// п1 если текущий бар в состоянии "обновлен"
                if (barCurrent.State == Enums.BarState.Changed)
                {
                    //state = "Updated";
                    ///     проверяем возможность добавления нового тика в текущий бар
                    ///     если тело бара + расстояние до тика > значения интревала
                    ///     если добавить нельзя - интервал превышен
                    if (barBuilder.CheckTimeExcess(barCurrent, tick))
                    {
                        ///         цена закрытия = цене открытия + интервал
                        ///         переводим текущий бар в состояние "закончен"
                        barBuilder.UpdateBarFinish(barCurrent, tick);
                        //state = "Finished";
                        ///         добавить "законченый" бар в контекст
                        this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fffffff}, {1}, добавлен новый бар {2}", DateTime.Now, this.GetType().Name, barCurrent.ToString()));
                        //this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fffffff}, {1}, добавлен новый бар {2}", barCurrent.DateTime, this.GetType().Name, barCurrent.ToString()));
                        this.tradingData.Get<ObservableCollection<Bar>>().Add(barCurrent);
                        ///         переходим в п0
                        continue;
                    }
                    ///     если добавить можно - интервал не превышен
                    ///     если значения интревала >= тело бара + расстояние до тика
                    else
                    {
                        ///         поступивший тик добавляем в текущий бар
                        barCurrent = barBuilder.UpdateBar(barCurrent, tick);
                        //state = "Updated";
                        ///         сгенерировать событие бара "обновлен"
                        ///         выходим из процесса обработки
                        //break;
                    }
                    ///     проверяем завершение бара новым тиком
                    if (barBuilder.CheckTimeReach(barCurrent))
                    {
                        ///         если текущий бар готов т.е. может быть закончен
                        ///             переводим текущий бар в состояние "закончен"
                        barBuilder.FinishBarState(barCurrent);
                        //state = "Finished";
                        ///             сгенерировать событие на "закончен" бара
                        ///             добавить завершенный бар в контекст                    
                        this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fffffff}, {1}, добавлен новый бар {2}", DateTime.Now, this.GetType().Name, barCurrent.ToString()));
                        //this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fffffff}, {1}, добавлен новый бар {2}", barCurrent.DateTime, this.GetType().Name, barCurrent.ToString()));
                        this.tradingData.Get<ObservableCollection<Bar>>().Add(barCurrent);
                    }
                    ///         выходим из процесса обработки
                    break;
                }
            }
        }

        public override void OnItemAdded(Tick item)
        {
            //{
            //    if (((FakeTimeTracker2)timeTracker).stop > item.DateTime)
            //    {
            //        ((FakeTimeTracker2)timeTracker).start = item.DateTime.Date.AddHours(9);
            //        ((FakeTimeTracker2)timeTracker).stop = item.DateTime;
            //    }
            //    var d3 = item.DateTime - ((FakeTimeTracker2)timeTracker).stop;
            //    ((FakeTimeTracker2)timeTracker).IncrementStopDate(d3.Seconds);
            //}

            if (barSettings.Symbol != item.Symbol)
                return;

            //
            if (barCurrent.DateTime.AddSeconds(barSettings.Interval) > item.DateTime)
                return;

            DateTime end = GetPeriodEndDate();

            if (this.tradingData.Get<IEnumerable<Tick>>().LastOrDefault(t => t.Symbol == item.Symbol).DateTime < end)
                return;

            if (BarExists(end))
                return;

            if (end > this.timeTracker.StartAt + this.timeTracker.Duration)
                return;

            DateTime begin = end.AddSeconds(-this.barSettings.Interval);

            IEnumerable<Tick> barTicks = GetTicksInRangeOf(item, begin, end);

            if (barTicks == null)
                return;

            if (barTicks.Count() == 0)
                return;

            Bar fresh = BarsFactory.MakeBar(barTicks, end);
            fresh.Interval = this.barSettings.Interval;

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, добавлен новый бар {2}", DateTime.Now, this.GetType().Name, fresh.ToString()));

            this.tradingData.Get<ObservableCollection<Bar>>().Add(fresh);
        }
        private bool BarExists(DateTime date)
        {
            return this.tradingData.Get<IEnumerable<Bar>>().Any(b => b.Symbol == this.barSettings.Symbol
                && b.DateTime.Year == date.Year
                && b.DateTime.Month == date.Month
                && b.DateTime.Day == date.Day
                && b.DateTime.Hour == date.Hour
                && b.DateTime.Minute == date.Minute
                && b.DateTime.Second == date.Second
                && b.DateTime.Millisecond == date.Millisecond);
        }
        private IEnumerable<Tick> GetTicksInRangeOf(Tick item, DateTime begin, DateTime end)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<Tick>>().Where(t => t.Symbol == item.Symbol && t.DateTime >= begin && t.DateTime < end);
            }
            catch
            {
                return null;
            }
        }
        private DateTime GetPeriodEndDate()
        {
            DateTime p = this.timeTracker.StartAt.AddSeconds(this.barSettings.Interval);

            DateTime current = this.timeTracker.StartAt + this.timeTracker.Duration;

            if (current < p)
                return p.RoundDownToNearestMinutes(this.barSettings.Interval / 60);

            return current.RoundDownToNearestMinutes(this.barSettings.Interval / 60);
        }
    }
}
