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
        /// Пример из TSLab.Script.Helpers
        /// </summary>
        public static partial class IndicatorTSLab
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
            /// <param name="curbar">\~english current bar number \~russian Номер текущего бара</param>
            /// <returns>\~english True Range \~russian Истинный диапазон</returns>
            public static double TrueRange(IList<Bar> candles, int curbar)
            {
                Bar item = candles[curbar];
                double high = item.High;
                double low = item.Low;
                double num = Math.Abs(high - low);
                if (curbar > 0)
                {
                    double close = candles[curbar - 1].Close;
                    num = Math.Max(num, Math.Abs(close - high));
                    num = Math.Max(num, Math.Abs(close - low));
                }
                return num;
            }
        }
        /// <summary>
        /// Пример из QuantConnect.Indicators
        /// </summary>
        public static partial class IndicatorLean
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
}
