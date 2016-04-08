using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Common.Models;
using TRL.Common.Extensions.Models;

namespace TRL.Common.Models
{
    /// <summary>
    /// Постритель баров
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
        //public Bar LastBar { get; private set; }

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

        public BarBuilderTimeBar(BarSettings barSettings)
            : base(barSettings)
        {
            TimeSpanInterval = new TimeSpan(0, 0, barSettings.Interval);
            //this.BarSettings = barSettings;
            //LastBar = new Bar();
            //LastBar.DateTime = DateTime.MinValue;
            BarList = NewBarList(DateTime.Now);
            
        }
        /// <summary>
        /// Сформировать список заготовок под бары
        /// на основе dateTime.Date
        /// </summary>
        /// <param name="dateTime">dateTime.Date</param>
        /// <returns>список заготовок под бары</returns>
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
        /// Установить значения OHLCV для существующего бара
        /// </summary>
        /// <param name="bar">текущий бар</param>
        /// <param name="ticks">набор тиков</param>
        /// <returns></returns>
        public static Bar SetBarOHLCV(Bar bar, IEnumerable<Tick> ticks)
        {
            double open = ticks.First().Price;
            double close = ticks.Last().Price;

            IEnumerable<Tick> orderedByPrice = TicksOrderByPrice(ticks);

            double high = orderedByPrice.Last().Price;
            double low =  orderedByPrice.First().Price;

            double volume = orderedByPrice.Sum(i => i.Volume);

            bar.Open = open;
            bar.High = high;
            bar.Low = low;
            bar.Close = close;
            bar.Volume = volume;
            return bar;
        }
        /// <summary>
        /// Установить значения OHLCV для существующего бара
        /// </summary>
        /// <param name="bar">текущий бар</param>
        /// <param name="source">бар источник OHLCV</param>
        /// <returns></returns>
        internal static Bar SetBarOHLCV(Bar bar, Bar source)
        {
            //bar.Open = source.Open;
            //bar.High = source.High;
            //bar.Low = source.Low;
            //bar.Close = source.Close;
            //bar.Volume = source.Volume;
            bar.Open =  source.Close;
            bar.High =  source.Close;
            bar.Low =   source.Close;
            bar.Close = source.Close;
            bar.Volume = 0;
            return bar;
            //throw new NotImplementedException();
        }

        public static IEnumerable<Tick> TicksOrderByPrice(IEnumerable<Tick> ticks)
        {
            return ticks.OrderBy(i => i.Price);
        }

        int recursion = 0;
        /// <summary>
        /// Получить заготовку нового бара
        /// </summary>
        /// <param name="tick">тик, время которого будет лежать в интервале нового бара</param>
        /// <returns>Заготовка нового бара</returns>
        public Bar GetBarTemplate(Tick tick)
        {
            recursion += 1;
            Bar bar = null;
            try
            {
                bar = BarList.First(t => t.Symbol == tick.Symbol &&
                                    t.DateTimeOpen <= tick.DateTime &&
                                    t.DateTime > tick.DateTime);
            }
            catch (System.InvalidOperationException e)
            {
                if (bar == null)
                {
                    if (recursion == 3) //throw new Exception("Не удалось создать нужный интервал");
                        return null;
                    // Если в текущем списка нет нужного значения
                    // Формируем новый список значений
                    BarList = NewBarList(tick.DateTime);
                    bar = GetBarTemplate(tick);
                }
            }//var bars = BarList.Select(t => t.Symbol == tick.Symbol && t.DateTime < tick.DateTime);

            // Удаляем заготовки под бары с начала списка по текущий бар включительно
            int Index = BarList.IndexOf(bar);
            for (int i = 0; i <= Index; i++)
                BarList.RemoveAt(0);
            recursion = 0;
            return bar;
        }   //throw new NotImplementedException();

        //public static IEnumerable<Tick> TicksOrderByDateTime(IEnumerable<Tick> ticks)
        //{
        //    return ticks.OrderBy(i => i.DateTime);
        //}
    }
}
