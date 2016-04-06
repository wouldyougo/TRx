using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Common.Models;
using TRL.Common.Extensions.Models;

namespace TRL.Common.Models
{
    //сущность:
    //  включает сущности:
    //  реализует методы:

    //сущность:
    //  фабрика баров
    //  включает сущности:
    //      тик
    //      бар
    //      настройки баров (тип бара, параметр для формирования бара)

    //  фабрика баров
    //  реализует методы:
    //      создает новый бар добавлением нового тика
    //      обновляет существующий бар добавлением нового тика

    /// <summary>
    /// Постритель баров
    /// (создает новый бар добавлением нового тика
    ///  обновляет существующий бар добавлением нового тика)
    /// </summary>
    public class BarBuilderTimeBar : BarBuilder
    {
        ///// <summary>
        ///// Настройка бара, определяет:
        ///// Код инструмента
        ///// Тип бара
        ///// Интервал
        ///// в секундах - для времеонного интервала
        ///// в единицах - для volume или range бара
        ///// </summary>
        //public BarSettings BarSettings { get; private set; }

        /// <summary>
        /// Заготовки для баров под заданный таймфрейм
        /// </summary>
        public IList<Bar> BarList { get; private set; }

        /// <summary>
        /// StartDateTime
        /// </summary>
        public DateTime DateTimeStart { get; private set; }

        public DateTime DateTimeFinish { get; private set; }
        
        public System.TimeSpan TimeSpanInterval { get; private set; }


        //public Bar LastBar { get; private set; }

        public BarBuilderTimeBar(BarSettings barSettings)
            : base(barSettings)
        {
            TimeSpanInterval = new TimeSpan(0, 0, barSettings.Interval);
            //this.BarSettings = barSettings;
            //LastBar = new Bar();
            //LastBar.DateTime = DateTime.MinValue;
            BarList = NewBarList(DateTime.Now);
            
        }

        private List<Bar> NewBarList(DateTime dateTime)
        {
            //Вычисляем колчискво интервалов для секунд и минут
            //из DateTimeStart берем Time для для секунд и минут (и часов?)
            //из dateTime      берем Date
            DateTimeStart = dateTime.Date + BarSettings.DateTimeStart.TimeOfDay;
            DateTimeFinish = DateTimeStart.Date.AddDays(1);
            var dt = DateTimeFinish - DateTimeStart;
            int ic = (int)Math.Ceiling(dt.TotalSeconds / TimeSpanInterval.TotalSeconds);
            var res = new List<Bar>(ic);
            DateTime dateO = DateTimeStart;
            DateTime dateC = DateTimeStart + TimeSpanInterval;
            for (int i = 0; i < ic; i++) {
                res.Add(new Bar(BarSettings.Symbol, dateO, dateC));
                dateO += TimeSpanInterval;
                dateC += TimeSpanInterval;
            }
            return res;
        }

        /// <summary>
        /// Проверить достигнут ли RangeBar интервал
        /// </summary>
        /// <param name="bar"></param>
        /// <returns></returns>      
        public bool CheckTimeReach(Bar bar)
        {
            bool finish = (bar.BarLengthHL() >= this.BarSettings.Interval);
            return finish;
        }
        /// <summary>
        ///     проверяем возможность добавления нового тика в текущий бар
        ///     если тело бара + расстояние до тика > значения интревала
        ///     true - интервал превышен
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="tick"></param>
        /// <returns></returns>
        public bool CheckTimeExcess(Bar bar, Tick tick)
        {
            double step = 0;
            if (tick.Price < bar.Low)
            {
                step = bar.Low - tick.Price;
            }
            else
            if (tick.Price > bar.High)
            {
                step = tick.Price - bar.High;
            }
            bool finish = (bar.BarLengthHL() + step > this.BarSettings.Interval);
            return finish;

            //throw new NotImplementedException();
        }
    }
}
