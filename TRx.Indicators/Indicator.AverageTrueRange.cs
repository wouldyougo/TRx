using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Common.Models;

//namespace TSLab.Script.Helpers
namespace TRx.Indicators
{
    public static partial class Indicator
	{
        /// <summary>
        /// Вычисляет значение TrueRange для свечки из набора.
        /// TrueRange is defined as the maximum of the following:
        ///   High - Low
        ///   ABS(High - PreviousClose)
        ///   ABS(Low  - PreviousClose)
        /// </summary>
        /// <param name="bars">Набор (коллекция) свечек.</param>
        /// <param name="index">Номер свечки в наборе, для которой нужно вернуть значение.</param>
        /// <returns>Возвращает значение TrueRange для свечки из набора.</returns>         
        public static double TrueRange_i(IList<Bar> bars, int index)
        {
            //int count = bars.Count();
            //if (count < index)
            //    return 0;
            if (index == 0)
                return bars[0].High - bars[0].Low;

            double[] values = new double[3];
            Bar item = bars[index];
            double prev_Close = bars[index - 1].Close;

            values[0] = item.High - item.Low;
            values[1] = Math.Abs(item.High - prev_Close);
            values[2] = Math.Abs(item.Low  - prev_Close);

            return values.Max();
        }

        /// <summary>
        /// Вычисляет значение TrueRange для последних значений (количеством period) из набора.
        /// </summary>
        /// <param name="bars">Исходный набор свечек</param>
        /// <param name="period">period</param>
        /// <returns></returns>
        public static IList<double> TrueRange(IList<Bar> bars, int period)
        {
            int count = bars.Count();
            IList<double> result = new List<double>();

            int start = Math.Max(count - period, 0);
            for (int i = start; i < count; i++)
                result.Add(TrueRange_i(bars, i));
            return result;
        }

        /// <summary>
        /// Вычисляет значение TrueRange для последних значений (количеством period) из набора.
        /// </summary>
        /// <param name="bars">Исходный набор свечек</param>
        /// <param name="period">period</param>
        /// <returns></returns>
        public static double AverageTrueRange(IList<Bar> bars, int period)
        {
            int count = bars.Count();
            if (count == 0) return 0;

            IList<double> tr = TrueRange(bars, period);
            //return Math.Round(tr.Average(), 4);
            return tr.Average();
        }

    }
}
