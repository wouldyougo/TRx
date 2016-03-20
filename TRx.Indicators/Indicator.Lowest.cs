using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace TSLab.Script.Helpers
namespace TRx.Indicators
{
    public static partial class Indicator
	{
        /// <summary>
        /// пример из TSLab.Script.Helpers
        /// </summary>
        public static partial class IndicatorTSLab
        {
            /// <summary>
            /// \~english Calculate lowest double value \~russian Рассчитать минимальное значение
            /// </summary>
            /// <param name="period">\~english calculate period \~russian Период расчета</param>
            /// <param name="p">\~english latest calculation bar \~russian Последний бар в расчете</param>
            /// <returns>\~english Lowest double value \~russian Возвращает минимальное значение</returns>
            public static IList<double> Lowest(IList<double> p, int period)
            {
                int count = p.Count;
                double[] numArray = new double[count];
                for (int i = 0; i < count; i++)
                {
                    numArray[i] = Lowest(p, i, period);
                }
                return numArray;
            }

            /// <summary>
            /// \~english Calculate lowest value \~russian Рассчитать минимальное значение
            /// </summary>
            /// <param name="curbar">\~english last calculation bar \~russian Последний рассчитанный бар</param>
            /// <param name="period">\~english calculation period \~russian Период расчета</param>
            /// <param name="candles">\~english source candles list \~russian Список источников баров</param>
            /// <returns>\~english lowest value \~russian Минимальное значение</returns>
            public static double Lowest(IList<double> p, int curbar, int period)
            {
                double num = double.MaxValue;
                int num1 = curbar - period + 1;
                if (num1 < 0)
                {
                    num1 = 0;
                }
                for (int i = num1; i <= curbar; i++)
                {
                    num = Math.Min(num, p[i]);
                }
                return num;
            }
        }

        /// <summary>
        /// \~english Calculate highest value \~russian Рассчитать минимальное значение
        /// </summary>
        /// <param name="p">\~english source candles list \~russian Список значений источника</param>
        /// <param name="period">\~english calculation period \~russian Период расчета</param>
        /// <returns>\~english lowest value \~russian Минимальное значение</returns>
        public static double Lowest_i(IList<double> p, int period)
        {
            //double num = double.MinValue;
            double num = p.Skip(p.Count - period).Take(period).Min();
            return num;
        }
        /// <summary>
        /// Рассчитать минимальное значение
        /// </summary>
        /// <param name="p">источник</param>
        /// <param name="period">период</param>
        /// <returns>Минимальные значения</returns>
        public static IList<double> Lowest(IList<double> p, int period)
        {
            int count = p.Count;
            double[] numArray = new double[count];
            for (int i = 0; i < count; i++)
            {
                numArray[i] = Indicator.Lowest_i(p.Take(i+1).Skip(i + 1 - period).ToList(), period);
            }
            return numArray;
        }
    }
}
