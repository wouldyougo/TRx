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
    public class BarBuilder
    {
        /// <summary>
        /// Настройка бара, определяет:
        /// Код инструмента
        /// Тип бара
        /// Интервал
        /// в секундах - для времеонного интервала
        /// в единицах - для volume или range бара
        /// </summary>
        public BarSettings BarSettings { get; private set; }

        public BarBuilder(BarSettings barSettings)
        {
            this.BarSettings = barSettings;
        }

        /// <summary>
        /// Проверить на Symbol != BarSettings.Symbol
        /// Вызвать исключение
        /// </summary>
        /// <param name="tick"></param>
        private void CheckSymbol(string Symbol)
        {
            if (BarSettings.Symbol != null)
                if (Symbol != BarSettings.Symbol)
                {
                    throw new System.Exception("Symbol != BarSettings.Symbol");
                }
        }

        private void CheckSymbol(string Symbol1, string Symbol2)
        {
            if (BarSettings.Symbol != null) {
                if (Symbol1 != BarSettings.Symbol)
                {
                    throw new System.Exception("Symbol != BarSettings.Symbol");
                }
            }
            if (Symbol1 != Symbol2)
            {
                throw new System.Exception("Symbol1 != Symbol2");
            }
        }

        /// <summary>
        /// Проверить на State == Enums.BarState.Finished
        /// Вызвать исключение
        /// </summary>
        /// <param name="tick"></param>
        private void CheckState(Enums.BarState State)
        {
            if (State == Enums.BarState.Finished)
            {
                throw new System.Exception("State == Enums.BarState.Finished");
            }
        }

        /// <summary>
        /// Создает новый бар на основе тика
        /// </summary>
        /// <param name="tick"></param>
        /// <returns>Новый бар</returns>
        public Bar CreateBar(Tick tick)
        {
            return this.CreateBar(tick, tick.DateTime);
        }

        /// <summary>
        /// Создает новый бар на основе тика
        /// </summary>
        /// <param name="tick"></param>
        /// <param name="barDate">DateTime начала нового бара</param>
        /// <returns>Новый бар</returns>
        public Bar CreateBar(Tick tick, DateTime barDate)
        {
            //if (tick.Symbol != BarSettings.Symbol)
            //{
            //    throw new System.Exception("tick.Symbol != BarSettings.Symbol");
            //}
            CheckSymbol(tick.Symbol);

            double open = tick.Price;
            double close = tick.Price;
            double high = tick.Price;
            double low = tick.Price;
            double volume = tick.Volume;
            double volumePrice = tick.Volume * tick.Price;

            return new Bar
            {
                Symbol = tick.Symbol,
                DateTime = barDate,
                Open = open,
                High = high,
                Low = low,
                Close = close,
                Volume = volume,
                VolumePrice = volumePrice,
                Interval = BarSettings.Interval,
                //State = Enums.BarState.Started
                State = Enums.BarState.Changed
            };
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Обновляет новый бар добавлением к бару нового тика
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="tick"></param>
        /// <returns>Новый бар</returns>
        public Bar UpdateBar(Bar bar, Tick tick)
        {
            CheckSymbol(bar.Symbol, tick.Symbol);
            CheckState(bar.State);

            if (tick.Price < bar.Low)
            {
                bar.Low = tick.Price;
            }
            else
            if (tick.Price > bar.High)
            {
                bar.High = tick.Price;
            }
            bar.Close    = tick.Price;
            bar.DateTime = tick.DateTime;
            bar.Volume  += tick.Volume;
            bar.VolumePrice += tick.Price * tick.Volume;
            bar.State = Enums.BarState.Changed;
            return bar;            
        }

        /// <summary>
        /// Завершает новый бар
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="tick"></param>
        /// <returns>Новый бар</returns>
        public Bar FinishBarState(Bar bar)
        {
            bar.State = Enums.BarState.Finished;
            return bar;
        }

        /// <summary>
        /// Проверить достигнут ли RangeBar интервал
        /// </summary>
        /// <param name="bar"></param>
        /// <returns></returns>      
        public bool CheckRangeReach(Bar bar)
        {
            bool finish = (bar.BarLengthHL() >= this.BarSettings.Interval);
            return finish;
        }

        //Bar bar2 = CreateBar(tick, tick.DateTime);
        //List<Bar> listBar = new List<Bar>();

        //listBar.Add(bar);
        //listBar.Add(bar2);

        //Bar bar3;
        //bar3 = MakeBar(listBar);
        //return bar3;
        //throw new NotImplementedException();



        ///// <summary>
        ///// Создает новый бар на основе нескольких тиков
        ///// </summary>
        ///// <param name="tick"></param>
        ///// <param name="barDate">DateTime начала нового бара</param>
        ///// <returns>Новый бар</returns>
        //public Bar CreateBar(IEnumerable<Tick> ticks, DateTime barDate)
        //{
        //    throw new NotImplementedException();

        //    double open = ticks.First().Price;
        //    double close = ticks.Last().Price;

        //    IEnumerable<Tick> orderedByPrice = OrderByPrice(ticks);

        //    double high = orderedByPrice.Last().Price;
        //    double low = orderedByPrice.First().Price;

        //    double volume = orderedByPrice.Sum(i => i.Volume);

        //    return new Bar { Symbol = ticks.ElementAt(0).Symbol, DateTime = barDate, Open = open, High = high, Low = low, Close = close, Volume = volume };
        //}

        ///// <summary>
        ///// Обновляет бар добавлением нового тика
        ///// </summary>
        ///// <param name="bar"></param>
        ///// <param name="tick"></param>
        ///// <returns>Новый бар</returns>
        //public Bar UpdateBar(Bar bar, Tick tick)
        //{
        //    Bar bar3 = JastUpdateBar(bar, tick);
        //    if (bar3.BarLengthHL() > BarSettings.Interval)
        //    {
        //        //bar3 = CreateBar(tick);
        //        bar3 = null;
        //    }
        //    return bar3;
        //    //throw new NotImplementedException();
        //}

        ///// <summary>
        ///// Обновляет бар добавлением нового тика или
        ///// Создает новый бар добавлением нового тика
        ///// </summary>
        ///// <param name="bar"></param>
        ///// <param name="tick"></param>
        ///// <returns>Новый бар</returns>
        //public Bar UpdateOrMakeBar(Bar bar, Tick tick)
        //{
        //    Bar bar3 = UpdateBar(bar, tick);
        //    if (bar3 == null)
        //    {
        //        bar3 = CreateBar(tick);
        //    }
        //    return bar3;

        //    //Bar bar3 = JastUpdateBar(bar, tick);
        //    //if (bar3.BarLengthHL() > BarSettings.Interval)
        //    //{
        //    //    bar3 = CreateBar(tick);
        //    //}
        //    //return bar3;
        //    //throw new NotImplementedException();
        //}


        //public static IEnumerable<Tick> OrderByPrice(IEnumerable<Tick> ticks)
        //{
        //    return ticks.OrderBy(i => i.Price);
        //}

        //public static IEnumerable<Tick> OrderByDateTime(IEnumerable<Tick> ticks)
        //{
        //    return ticks.OrderBy(i => i.DateTime);
        //    //throw new NotImplementedException();            
        //}
        //public IEnumerable<Bar> OrderByLow(IEnumerable<Bar> bars)
        //{
        //    return bars.OrderBy(i => i.Low);
        //}

        //public IEnumerable<Bar> OrderByHigh(IEnumerable<Bar> bars)
        //{
        //    return bars.OrderBy(i => i.High);
        //}

        //public static IEnumerable<Bar> OrderByDateTime(IEnumerable<Bar> bars)
        //{
        //    return bars.OrderBy(i => i.DateTime);
        //}

        /// <summary>
        /// Создает новый бар из нескольких
        /// </summary>
        /// <param name="bars"></param>
        /// <returns>Новый бар</returns>
        private Bar MakeBar(List<Bar> bars)
        {
            //throw new NotImplementedException();
            double open = bars.First().Open;
            double close = bars.Last().Close;

            List<Bar> orderedByHigh = bars.OrderBy(i => i.High).ToList<Bar>();
            double high = orderedByHigh.Last().High;

            List<Bar> orderedByLow = bars.OrderBy(i => i.Low).ToList<Bar>();
            double low = orderedByLow.First().Low;

            double volume = bars.Sum(i => i.Volume);

            return new Bar
            {
                Symbol = bars.First().Symbol,
                DateTime = bars.First().DateTime,
                Open = open,
                High = high,
                Low = low,
                Close = close,
                Volume = volume,
                Interval = BarSettings.Interval
            };
        }
        /// <summary>
        /// Создает новый бар на основе price
        /// </summary>
        /// <param name="price"></param>
        /// <param name="barDate">DateTime начала нового бара</param>
        /// <returns>Новый бар</returns>
        internal Bar CreateBar(double price, DateTime dateTime, string symbol)
        {
            //if (tick.Symbol != BarSettings.Symbol)
            //{
            //    throw new System.Exception("tick.Symbol != BarSettings.Symbol");
            //}
            CheckSymbol(symbol);

            double open = price;
            double close = price;
            double high = price;
            double low = price;
            double volume = 0;
            double volumePrice = 0;

            return new Bar
            {
                Symbol = symbol,
                DateTime = dateTime,
                Open = open,
                High = high,
                Low = low,
                Close = close,
                Volume = volume,
                VolumePrice = volumePrice,
                Interval = BarSettings.Interval,
                //State = Enums.BarState.Started
                State = Enums.BarState.Changed
            };
            //throw new NotImplementedException();
        }

        /// <summary>
        ///     проверяем возможность добавления нового тика в текущий бар
        ///     если тело бара + расстояние до тика > значения интревала
        ///     true - интервал превышен
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="tick"></param>
        /// <returns></returns>
        internal bool CheckRangeExcess(Bar bar, Tick tick)
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
        /// <summary>
        /// Обновляет новый бар интервалом после поступления тика
        /// цена закрытия = цене открытия + интервал
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="tick"></param>
        /// <returns>Новый бар</returns> 
        internal void UpdateBarFinish(Bar bar, Tick tick)
        {
            CheckSymbol(bar.Symbol, tick.Symbol);
            CheckState(bar.State);

            if (tick.Price < bar.Low)
            {
                //bar.Low = tick.Price;
                bar.Low = bar.High - BarSettings.Interval;
                bar.Close = bar.Low;
            }
            else
            if (tick.Price > bar.High)
            {
                //bar.High = tick.Price;
                bar.High = bar.Low + BarSettings.Interval;
                bar.Close = bar.High;
            }
            bar.DateTime = tick.DateTime;
            //bar.Volume += tick.Volume;
            //bar.VolumePrice += tick.Price * tick.Volume;
            //bar.State = Enums.BarState.Changed;
            bar.State = Enums.BarState.Finished;
            //return bar;
            //throw new NotImplementedException();
        }
    }
}
