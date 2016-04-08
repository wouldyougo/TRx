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
    // П1
    // Проверяем попадание Тика в текущий бар           
    //// У10
    //// Если Тик попадает в текущий бар
    ////// Выходим    
    //// У11
    //// Есди Тик выходит за текущий бар
    ////// Завершаем текщий бар
    ////// Запрашиваем новый текущий бар у барбилдера
    ////// Выходим    

    // П2
    //// Исключение
    //// Если текущий бар отсутсвует
    ////// Запрашиваем новый текущий бар у барбилдера
    ////// Выходим    

    /// <summary>
    /// делаем из тиков бары
    /// </summary>
    public class MakeTimeBarsOnTick:AddedItemHandler<Tick>
    {
        private BarSettings barSettings;
        private IDataContext tradingData;
        private ILogger logger;

        private BarBuilderTimeBar barBuilder;
        private Bar barCurrent;
        private Bar barPrevious = new Bar();

        //public MakeTimeBarsOnTick(BarSettings barSettings, ITimeTrackable timeTracker, IDataContext tradingData, ILogger logger)
        public MakeTimeBarsOnTick(BarSettings barSettings, IDataContext tradingData, ILogger logger)
            : base(tradingData.Get<ObservableCollection<Tick>>())
        {
            this.tradingData = tradingData;
            this.barSettings = barSettings;
            this.logger = logger;
            //подменяем тип бара на TimeBar
            if (this.barSettings.BarType != Enums.DataModelType.TimeBar)
                this.barSettings.BarType  = Enums.DataModelType.TimeBar;
            this.barBuilder = new BarBuilderTimeBar(barSettings);
            // по умолчанию должен быть null
            //barCurrent = new Bar();
        }

        public override void OnItemAdded(Tick tick)
        {
            if (barSettings.Symbol != tick.Symbol)
                return;
            // П1
            // Проверяем попадание Тика в текущий бар           
            try
            {   //// У10
                //// Если Тик попадает в текущий бар
                ////// Выходим    
                //if ((barCurrent.DateTime.AddSeconds(-barSettings.Interval) < item.DateTime)&&
                if ((barCurrent.DateTimeOpen < tick.DateTime) &&
                    (barCurrent.DateTime     > tick.DateTime))
                    return;
                if (barCurrent.DateTimeOpen > tick.DateTime) {
                    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fffffff}, {1}, Ошибка. тик перед баром {2}", DateTime.Now, this.GetType().Name, barCurrent.ToString()));
                    return;
                }
            }
            catch (System.NullReferenceException e)
            {   // П2
                //// Исключение
                //// Если текущий бар отсутсвует
                ////// Запрашиваем новый текущий бар у барбилдера
                ////// Выходим    
                if (barCurrent == null)
                {
                    //state = "Created";
                    barCurrent = barBuilder.GetBarTemplate(tick);
                    return;
                }
                //throw;
                throw e;
            }
            //// У11
            //// Есди Тик выходит за текущий бар
            ////// Завершаем текщий бар
            IEnumerable<Tick> barTicks = GetTicksInRangeOf(tick, barCurrent.DateTimeOpen, barCurrent.DateTime);
            ////// TODO вопрос как правильно поступать если не было тиков в диапазоне?
            //////if (barTicks == null)
            /////    return;
            if (barTicks.Count() == 0)
            {
                barCurrent = BarBuilderTimeBar.SetBarOHLCV(barCurrent, barPrevious);
                //return;
            }
            else {
                barCurrent = BarBuilderTimeBar.SetBarOHLCV(barCurrent, barTicks);
            }
            barBuilder.FinishBarState(barCurrent);
            ////// state = "Finished";
            ////// сгенерировать событие на "закончен" бара
            ////// добавить завершенный бар в контекст                     
            if (TradingDataBarExists(barCurrent.DateTime) != true)
            {
                this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fffffff}, {1}, добавлен новый бар {2}", DateTime.Now, this.GetType().Name, barCurrent.ToString()));
                this.tradingData.Get<ObservableCollection<Bar>>().Add(barCurrent);
            }
            else {
                this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fffffff}, {1}, Ошибка. Не добавлен бар {2}", DateTime.Now, this.GetType().Name, barCurrent.ToString()));
                //throw new Exception(String.Format("{0:dd/MM/yyyy H:mm:ss.fffffff}, {1}, Exception новый бар {2}", DateTime.Now, this.GetType().Name, barCurrent.ToString()));
            }
            ////// Запрашиваем новый текущий бар у барбилдера
            barCurrent = barBuilder.GetBarTemplate(tick);
            return;
            ////// Выходим
        }

        private bool TradingDataBarExists(DateTime date)
        {
            return this.tradingData.Get<IEnumerable<Bar>>().Any(b => b.Symbol == this.barSettings.Symbol
                && b.Interval == this.barSettings.Interval
                && b.DateTime == date);
            //&& b.DateTime.Year == date.Year
            //&& b.DateTime.Month == date.Month
            //&& b.DateTime.Day == date.Day
            //&& b.DateTime.Hour == date.Hour
            //&& b.DateTime.Minute == date.Minute
            //&& b.DateTime.Second == date.Second
            //&& b.DateTime.Millisecond == date.Millisecond);
        }

        private IEnumerable<Tick> GetTicksInRangeOf(Tick item, DateTime begin, DateTime end)
        {
            //try
            {
                return this.tradingData.Get<IEnumerable<Tick>>().Where(t => t.Symbol == item.Symbol && t.DateTime >= begin && t.DateTime < end);
            }
            //catch
            {
                //throw;
                //return null;
            }
        }
    }
}
