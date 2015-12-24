using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Indicators
{
    public class TrueRange
    {
        /// <summary>
        /// Вычисляет значение TrueRange для свечки из набора.
        /// </summary>
        /// <param name="bars">Набор (коллекция) свечек.</param>
        /// <param name="index">Номер свечки в наборе, для которой нужно вернуть значение.</param>
        /// <returns>Возвращает значение TrueRange для свечки из набора.</returns>
        public static double Value(IEnumerable<Bar> bars, int index)
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
        public static IEnumerable<double> Values(IEnumerable<Bar> bars)
        {
            int count = bars.Count();
            
            double[] result = new double[count];

            if (count == 0)
                return result;

            for (int i = 0; i < count; i++)
                result[i] = TrueRange.Value(bars, i);

            return result;
        }
    }
}
