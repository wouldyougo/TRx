using System;
using System.Collections.Generic;

namespace TRx.Indicators
{
    public static partial class Indicator
	{

        /// <summary>
        /// Разность source1(i) - source1(i - Period)
        /// </summary>
        /// <param name="source1">Уменьшаемое и Вычитаемое</param>
        /// <param name="Period">Период вычитаемого</param>
        /// <returns>Разность</returns>
        public static IList<double> Difference(IList<double> source1, int Period)
        {
            var value = MathHelper.Sub(source1, source1, Period);
            return value;
        }

        /// <summary>
        /// Разность source1(i) - source1(i - 1)
        /// </summary>
        /// <param name="source1">Уменьшаемое и Вычитаемое</param>
        /// <param name="Period">Период вычитаемого</param>
        /// <returns>Разность</returns>
        public static IList<double> Difference(IList<double> source1)
        {
            var value = MathHelper.Sub(source1, source1, 1);
            return value;
        }
    }
}
