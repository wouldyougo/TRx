using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace TSLab.Script.Helpers
namespace TRx.Indicators
{
    public static partial class MathHelper
	{
        public static IList<double> Add(IList<double> src1, IList<double> src2)
        {
            int Count = Math.Max(src1.Count, src2.Count);
            double[] array = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                double num1 = (src1.Count == 0) ? 0.0 : src1[Math.Min(i, src1.Count - 1)];
                double num2 = (src2.Count == 0) ? 0.0 : src2[Math.Min(i, src2.Count - 1)];
                array[i] = num1 + num2;
            }
            return array;
        }

        public static IList<double> Sub(IList<double> src1, IList<double> src2)
        {
            int Count = Math.Max(src1.Count, src2.Count);
            double[] array = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                double num1 = (src1.Count == 0) ? 0.0 : src1[Math.Min(i, src1.Count - 1)];
                double num2 = (src2.Count == 0) ? 0.0 : src2[Math.Min(i, src2.Count - 1)];
                array[i] = num1 - num2;
            }
            return array;
        }
        /// <summary>
        /// Разность source1(i) - source2(i - Period)
        /// </summary>
        /// <param name="source1">Уменьшаемое</param>
        /// <param name="source2">Вычитаемое</param>
        /// <param name="Period">Период вычитаемого</param>
        /// <returns>Разность</returns>
        public static IList<double> Sub(IList<double> source1, IList<double> source2, int Period)
        {
            var value = new double[source1.Count];
            for (int i = Period; i < source1.Count; i++)
            {
                value[i] = source1[i] - source2[i - Period];
            }
            return value;
        }

        public static IList<double> Mul(IList<double> src1, IList<double> src2)
        {
            int Count = Math.Max(src1.Count, src2.Count);
            double[] array = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                double num1 = (src1.Count == 0) ? 0.0 : src1[Math.Min(i, src1.Count - 1)];
                double num2 = (src2.Count == 0) ? 0.0 : src2[Math.Min(i, src2.Count - 1)];
                array[i] = num1 * num2;
            }
            return array;
        }
       
        /// <summary>
        /// Деление source1(i) / source2(i)
        /// </summary>
        /// <param name="source1">Делимоемое</param>
        /// <param name="source2">Делитель</param>
        /// <returns>Результат</returns>        
        public static IList<double> Div(IList<double> src1, IList<double> src2)
        {
            int Count = Math.Max(src1.Count, src2.Count);
            double[] array = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                double num1 = (src1.Count == 0) ? 0.0 : src1[Math.Min(i, src1.Count - 1)];
                double num2 = (src2.Count == 0) ? 0.0 : src2[Math.Min(i, src2.Count - 1)];
                num2 = (num2 == 0) ? 1.0 : num2;
                array[i] = num1 / num2;
            }
            return array;
        }

        public static IList<double> Add(IList<double> src1, double add)
        {
            int Count = src1.Count;
            double[] array = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                array[i] = src1[i] + add;
            }
            return array;
        }

        public static IList<double> Mul(IList<double> src1, double mul)
        {
            int Count = src1.Count;
            double[] array = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                array[i] = src1[i] * mul;
            }
            return array;
        }

        public static IList<double> Abs(IList<double> src1)
        {
            int Count = src1.Count;
            double[] array = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                array[i] = Math.Abs(src1[i]);                
            }
            return array;
        }
        //positive
        //negative        
    }
}
