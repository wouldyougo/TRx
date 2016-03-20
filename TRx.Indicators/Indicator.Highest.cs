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
        public static partial class IndicatorTSLab
        {
            /// <summary>
            /// \~english Calculate highest double value \~russian Рассчитать максимальное значение
            /// </summary>
            /// <param name="p">\~english latest calculation bar \~russian Последние бары в расчете</param>
            /// <param name="period">\~english calculate period \~russian Период расчета</param>
            /// <returns>\~english highest double value \~russian Возвращает максимальное значение</returns>
            public static IList<double> Highest(IList<double> p, int period)
            {
                int count = p.Count;
                double[] numArray = new double[count];
                for (int i = 0; i < count; i++)
                {
                    numArray[i] = Highest(p, i, period);
                }
                return numArray;
            }

            /// <summary>
            /// \~english Calculate highest value \~russian Рассчитать максимальное значение
            /// </summary>
            /// <param name="p">\~english source candles list \~russian Список значений источника</param>
            /// <param name="period">\~english calculation period \~russian Период расчета</param>
            /// <param name="curbar">\~english last calculation bar \~russian Последний рассчитанный бар</param>/// 
            /// <returns>\~english highest value \~russian Максимальное значение</returns>
            public static double Highest(IList<double> p, int period, int curbar)
            {
                double num = double.MinValue;
                int num1 = curbar - period + 1;
                if (num1 < 0)
                {
                    num1 = 0;
                }
                for (int i = num1; i <= curbar; i++)
                {
                    num = Math.Max(num, p[i]);
                }
                return num;
            }
        }

        /// <summary>
        /// \~english Calculate highest value \~russian Рассчитать максимальное значение
        /// </summary>
        /// <param name="p">\~english source candles list \~russian Список значений источника</param>
        /// <param name="period">\~english calculation period \~russian Период расчета</param>
        /// <returns>\~english highest value \~russian Максимальное значение</returns>
        public static double Highest_i(IList<double> p, int period)
        {
            //double num = double.MinValue;
            double num = p.Skip(p.Count - period).Take(period).Max();
            return num;
        }
        /// <summary>
        /// Рассчитать максимальное значение
        /// </summary>
        /// <param name="p">источник</param>
        /// <param name="period">период</param>
        /// <returns>Максимальные значения</returns>
        public static IList<double> Highest(IList<double> p, int period)
        {
            int count = p.Count;
            double[] numArray = new double[count];
            for (int i = 0; i < count; i++)
            {
                numArray[i] = Indicator.Highest_i(p.Take(i+1).Skip(i + 1 - period).ToList(), period);
            }
            return numArray;
        }
    }
}
