using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Common.Models;

/// <summary>
/// не рекомендуется использовать
/// </summary>
namespace TRx.Indicators.Obsolete
{
    public static partial class Indicator
	{
        [System.Obsolete("используйте TrueRange(IList<Bar> bars, int period)")]
        /// <summary>
        /// Если использовать IList<Bar> bars как окно для исходного списка баров,
        /// то на каждом шаге будет погрешность при вычислении первого значения TrueRange
        /// Получить набор значений TrueRange для массива свечек.
        /// </summary>
        /// <param name="bars">Исходный набор свечек.</param>
        /// <returns>Набор результирующих значений TrueRange для всех свечей из исходного набора.</returns>
        public static IList<double> TrueRange(IList<Bar> bars)
        {
            int count = bars.Count();
            double[] result = new double[count];
            //if (count == 0) return result;
            for (int i = 0; i < count; i++)
                result[i] = TRx.Indicators.BarSource.Indicator.TrueRange_i(bars, i);
            return result;
        }

        [System.Obsolete("используйте AverageTrueRange(IList<Bar> bars, int period)")]
        /// <summary>
        /// Если использовать IList<Bar> bars как окно для исходного списка баров,
        /// то на каждом шаге будет погрешность при вычислении первого значения TrueRange
        /// </summary>
        /// <param name="bars"></param>
        /// <returns></returns>
        public static double AverageTrueRange(IList<Bar> bars)
        {
            int count = bars.Count();
            if (count == 0) return 0;

            double[] tr = new double[count];
            for (int i = 0; i < count; i++)
                tr[i] = TRx.Indicators.BarSource.Indicator.TrueRange_i(bars, i);

            //return Math.Round(tr.Average(), 4);
            return tr.Average();
        }
    }
}
/// <summary>
/// Пример из TSLab.Script.Helpers
/// </summary>
namespace TRx.Indicators.TSLab
{
    /// <summary>
    /// Пример из TSLab.Script.Helpers
    /// </summary>
    public static partial class Indicator
    {
        /// <summary>
        /// \~english ATR helper \~russian Помощник ATR (средний истинный диапазон)
        /// </summary>
        /// <param name="candles"></param>
        /// <param name="start"></param>
        /// <param name="period"></param>
        /// <param name="prevAtr"></param>
        /// <returns></returns>
        public static double AverageTrueRange(IList<Bar> candles, int start, int period, double prevAtr)
        {
            if (start - period > 0)
            {
                return (prevAtr * (double)(period - 1) + TrueRange(candles, start)) / (double)period;
            }
            double num = prevAtr * (double)start;
            num = num + TrueRange(candles, start);
            return num / (double)(start + 1);
        }

        /// <summary>
        /// \~english Calculate Average True Range for the all candles \~russian Рассчитать ATR (средний истинный диапазон) для всех свечей
        /// </summary>
        /// <param name="candles">\~english source candles list \~russian Список свечей</param>
        /// <param name="period">\~english ATR period \~russian Период ATR</param>
        /// <returns>\~english ATRs list \~russian Список ATR</returns>
        public static IList<double> AverageTrueRange(IList<Bar> candles, int period)
        {
            double num = 0;
            int count = candles.Count;
            double[] numArray = new double[count];
            for (int i = 0; i < count; i++)
            {
                double num1 = AverageTrueRange(candles, i, period, num);
                double num2 = num1;
                numArray[i] = num1;
                num = num2;
            }
            return numArray;
        }
        /// <summary>
        /// \~english Calculate True Range for current bar \~russian Рассчитать истинный диапазон для текущего бара
        /// </summary>
        /// <param name="candles">\~english source candles list \~russian Список источников баров</param>
        /// <param name="index">\~english current bar number \~russian Номер текущего бара</param>
        /// <returns>\~english True Range \~russian Истинный диапазон</returns>
        public static double TrueRange(IList<Bar> candles, int index)
        {
            Bar item = candles[index];
            double high = item.High;
            double low = item.Low;
            double num = Math.Abs(high - low);
            if (index > 0)
            {
                double close = candles[index - 1].Close;
                num = Math.Max(num, Math.Abs(close - high));
                num = Math.Max(num, Math.Abs(close - low));
            }
            return num;
        }
    }
}
/// <summary>
///  Пример из TRL
/// </summary>
namespace TRx.Indicators.TRL
{
    /// <summary>
    /// Пример из TRL
    /// </summary>
    public static partial class Indicator
    {
        /// <summary>
        /// Вычисляет значение TrueRange для свечки из набора.
        /// </summary>
        /// <param name="bars">Набор (коллекция) свечек.</param>
        /// <param name="index">Номер свечки в наборе, для которой нужно вернуть значение.</param>
        /// <returns>Возвращает значение TrueRange для свечки из набора.</returns>
        public static double TrueRange(IEnumerable<Bar> bars, int index)
        {
            int count = bars.Count();

            if (count < index)
                return 0;

            Bar[] bArray = bars.ToArray();

            if (index == 0)
            {
                return bArray[0].High - bArray[0].Low;
            }

            double[] values = new double[3];

            values[0] = bArray[index].High - bArray[index].Low;
            values[1] = Math.Abs(bArray[index].High - bArray[index - 1].Close);
            values[2] = Math.Abs(bArray[index].Low - bArray[index - 1].Close);

            return values.Max();
        }

        /// <summary>
        /// Получить набор значений TrueRange для массива свечек.
        /// </summary>
        /// <param name="bars">Исходный набор свечек.</param>
        /// <returns>Набор результирующих значений TrueRange для всех свечей из исходного набора.</returns>
        public static IEnumerable<double> TrueRange(IEnumerable<Bar> bars)
        {
            int count = bars.Count();

            double[] result = new double[count];

            if (count == 0)
                return result;

            for (int i = 0; i < count; i++)
                result[i] = TrueRange(bars, i);

            return result;
        }
    }
}

/// <summary>
/// Пример из QuantConnect.Indicators
/// </summary>
namespace TRx.Indicators.Lean
{
    /// <summary>
    /// Пример из QuantConnect.Indicators
    /// </summary>
    public static partial class Indicator
    {
        /// <summary>
        /// Computes the TrueRange from the current and previous trade bars
        /// 
        /// TrueRange is defined as the maximum of the following:
        ///   High - Low
        ///   ABS(High - PreviousClose)
        ///   ABS(Low  - PreviousClose)
        /// </summary>
        /// <param name="previous">The previous trade bar</param>
        /// <param name="current">The current trade bar</param>
        /// <returns>The true range</returns>
        public static double ComputeTrueRange(Bar previous, Bar current)
        {
            var range1 = current.High - current.Low;
            if (previous == null)
            {
                return range1;
            }

            var range2 = Math.Abs(current.High - previous.Close);
            var range3 = Math.Abs(current.Low - previous.Close);

            return Math.Max(range1, Math.Max(range2, range3));
        }
    }
}