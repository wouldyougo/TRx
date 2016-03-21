using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRx.Indicators
{
    public static partial class Indicator
    {
        public static double SMA_i(IList<double> p, int period)
        {
            var window = p.Skip(p.Count - period).Take(period);
            //var sma = window.Sum() / period;
            var sma = window.Sum() / window.Count();
            return sma;
        }
        public static IList<double> SMA(IList<double> p, int period)
        {
            int count = p.Count;
            double[] result = new double[count];
            for (int i = 0; i < count; i++)
            {
                //numArray[i] = Indicator.SMA_i(p.Take(i + 1).Skip(i + 1 - period).ToList(), period);
                result[i] = Indicator.SMA_i(p.Take(i + 1).ToList(), period);
            }
            return result;
        }
        [System.Obsolete("используйте TRx.Indicators.Indicator.SMA")]
        /// <summary>
        /// \~english Calculate SMA for current bar \~russian Рассчитать SMA (простое скользящее среднее)
        /// </summary>
        /// <param name="collection">\~english source candles list \~russian Список источника</param>
        /// <param name="period">\~english SMA period \~russian Период SMA</param>
        /// <returns>\~english SMA \~russian SMA (Простое cкользящее cреднее)</returns>
        public static IEnumerable<double> SMA(IEnumerable<double> collection, int period)
        {
            List<double> result = new List<double>();
            int limit = collection.Count() - period;

            for (int i = 0; i <= limit; i++)
            {
                var sma = collection.Skip(i).Take(period).Sum() / period;
                result.Add(sma);
            }
            return result;
        }
    }
}

//namespace TSLab.Script.Helpers
namespace TRx.Indicators.TSLab
{
    public static partial class Indicator
	{
        /// <summary>
        /// \~english Calculate SMA for current bar \~russian Рассчитать SMA (простое скользящее среднее) для текущего бара
        /// </summary>
        /// <param name="candles">\~english source candles list \~russian Список источников баров</param>
        /// <param name="curbar">\~english current bar number \~russian Номер текущего бара </param>
        /// <param name="period">\~english SMA period \~russian Период SMA (простого скользящего среднего)</param>
        /// <returns>\~english SMA \~russian SMA (Простое cкользящее cреднее)</returns>
        public static double SMA(IList<double> candles, int curbar, int period)
        {
            int num = curbar - period + 1;
            if (num < 0)
            {
                num = 0;
            }
            period = curbar - num + 1;
            double item = 0;
            while (num <= curbar && num < candles.Count)
            {
                int num1 = num;
                num = num1 + 1;
                item = item + candles[num1];
            }
            item = item / (double)Math.Min(period, curbar + 1);
            return item;
        }

        /// <summary>
        /// Рассчитать SMA (простое скользящее среднее) для входящего списка 
        /// </summary>
        /// <param name="candles">Входящий список</param>
        /// <param name="period">Период SMA (простого скользящего среднего)</param>
        /// <returns></returns>
        public static IList<double> SMA(IList<double> candles, int period)
        {
            int count = candles.Count;
            double[] numArray = new double[count];
            for (int i = 0; i <= Math.Min(count - 1, period); i++)
            {
                numArray[i] = Indicator.SMA(candles, i, period);
            }
            for (int j = period; j < count; j++)
            {
                double num4 = candles[j];
                double num5 = candles[j - period];
                double num6 = numArray[j - 1];
                numArray[j] = num6 + ((num4 - num5) / ((double)period));
            }
            return numArray;
        }
    }
}
