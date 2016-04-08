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
    public class BarBuilderRangeBar: BarBuilder
    {
        /// <summary>
        /// Настройка бара, определяет:
        /// Код инструмента
        /// Тип бара
        /// Интервал
        /// в секундах - для времеонного интервала
        /// в единицах - для volume или range бара
        /// </summary>
        //public BarSettings BarSettings { get; private set; }

        //public Bar LastBar { get; private set; }

        public BarBuilderRangeBar(BarSettings barSettings)
            : base(barSettings)
        {
            //this.BarSettings = barSettings;
            //LastBar = new Bar();
            //LastBar.DateTime = DateTime.MinValue;
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

        /// <summary>
        ///     проверяем возможность добавления нового тика в текущий бар
        ///     если тело бара + расстояние до тика > значения интревала
        ///     true - интервал превышен
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="tick"></param>
        /// <returns></returns>
        public bool CheckRangeExcess(Bar bar, Tick tick)
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
        /// Завершает новый бар
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="tick"></param>
        /// <returns>Новый бар</returns>
        public override Bar FinishBarState(Bar bar)
        {
            bar.State = Enums.BarState.Finished;
            UpdateDateId(bar);
            return bar;
        }
    }
}
