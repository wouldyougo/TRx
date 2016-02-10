using System;
using System.Collections.Generic;

namespace TRx.Indicators
{
    public static partial class Indicator
    {
        //public static IList<bool> CrossOver(IList<double> src1, IList<double> src2)
        //{
        //    int num = Math.Max(src1.Count, src2.Count);
        //    int num1 = Math.Min(src1.Count, src2.Count);
        //    bool[] flagArray = new bool[num];
        //    for (int i = 1; i < num1; i++)
        //    {
        //        flagArray[i] = (src1[i] <= src2[i] ? false : src1[i - 1] <= src2[i - 1]);
        //    }
        //    return flagArray;
        //}

        //public static bool CrossOver(IList<double> src1, IList<double> src2, IList<bool> dst, int i)
        //{
        //    if (i > 1)
        //    {
        //        dst[i] = (src1[i] <= src2[i] ? false : src1[i - 1] <= src2[i - 1]);
        //    }
        //    return dst[i];
        //}
        //public static bool CrossOver(IList<double> src1, IList<double> src2, int i)
        //{
        //    bool dst = false;
        //    if (i > 1)
        //    {
        //        dst = (src1[i] <= src2[i] ? false : src1[i - 1] <= src2[i - 1]);
        //    }
        //    return dst;
        //}

        //public static IList<bool> CrossUnder(IList<double> src1, IList<double> src2)
        //{
        //    int num = Math.Max(src1.Count, src2.Count);
        //    int num1 = Math.Min(src1.Count, src2.Count);
        //    bool[] flagArray = new bool[num];
        //    for (int i = 1; i < num1; i++)
        //    {
        //        flagArray[i] = (src1[i] >= src2[i] ? false : src1[i - 1] >= src2[i - 1]);
        //    }
        //    return flagArray;
        //}

        //public static bool CrossUnder(IList<double> src1, IList<double> src2, IList<bool> dst, int i)
        //{
        //    if (i > 1)
        //    {
        //        dst[i] = (src1[i] >= src2[i] ? false : src1[i - 1] >= src2[i - 1]);
        //    }
        //    return dst[i];
        //}
        //public static bool CrossUnder(IList<double> src1, IList<double> src2, int i)
        //{
        //    bool dst = false;
        //    if (i > 1)
        //    {
        //        dst = (src1[i] >= src2[i] ? false : src1[i - 1] >= src2[i - 1]);
        //    }
        //    return dst;
        //}

        /// <summary>
        /// Пересечение сверху или каксание
        /// </summary>
        /// <param name="src1">линия</param>
        /// <param name="src2">пересекающий</param>
        /// <returns></returns>
        public static bool CrossOver(IList<double> src1, IList<double> src2)
        {
            int i1 = src1.Count;
            int i2 = src2.Count;
            if (src1.Count > 1 && src2.Count > 1)
            {
                return CrossOver(src1, src2, i1, i2);
            }
            else return false;
        }

        /// <summary>
        /// Пересечение сверху или каксание
        /// </summary>
        /// <param name="src1">линия</param>
        /// <param name="src2">пересекающий</param>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static bool CrossOver(IList<double> src1, IList<double> src2, int i1, int i2)
        {
            bool dst = false;
            //if (i1 > 1 && i2 > 1)
            try
            {
                //dst = (src1[i1] <= src2[i2] ? false : src1[i1 - 1] <= src2[i2 - 1]);
                dst = (src1[i1] < src2[i2] ? false : src1[i1 - 1] < src2[i2 - 1]);
            }
            catch
            {
                dst = false;
            }
            return dst;
        }

        /// <summary>
        /// Пересечение снизу или касание
        /// </summary>
        /// <param name="src1">линия</param>
        /// <param name="src2">пересекающий</param>
        /// <returns></returns>
        public static bool CrossUnder(IList<double> src1, IList<double> src2)
        {
            int i1 = src1.Count;
            int i2 = src2.Count;
            if (src1.Count > 1 && src2.Count > 1)
            {
                return CrossUnder(src1, src2, i1, i2);
            }
            else return false;
        }

        /// <summary>
        /// Пересечение снизу или касание
        /// </summary>
        /// <param name="src1">линия</param>
        /// <param name="src2">пересекающий</param>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static bool CrossUnder(IList<double> src1, IList<double> src2, int i1, int i2)
        {
            bool dst = false;
            //if (i1 > 1 && i2 > 1)
            try
            {
                //dst = (src1[i1] >= src2[i2] ? false : src1[i1 - 1] >= src2[i2 - 1]);
                dst = (src1[i1] > src2[i2] ? false : src1[i1 - 1] > src2[i2 - 1]);
            }
            catch
            {
                dst = false;
            }
            return dst;
        }
        /// <summary>
        /// Пересечение сверху или каксание
        /// </summary>
        /// <param name="src1">линия</param>
        /// <param name="src2">пересекающий</param>
        /// <param name="i2">номер элемента данных</param>
        /// <returns></returns>
        public static bool CrossOver(double src1, IList<double> src2, int i2)
        {
            bool dst = false;
            //if (i1 > 1 && i2 > 1)
            try
            {
                //dst = (src1 <= src2[i2] ? false : src1 <= src2[i2 - 1]);
                dst = (src1 < src2[i2] ? false : src1 < src2[i2 - 1]);
            }
            catch
            {
                dst = false;
            }
            return dst;
        }
        /// <summary>
        /// Пересечение снизу или касание
        /// </summary>
        /// <param name="src1">линия</param>
        /// <param name="src2">пересекающий</param>
        /// <param name="i2">номер элемента данных</param>
        /// <returns></returns>
        public static bool CrossUnder(double src1, IList<double> src2, int i2)
        {
            bool dst = false;
            //if (i1 > 1 && i2 > 1)
            try
            {
                //dst = (src1 >= src2[i2] ? false : src1 >= src2[i2 - 1]);
                dst = (src1 > src2[i2] ? false : src1 > src2[i2 - 1]);
            }
            catch
            {
                dst = false;
            }
            return dst;
        }
    }
}
